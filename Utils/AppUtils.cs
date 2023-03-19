using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Passwordless_Authenticator.Utils
{
    internal class AppUtils
    {
        public static string getUUID() {
            Guid guid= Guid.NewGuid();
            return guid.ToString();
        }

        
        public static async Task<StorageFile> filePicker()
        {
            var filePicker = new FileOpenPicker();

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

            filePicker.FileTypeFilter.Add(".db");
            var file = await filePicker.PickSingleFileAsync();
            return file;
        }

        public static async Task<StorageFolder> folderPicker()
        {
            var folderPicker = new FolderPicker();

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            return folder;
        }
        
    }
}
