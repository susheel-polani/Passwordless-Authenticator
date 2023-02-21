using Passwordless_Authenticator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials.UI;

namespace Passwordless_Authenticator.Constants.Auth
{
    internal class AppAuthConstants
    {
        public const string APP_AUTH_PROMPT_MSG = "Authentication needed to access the Domain specific Key pairs.";

        public static Dictionary<UserConsentVerifierAvailability, string> windowsAuthStatusMessages = new Dictionary<UserConsentVerifierAvailability, string>()
        {
            [UserConsentVerifierAvailability.Available] = "Windows Hello Authentication is available.",
            [UserConsentVerifierAvailability.DeviceNotPresent] = "Windows Hello Authentication is not available.",
            [UserConsentVerifierAvailability.NotConfiguredForUser] = @"Windows Hello is not configured. Please configure PIN/FingerPrint/Facial Recognition.",
            [UserConsentVerifierAvailability.DisabledByPolicy] = "Windows Hello is disabled by policy.",
            [UserConsentVerifierAvailability.DeviceBusy] = "Device is busy. Please close the Login prompt from other apps and try again",

        };

        public static Dictionary<UserConsentVerificationResult, string> windowsAuthVerificationMessages = new Dictionary<UserConsentVerificationResult, string>()
        {
            [UserConsentVerificationResult.Verified] = "Logged In Successfully",
            [UserConsentVerificationResult.DeviceNotPresent] = "Windows Hello Authentication is not available.",
            [UserConsentVerificationResult.NotConfiguredForUser] = @"Windows Hello is not configured. Please configure PIN/FingerPrint/Facial Recognition.",
            [UserConsentVerificationResult.DisabledByPolicy] = "Windows Hello is disabled by policy.",
            [UserConsentVerificationResult.DeviceBusy] = "Device is busy. Please close the Login prompt from other apps and try again",
            [UserConsentVerificationResult.RetriesExhausted] = "There have been too many failed attempts",
            [UserConsentVerificationResult.Canceled] = "Authentication cancelled by the user",

        };

        public static WindowsAuthData getAvailabilityData(UserConsentVerifierAvailability status)
        {

            WindowsAuthData windowsAuthData = new WindowsAuthData(true, "");

            if (status != UserConsentVerifierAvailability.Available)
            {
                windowsAuthData.flag = false;
            }

            windowsAuthData.message = windowsAuthStatusMessages[status];

            return windowsAuthData;
        }

        public static WindowsAuthData getVerificationData(UserConsentVerificationResult status)
        {

            WindowsAuthData windowsAuthData = new WindowsAuthData(true, "");

            if (status != UserConsentVerificationResult.Verified)
            {
                windowsAuthData.flag = false;
            }

            windowsAuthData.message = windowsAuthVerificationMessages[status];

            return windowsAuthData;
        }
    }
}
