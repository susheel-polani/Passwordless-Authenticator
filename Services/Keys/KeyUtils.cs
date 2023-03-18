using Microsoft.UI.Xaml.Controls;
using Passwordless_Authenticator.Constants;
using Passwordless_Authenticator.Services.Crypto;
using Passwordless_Authenticator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Passwordless_Authenticator.Services.Keys
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
        public static async void exportKeys()
        {
            DbUtils.copyDB();
            DbUtils.appendXML(AppConstants.COPY_DB_PATH);

            var folderPicker = new Windows.Storage.Pickers.FolderPicker();

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);

            string enc_path = Path.Combine(folder.Path, "encrypted.db");
            File.Copy(AppConstants.COPY_DB_PATH, AppConstants.ENC_DB_PATH);
            FileEncryptionService.EncryptFile(AppConstants.ENC_DB_PATH, enc_path, "passwordkeyvault");
            File.Delete(AppConstants.ENC_DB_PATH);
        }
    }
}
