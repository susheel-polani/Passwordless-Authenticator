// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Passwordless_Authenticator.Services.SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.MediaProperties;
using Passwordless_Authenticator.Services.HTTPServer;
using Passwordless_Authenticator.Models;
using Passwordless_Authenticator.Services.Auth;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Passwordless_Authenticator
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    /// 

    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        /// 


        string setting;
        public App()
        {
            this.InitializeComponent();
            WebInterfaceServer.startServer();
            DataAccess.setUpDatabase();
        }

        private async void initApp()
        {
            try
            {
                await PasswordDB.InitializePwdDatabase();
                await UserPrefDB.InitializeUsrPrfDatabase();
                setting = UserPrefDB.GetPref();
                if (setting == "Empty")
                {
                    m_window = new MainWindow();
                    m_window.Activate();

                }
                else
                {
                    string authmessage = "Enter credentials to authenticate to the app";
                    WindowsAuthData auth_res = await AppAuthenticationService.authenticateUser(authmessage);
                    if (auth_res.flag == true)
                    {
                        m_window = new MainWindow();
                        m_window.Activate();
                    }
                }
            }
            catch
            {
                Debug.WriteLine("DB init failed");
            }
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            //m_window = new MainWindow();
            //m_window.Activate();
            initApp();
        }

        public static Window m_window;
    }
}
