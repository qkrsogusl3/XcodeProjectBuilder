using UnityEditor.iOS.Xcode;

namespace XcodeProjectBuilder
{
    public static class InfoPlistExtensions
    {
        public static void DisableAppUsesNonExemptEncryption(this IPlist plist)
        {
            plist.Booleans["ITSAppUsesNonExemptEncryption"] = false;
        }

        /// <summary>
        /// https://developers.google.com/identity/sign-in/ios/start-integrating?hl=ko#add_client_id
        /// </summary>
        /// <param name="plist"></param>
        /// <param name="googleServicePlistPath">GoogleService-Info.plist path (from project root)</param>
        public static void GoogleSignInConfiguration(this IPlist plist, string googleServicePlistPath)
        {
            var googleServicePlist = new PlistDocument();
            googleServicePlist.ReadFromFile(googleServicePlistPath);
            var clientId = googleServicePlist.root["CLIENT_ID"].AsString();
            var reversedId = googleServicePlist.root["REVERSED_CLIENT_ID"].AsString();
            plist.GoogleSignInConfiguration(clientId, reversedId);
        }

        /// <summary>
        /// https://developers.google.com/identity/sign-in/ios/start-integrating?hl=ko#add_client_id
        /// </summary>
        /// <param name="plist"></param>
        /// <param name="clientId"></param>
        /// <param name="reversedClientId"></param>
        public static void GoogleSignInConfiguration(this IPlist plist, string clientId, string reversedClientId)
        {
            plist.Root.SetString("GIDClientID", clientId);

            var urlTypesArray = plist.Root.CreateArray("CFBundleURLTypes");
            var dic = urlTypesArray.AddDict();
            var urlSchemesArray = dic.CreateArray("CFBundleURLSchemes");
            urlSchemesArray.AddString(reversedClientId);
        }
    }
}