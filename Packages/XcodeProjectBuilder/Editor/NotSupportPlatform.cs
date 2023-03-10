using UnityEditor.iOS.Xcode;

namespace XcodeProjectBuilder
{
    public class NotSupportPlatform : IXcodeProject
    {
        public IBuildSettings BuildSettings => new NotSupportBuildSettings();
        public IBuildPhases BuildPhases => new NotSupportBuildPhases();
        public IPlist Info => new NotSupportPlist();

        private class NotSupportBuildSettings : IBuildSettings
        {
            public string OtherLinkerFlags { get; set; }

            public void AddOtherLinkerFlags(string value)
            {
            }

            public bool EnableBitcode { get; set; }
        }

        private class NotSupportBuildPhases : IBuildPhases
        {
            public void AddCopyBundleResources(string path)
            {
            }

            public void AddCopyBundleResources(string fromPath, string toPath)
            {
            }
        }

        private class NotSupportPlist : IPlist, IPlistBooleanElements
        {
            public PlistElementDict Root => null;
            public IPlistBooleanElements Booleans => this;

            bool IPlistBooleanElements.this[string key]
            {
                get => false;
                set { }
            }

            public void Dispose()
            {
            }
        }

        public void Dispose()
        {
        }
    }
}