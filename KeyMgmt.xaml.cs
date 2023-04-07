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
using Passwordless_Authenticator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using static System.Net.Mime.MediaTypeNames;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Passwordless_Authenticator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class KeyMgmt : Page
    {
        public static string filePath;
        public KeyMgmt()
        {
            this.InitializeComponent();
        }
        private async void ExportKs(string folderPath)
        {
            // DbUtils.populateDB(); /* remove this function after integrating with sign up */
            string skey = CryptoUtils.getUniqueKey(16);
            string sIV = CryptoUtils.getUniqueKey(16);
            string encres = await KeyUtils.exportKeys(folderPath, skey);

            if (encres == "File encrypted successfully.")
            {
                TextB1Brdr.Visibility = Visibility.Visible;
                TextB1.Text = "Database encrypted. \nKey: " + skey + "\nUse this key to decrypt the DB.";
            }
            else
            {
                TextB1Brdr.Visibility = Visibility.Visible;
                TextB1.Text = encres;
            }
        }

        private void ImportKs(string filePath)
        {
            // KeyUtils.importKeys();
        }

        private async void checkDec(object sender, RoutedEventArgs e)
        {
            string skey = enterKey.Password;
            string opres = await KeyUtils.importKeys(filePath, skey);
            TextB1Brdr.Visibility  = Visibility.Visible;
            TextB1.Text = opres;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            List<string> key_service = e.Parameter as List<string>;

            if (key_service[0] == "export") 
            {
                ExportKs(key_service[1]);
            }

            else if (key_service[0] == "import")
            {
                enterKeyBrdr.Visibility = Visibility.Visible;
                submitDecBrdr.Visibility = Visibility.Visible;
                TextB1Brdr.Visibility = Visibility.Visible;
                TextB1.Text = "Enter the key to decrypt the database";
                filePath = key_service[1];
            }

            base.OnNavigatedTo(e);
        }

        private void goHome(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));
        }

    }
}
