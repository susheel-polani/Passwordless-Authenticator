// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Passwordless_Authenticator.Models;
using Passwordless_Authenticator.Services.Auth;
using Passwordless_Authenticator.Services.Crypto;
using Passwordless_Authenticator.Services.HTTPServer;
using Passwordless_Authenticator.Services.SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Passwordless_Authenticator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
        }

       private async void login(object sender, RoutedEventArgs e)
        {
            string preference = UserPrefDB.GetPref();
            if (preference == "Custom")
            {
                enterPassBrdr.Visibility = Visibility.Visible;
                submitPassBrdr.Visibility = Visibility.Visible;
                appLoginBrdr.Visibility = Visibility.Collapsed;

            }
            else
            {
                WindowsAuthData result = await AppAuthenticationService.authenticate("Enter windows PIN to log in");
                if (result.message == "Logged In Successfully")
                {
                    WebInterfaceServer.startServer();
                    DataAccess.setUpDatabase();
                    this.Frame.Navigate(typeof(HomePage));
                }
                else
                {
                    TextB1Brdr.Visibility = Visibility.Visible;
                    TextB1.Text = result.message;
                };
            }


        }

        private void setPassword(object sender, RoutedEventArgs e)
        {
            TextB1Brdr.Visibility = Visibility.Visible;
            TextB1.Text = "Enter your new password";
            setPassBrdr.Visibility = Visibility.Collapsed;
            enterNewPassBrdr.Visibility = Visibility.Visible;
            submitNewPassBrdr.Visibility = Visibility.Visible;
        }


        private void checkPassword(object sender, RoutedEventArgs e)
        {
            try
            {
                bool check = PasswordService.checkPassword(enterPass.Password);
                if (check)
                {
                    WebInterfaceServer.startServer();
                    DataAccess.setUpDatabase();
                    this.Frame.Navigate(typeof(HomePage));
                }
                else
                {
                    TextB1Brdr.Visibility = Visibility.Visible;
                    TextB1.Text = "Incorrect password, try again";
                    forgotPassBrdr.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                TextB1.Text = ex.ToString();
            }
        }

        private void setNewPassword(object sender, RoutedEventArgs e)
        {
            string userPass = enterNewPass.Password;
            if (userPass.Length == 0)
            {
                TextB1.Text = "Please enter a valid password";
            }
            else
            {
                try
                {
                    string userPassHash = CryptoUtils.hashData(userPass);
                    string recoveryKey = CryptoUtils.getUniqueKey(30);
                    string recoveryKeyHash = CryptoUtils.hashData(recoveryKey);
                    PasswordDB.AddPassword(userPassHash, recoveryKeyHash);
                    UserPrefDB.SetPref("Custom");
                    string op_message = "Custom password set up as authentication. Your recovery key is: \n" + recoveryKey + "\nKeep this Key handy as you will need it in case you forget your password!";
                    this.Frame.Navigate(typeof(AuthSetup), op_message);
                }
                catch (Exception ex)
                {
                    TextB1.Text = ex.ToString();
                }
            }
        }

        private void forgotPassword(object sender, RoutedEventArgs e)
        {
            TextB1.Text = "Enter your recovery key to change password";
            enterPassBrdr.Visibility = Visibility.Collapsed;
            submitPassBrdr.Visibility = Visibility.Collapsed;
            enterRecoveryKeyBrdr.Visibility = Visibility.Visible;
            submitKeyBrdr.Visibility = Visibility.Visible;
            forgotPassBrdr.Visibility = Visibility.Collapsed;
        }

        private void submitRecoveryKey(object sender, RoutedEventArgs e)
        {
            bool check = PasswordService.checkKey(enterRecoveryKey.Password);
            if (check)
            {
                TextB1.Text = "Key entered successfully, change your password";
                enterRecoveryKeyBrdr.Visibility = Visibility.Collapsed;
                submitKeyBrdr.Visibility = Visibility.Collapsed;
                enterNewPassBrdr.Visibility = Visibility.Visible;
                submitNewPassBrdr.Visibility = Visibility.Visible;
            }
            else
            {
                TextB1.Text = "Incorrect Key, please try again";
            }
        }
    }
}
