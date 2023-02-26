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
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void goToLogin(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Login));
        }

        private async void changeAuthMethod(object sender, RoutedEventArgs e)
        {
            changeAuthentication.Visibility = Visibility.Collapsed;
            string preference = UserPrefDB.GetPref();
            if (preference == "Custom")
            {
                TextB2.Text = "Current authentication mechanism is a custom password";
                changePass.Visibility = Visibility.Visible;
                bool authAvail = await AppAuthenticationService.isAuthSetup();
                if (authAvail == false)
                {
                    // Key credential is not enabled yet as user 
                    // needs to connect to a Microsoft Account and select a PIN in the connecting flow.
                    TextB1.Text = "Windows Hello is not setup!\nPlease go to Windows Settings and set it up to use it as an option";
                }

                else
                {
                    TextB1.Text = "Windows Hello available";
                    useHello.Visibility = Visibility.Visible;
                }

            }
            else
            {
                TextB2.Text = "Current authentication mechanism is Windows Hello";
                setPass.Visibility = Visibility.Visible;
            }
        }

        private void setPassword(object sender, RoutedEventArgs e)
        {
            TextB1.Text = "Enter your new password";
            setPass.Visibility = Visibility.Collapsed;
            useHello.Visibility = Visibility.Collapsed;
            enterNewPass.Visibility = Visibility.Visible;
            submitNewPass.Visibility = Visibility.Visible;
        }

        private void changePassword(object sender, RoutedEventArgs e)
        {
            enterPass.Visibility = Visibility.Visible;
            submitPass.Visibility = Visibility.Visible;
            changePass.Visibility = Visibility.Collapsed;
            TextB1.Text = "Enter your password to set a new one";
            useHello.Visibility = Visibility.Collapsed;
        }

        private void checkPassword(object sender, RoutedEventArgs e)
        {
            bool check = PasswordService.checkPassword(enterPass.Password);
            if (check)
            {
                TextB1.Text = "Password entered correctly, enter your new password";
                enterPass.Visibility = Visibility.Collapsed;
                submitPass.Visibility = Visibility.Collapsed;
                enterNewPass.Visibility = Visibility.Visible;
                submitNewPass.Visibility = Visibility.Visible;
            }
            else
            {
                TextB1.Text = "Incorrect Password, try again";
                forgotPass.Visibility = Visibility.Visible;
            }
        }

        private void setNewPassword(object sender, RoutedEventArgs e)
        {
            string userPass = enterNewPass.Password;
            string userPassHash = CryptoUtils.hashData(userPass);
            string recoveryKey = CryptoUtils.getUniqueKey(30);
            string recoveryKeyHash = CryptoUtils.hashData(recoveryKey);
            PasswordDB.AddPassword(userPassHash, recoveryKeyHash);
            UserPrefDB.SetPref("Custom");
            TextB1.Text = "Password set successfully. Your recovery key is:" + recoveryKey;
            enterNewPass.Visibility = Visibility.Collapsed;
            submitNewPass.Visibility = Visibility.Collapsed;
            reloadHome.Visibility = Visibility.Visible;
        }

        private void forgotPassword (object sender, RoutedEventArgs e)
        {
            TextB1.Text = "Enter your recovery key to change password";
            enterPass.Visibility = Visibility.Collapsed;
            submitPass.Visibility = Visibility.Collapsed;
            enterRecoveryKey.Visibility = Visibility.Visible;
            submitKey.Visibility = Visibility.Visible;
            forgotPass.Visibility = Visibility.Collapsed;
        }

        private void submitRecoveryKey(object sender, RoutedEventArgs e)
        {
            bool check = PasswordService.checkKey(enterRecoveryKey.Password);
            if (check)
            {
                TextB1.Text = "Key entered successfully, change your password";
                enterRecoveryKey.Visibility = Visibility.Collapsed;
                submitKey.Visibility = Visibility.Collapsed;
                enterNewPass.Visibility = Visibility.Visible;
                submitNewPass.Visibility = Visibility.Visible;
            }
            else
            {
                TextB1.Text = "Incorrect Key, please try again";
            }
        }

        private void reloadHomePage(object sender, RoutedEventArgs e)
        {
            changeAuthentication.Visibility = Visibility.Visible;
            reloadHome.Visibility = Visibility.Collapsed;
            TextB1.Text = "HOME PAGE.";
            TextB2.Text = "HOME PAGE.";

        }
        private async void useWinHello(object sender, RoutedEventArgs e)
        {

            WindowsAuthData result = await AppAuthenticationService.authenticate();
            if (result.message == "Logged In Successfully")
            {
                UserPrefDB.SetPref("WindowsHello");
                TextB1.Text = "Windows Hello set as authentication mechanism";
                this.Frame.Navigate(typeof(HomePage));

            }

        }
    }
}
