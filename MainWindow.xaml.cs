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
using Passwordless_Authenticator.Services.HTTPServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Passwordless_Authenticator.Services.SQLite;
using System.ComponentModel.Design;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Passwordless_Authenticator
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
           this.InitializeComponent();
            WebInterfaceServer.startServer();

            authenticateUser();
        }

        private async void authenticateUser()
        {
            var authAvail = AppAuthenticationService.isWindowsAuthAvailable();
            WindowsAuthData test = authAvail.Result;

            if (test.message == "Windows Hello Authentication is available.")
            {
                var data = AppAuthenticationService.authenticate();
                WindowsAuthData result = data.Result;

                if (result.message == "Logged In Successfully")
                {
                    this.Frame
                }

            }

        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";

        }
    }
}
