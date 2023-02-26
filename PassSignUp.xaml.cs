// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Passwordless_Authenticator.Services.Crypto;
using Passwordless_Authenticator.Services.SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passwordBox1.Password))
            {
                TextB1.Text = "Password cannot be empty";
                TextB1.Visibility = Visibility.Visible;
            }

            else
            {
                string userPass = passwordBox1.Password;
                string userPassHash = CryptoUtils.hashData(userPass);
                string recoveryKey = CryptoUtils.getUniqueKey(30);
                string recoveryKeyHash = CryptoUtils.hashData(recoveryKey);
                PasswordDB.AddPassword(userPassHash, recoveryKeyHash);
                TextB1.Text = "Password set successfully. Your recovery key is:" + recoveryKey;

                passwordBox1.Visibility = Visibility.Collapsed;
                textBlockPassword.Visibility = Visibility.Collapsed;
                TextB1.Visibility = Visibility.Visible;
                Submit.Visibility = Visibility.Collapsed;
                goToHome.Visibility = Visibility.Visible;
                UserPrefDB.SetPref("Custom");

            }
        }

        private void goHome(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));
        }
    }
}
