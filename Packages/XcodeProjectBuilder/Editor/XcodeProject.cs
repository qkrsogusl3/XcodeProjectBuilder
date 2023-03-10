using System;
using System.IO;
using UnityEditor;

namespace XcodeProjectBuilder
{
    public interface IXcodeProject : System.IDisposable
    {
        IBuildSettings BuildSettings { get; }
        IBuildPhases BuildPhases { get; }
        IPlist Info { get; }
    }

    public static class XcodeProject
    {
        public static IXcodeProject FromPostProcess(BuildTarget buildTarget, string buildPath)
        {
            if (buildTarget != BuildTarget.iOS)
            {
                return new NotSupportPlatform();
            }

            return new PBXProjectWrapper(buildPath);
        }

        /// <summary>
        /// require PostProcessBuild(45)
        /// - Xcode14 version issue
        /// - https://github.com/google/GoogleSignIn-iOS/issues/105
        /// - https://stackoverflow.com/questions/72561696/xcode-14-needs-selected-development-team-for-pod-bundles
        /// - [jar resolver - append podfile](https://github.com/googlesamples/unity-jar-resolver#integration-strategies)
        /// </summary>
        /// <param name="buildTarget"></param>
        /// <param name="buildPath"></param>
        public static void SkipPodBundleSign(BuildTarget buildTarget, string buildPath)
        {
            if (buildTarget != BuildTarget.iOS) return;
            using var writer = File.AppendText($"{buildPath}/Podfile");
            var postInstall = @"
post_install do |installer|
    installer.pods_project.targets.each do |target|
        if target.respond_to?(:product_type) and target.product_type == ""com.apple.product-type.bundle""
            target.build_configurations.each do |config|
                config.build_settings['CODE_SIGNING_ALLOWED'] = 'NO'
            end
        end
    end
end
";
            writer.WriteLine(postInstall);
            writer.Close();
        }
    }
}