using Passwordless_Authenticator.Constants.Auth;
using Passwordless_Authenticator.Constants;
using Passwordless_Authenticator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials.UI;

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
    }
}
