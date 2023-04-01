﻿using Microsoft.UI.Xaml.Controls;
using Passwordless_Authenticator.Constants;
using Passwordless_Authenticator.Services.Crypto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Passwordless_Authenticator.Utils
{

    [ComImport]
    [Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IInitializeWithWindow
    {
        void Initialize(IntPtr hwnd);
    }
    internal class KeyUtils
    {
        // add export and import key functionality here
        public static async Task<string> exportKeys(string folderPath, string pass, string iv)
        {

            /*
            var folderPicker = new FolderPicker();

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            */

            if (folderPath != null)
            {
                    DbUtils.copyDB();
                    DbUtils.appendXML(AppConstants.COPY_DB_PATH);

                    // Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folderPath);

                    string enc_path = Path.Combine(folderPath, "encrypted.db");
                    File.Copy(AppConstants.COPY_DB_PATH, AppConstants.ENC_DB_PATH);
                    string encres = FileEncryptionService.EncryptFile(enc_path, pass, iv);
                    File.Delete(AppConstants.ENC_DB_PATH);
                    return encres;

            }

            else
            {
                Debug.WriteLine("No file selected");
                return null;

            }
        }

        public static async Task<string> importKeys(string filePath, string pass, string iv)
        {
            /*
            var filePicker = new FileOpenPicker();

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

            filePicker.FileTypeFilter.Add(".db");
            var file = await filePicker.PickSingleFileAsync();
            */

            if (filePath != null)
            {
                string decres = FileEncryptionService.DecryptFile(filePath, pass, iv);
                if (decres == "File decrypted Successfully.\n")
                {
                    string impres = await DbUtils.importDB();
                    return decres + impres;
                }
                else
                {
                    return decres;
                }

            }
            
            else
            {
                Debug.WriteLine("No file selected");
                return null;
            }


        }
    }
}
