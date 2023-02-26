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
        public static async Task<WindowsAuthData> authenticate()
        {
            try
            {
                var windowsAuthVerification = await UserConsentVerifier.RequestVerificationAsync(AppAuthConstants.APP_AUTH_PROMPT_MSG);

                return AppAuthConstants.getVerificationData(windowsAuthVerification);
            }
            catch
            {
                return new WindowsAuthData(false, AppConstants.GENERIC_ERROR);
            }

        }

        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
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
