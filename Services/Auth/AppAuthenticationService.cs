using Passwordless_Authenticator.Constants.Auth;
using Passwordless_Authenticator.Constants;
using Passwordless_Authenticator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials.UI;
using Windows.Security.Credentials;
using Microsoft.UI.Xaml.Controls;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Meziantou.Framework.Win32;
using Passwordless_Authenticator.Services.SQLite;

namespace Passwordless_Authenticator.Services.Auth
{
    internal class AppAuthenticationService
    {
        /// <summary>
        /// This Method checks for the availability of windows hello authenticatiom
        /// </summary>
        /// <returns>
        /// Class <c>WindowsAuthData</c>
        /// </returns>
        public static async Task<WindowsAuthData> isWindowsAuthAvailable()
        {
            try
            {
                var windowsAuthAvailability = await UserConsentVerifier.CheckAvailabilityAsync();

                return AppAuthConstants.getAvailabilityData(windowsAuthAvailability);
            }
            catch
            {
                return new WindowsAuthData(false, AppConstants.GENERIC_ERROR);
            }

        }

        /// <summary>
        /// This Method authenticates the user using windows hello authentication
        /// </summary>
        /// <returns>
        /// Class <c>WindowsAuthData</c>
        /// </returns>
        public static async Task<WindowsAuthData> authenticate(string[] args)
        {

            try
            {
                if (args is null)
                {
                    var windowsAuthVerification = await UserConsentVerifier.RequestVerificationAsync(AppAuthConstants.APP_AUTH_PROMPT_MSG);
                    return AppAuthConstants.getVerificationData(windowsAuthVerification);
                }
                else
                {
                    var windowsAuthVerification = await UserConsentVerifier.RequestVerificationAsync(args[0]);
                    return AppAuthConstants.getVerificationData(windowsAuthVerification);
                }
            }
            catch
            {
                return new WindowsAuthData(false, AppConstants.GENERIC_ERROR);
            }

        }

        public static async Task<WindowsAuthData> authenticateUser(string inputText)
        {
            WindowsAuthData result = new WindowsAuthData(false, "Unauthenticated");
            string setting = UserPrefDB.GetPref();
            if (setting == "Custom")
            {
                string pass = PromptAuth(inputText);
                bool check = PasswordService.checkPassword(pass);
                if (check)
                {
                    result.flag = true;
                    result.message = "Authenticated via Custom Password";
                }
                else
                {
                    result.flag = false;
                    result.message = "Authentication via Custom Password Failed";
                }

            }

            else if (setting == "WindowsHello")
            {
                string[] vals = { inputText };
                result = await AppAuthenticationService.authenticate(vals);
            }

            return result;
        }

        public static string PromptAuth(string inputText)
        {
            string pass;
            var creds = CredentialManager.PromptForCredentials(
                    messageText: inputText,
                    saveCredential: Meziantou.Framework.Win32.CredentialSaveOption.Selected,
                    userName: "User");

            pass = creds?.Password;

            return pass;
        }

        public static async Task<bool> isAuthSetup()
        {

            bool keyCredentialAvailable = await KeyCredentialManager.IsSupportedAsync();
            WindowsAuthData winhello = await isWindowsAuthAvailable();
            if (winhello.message == "Windows Hello Authentication is available.")
            {
                return true;
            }

            else if (keyCredentialAvailable)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
