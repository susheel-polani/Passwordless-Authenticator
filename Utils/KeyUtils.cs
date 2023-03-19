using Microsoft.UI.Xaml.Controls;
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
        public static async void exportKeys(string pass, string iv)
        {
            var folderPicker = new FolderPicker();

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {

                DbUtils.copyDB();
                DbUtils.appendXML(AppConstants.COPY_DB_PATH);

                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);

                string enc_path = Path.Combine(folder.Path, "encrypted.db");
                File.Copy(AppConstants.COPY_DB_PATH, AppConstants.ENC_DB_PATH);
                FileEncryptionService.EncryptFile(AppConstants.ENC_DB_PATH, enc_path, pass, iv);
                File.Delete(AppConstants.ENC_DB_PATH);
            }

            else
            {
                Debug.WriteLine("No file selected");
            }
        }

        public static async void importKeys()
        {
            var filePicker = new FileOpenPicker();

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

            filePicker.FileTypeFilter.Add(".db");
            var file = await filePicker.PickSingleFileAsync();

            if (file != null)
            {
                FileEncryptionService.DecryptFile(file.Path, AppConstants.IMP_DB_PATH, "test", "test");

                DbUtils.importDB();

            }
            
            else
            {
                Debug.WriteLine("No file selected");
            }


        }
    }
}
