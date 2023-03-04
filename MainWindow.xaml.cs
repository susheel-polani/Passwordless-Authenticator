// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Passwordless_Authenticator.Services.HTTPServer;
using Passwordless_Authenticator.Services.SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

using Microsoft.Extensions.Configuration;
using Passwordless_Authenticator.Constants;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using Passwordless_Authenticator.Dao_controllers;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using System.Threading.Tasks;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Passwordless_Authenticator
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private AppWindow _appWindow;
        public MainWindow()
        {
            this.InitializeComponent();
            WebInterfaceServer.startServer();
            DataAccess.setUpDatabase();

            this.Closed += OnClosed;

            _appWindow = GetAppWindowForCurrentWindow();
            _appWindow.Closing += OnClosing;
        }

            

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            myButton.Content = "Clicked";

            try {
                UserAuthDataController.test();
            } catch (Exception ex) { 
            
                Debug.WriteLine(ex);
            }

           
        }

        private void OnClosed(object sender, WindowEventArgs e)
        {
            string btnText = myButton.Content.ToString();
        }


        private async void OnClosing(object sender, AppWindowClosingEventArgs e)
        {

            string btnText = myButton.Content.ToString();

            e.Cancel = true;     //Cancel close
                                 //Otherwise, the program will not wait for the task to execute, and the main thread will close immediately

            //await System.Threading.Tasks.Task.Delay(5000); //wait for 5 seconds (= 5000ms)

            //this.Close();   //close
        }


        private AppWindow GetAppWindowForCurrentWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(myWndId);
        }
    }
}
