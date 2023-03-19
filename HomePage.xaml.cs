// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using Passwordless_Authenticator.Constants;
using Passwordless_Authenticator.Models;
using Passwordless_Authenticator.Services.Auth;
using Passwordless_Authenticator.Services.Crypto;
using Passwordless_Authenticator.Services.HTTPServer;
using Passwordless_Authenticator.Services.SQLite;
using Passwordless_Authenticator.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

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

        private async void changeAuthMethod(object sender, RoutedEventArgs e)
        {
            reloadHomeBrdr.Visibility = Visibility.Visible;
            exportKBrdr.Visibility = Visibility.Collapsed;
            importKBrdr.Visibility = Visibility.Collapsed;
            changeAuthenticationBrdr.Visibility = Visibility.Collapsed;
            try
            {
                string preference = UserPrefDB.GetPref();
                if (preference == "Custom")
                {
                    bool authAvail = await AppAuthenticationService.isAuthSetup();
                    if (authAvail == false)
                    {
                        // Key credential is not enabled yet as user 
                        // needs to connect to a Microsoft Account and select a PIN in the connecting flow.
                        TextB1Brdr.Visibility = Visibility.Visible;
                        TextB1.Text = "Windows Hello is not setup!\nPlease go to Windows Settings and set it up to use it as an option";
                    }

                    else
                    {
                        useHelloBrdr.Visibility = Visibility.Visible;
                        changePassBrdr.Visibility = Visibility.Visible;
                    }

                }
                else
                {
                    setPassBrdr.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                TextB1Brdr.Visibility = Visibility.Visible;
                TextB1.Text = ex.ToString();
            }
        }

        private void setPassword(object sender, RoutedEventArgs e)
        {
            TextB1Brdr.Visibility = Visibility.Visible;
            TextB1.Text = "Enter your new password";
            setPassBrdr.Visibility = Visibility.Collapsed;
            useHelloBrdr.Visibility = Visibility.Collapsed;
            enterNewPassBrdr.Visibility = Visibility.Visible;
            submitNewPassBrdr.Visibility = Visibility.Visible;
        }

        private void changePassword(object sender, RoutedEventArgs e)
        {
            enterPassBrdr.Visibility = Visibility.Visible;
            submitPassBrdr.Visibility = Visibility.Visible;
            changePassBrdr.Visibility = Visibility.Collapsed;
            TextB1Brdr.Visibility = Visibility.Visible;
            TextB1.Text = "Enter your password to set a new one";
            useHelloBrdr.Visibility = Visibility.Collapsed;
        }

        private void checkPassword(object sender, RoutedEventArgs e)
        {
            try
            {
                bool check = PasswordService.checkPassword(enterPass.Password);
                if (check)
                {
                    TextB1.Text = "Password entered correctly, enter your new password";
                    enterPassBrdr.Visibility = Visibility.Collapsed;
                    submitPassBrdr.Visibility = Visibility.Collapsed;
                    enterNewPassBrdr.Visibility = Visibility.Visible;
                    submitNewPassBrdr.Visibility = Visibility.Visible;
                }
                else
                {
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

        private void forgotPassword (object sender, RoutedEventArgs e)
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

        private void reloadHomePage(object sender, RoutedEventArgs e)
        {
            changeAuthenticationBrdr.Visibility = Visibility.Visible;
            exportKBrdr.Visibility = Visibility.Visible;
            importKBrdr.Visibility = Visibility.Visible;

            reloadHomeBrdr.Visibility = Visibility.Collapsed;
            changePassBrdr.Visibility = Visibility.Collapsed;
            useHelloBrdr.Visibility = Visibility.Collapsed;
            enterPassBrdr.Visibility = Visibility.Collapsed;
            submitPassBrdr.Visibility = Visibility.Collapsed;
            setPassBrdr.Visibility = Visibility.Collapsed;
            forgotPassBrdr.Visibility = Visibility.Collapsed;
            enterRecoveryKeyBrdr.Visibility = Visibility.Collapsed;
            submitKeyBrdr.Visibility = Visibility.Collapsed;
            enterNewPassBrdr.Visibility = Visibility.Collapsed;
            submitNewPassBrdr.Visibility = Visibility.Collapsed;
            reloadHomeBrdr.Visibility = Visibility.Collapsed;
            TextB1Brdr.Visibility = Visibility.Collapsed;


            TextB1.Text = "HOME PAGE.";
        }
        private async void useWinHello(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowsAuthData result = await AppAuthenticationService.authenticate("Enter windows PIN to set it as authentication method");
                if (result.message == "Logged In Successfully")
                {
                    UserPrefDB.SetPref("WindowsHello");
                    string op_message = "Windows Hello set up as authentication";
                    this.Frame.Navigate(typeof(HomePage));

                }
                else
                {
                    TextB1.Text = result.message;
                }
            }
            catch (Exception ex)
            {
                TextB1.Text = ex.ToString();
            }

        }

        private async void genPrompt(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowsAuthData result = await AppAuthenticationService.authenticateUser("Enter your credentials to authenticate");
                TextB1.Text = result.flag.ToString() + "." + result.message;
            }
            catch (Exception ex)
            {
                TextB1.Text = ex.ToString();
            }
        }

        private async void exportKs(object sender, RoutedEventArgs e)
        {
            DbUtils.populateDB();
            string encryptPass = CryptoUtils.getUniqueKey(16);
            string encryptIV = CryptoUtils.getUniqueKey(16);
            KeyUtils.exportKeys(encryptPass, encryptIV);

            TextB1Brdr.Visibility = Visibility.Visible;
            TextB1.Text = "Database encrypted. Your key is : " + encryptPass + " and your IV is : " + encryptIV + ". Use these to decrypt the DB.";
        }

        private async void importKs(object sender, RoutedEventArgs e)
        {
            KeyUtils.importKeys();
        }

        private void pageLoaded(object sender, RoutedEventArgs e)
        {
            string preference = UserPrefDB.GetPref();
            if (preference == "Custom")
            {
                currentMethod.Text = " Custom Password";
            }
            else
            {
                currentMethod.Text = " Windows Hello";
            }
        }

    }
}
