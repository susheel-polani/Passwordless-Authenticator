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
using Passwordless_Authenticator.Services.SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Gaming.Input.ForceFeedback;
using Windows.Security.Credentials;
using Windows.System.UserProfile;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Passwordless_Authenticator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LandingPage : Page
    {
        public LandingPage()
        {
            this.InitializeComponent();
            setAuthMethod();
        }

        private async void setAuthMethod()
        {

            bool authAvail = await AppAuthenticationService.isAuthSetup();
            if (authAvail == false)
            {
                // Key credential is not enabled yet as user 
                // needs to connect to a Microsoft Account and select a PIN in the connecting flow.
                TextB1.Text = "Windows Hello is not setup!\nPlease go to Windows Settings and set it up, or use a custom password";
                useCustom.Visibility = Visibility.Visible;
            }

            else
            {
                TextB1.Text = "Windows Hello available. Do you want to use that for authentication?";
                usePassport.Visibility = Visibility.Visible;
                useCustom.Visibility = Visibility.Visible;
            }

        }

        private void goToSetPass(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage1));
        }

        private async void useWindows(object sender, RoutedEventArgs e)
        {
            WindowsAuthData result = await AppAuthenticationService.authenticate("Enter windows PIN to set it as authentication method");
            if (result.message == "Logged In Successfully")
            {
                UserPrefDB.SetPref("WindowsHello");
                string op_message = "Windows Hello set up as authentication";
                this.Frame.Navigate(typeof(AuthSetup), op_message);

            }
            else
            {
                TextB1.Text = result.message;
            }

        }
    }
}
