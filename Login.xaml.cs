// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Passwordless_Authenticator.Services.SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

        private void App_Login(object sender, RoutedEventArgs e)
        {
            string inputPassword = passwordBox2.Password;

            string actualPassword = PasswordDB.GetPassword();

            if (inputPassword == actualPassword)
            {
                m_window = new MainWindow();
                Frame rootFrame = new Frame();

                m_window.Content = rootFrame;

                rootFrame.Navigate(typeof(MainWindow));
                m_window.Activate();
            }
            else
            {
                Submit.Content = "Login Failed";
            }
        }

        private Window m_window;
        }   

}
