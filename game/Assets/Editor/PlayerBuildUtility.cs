using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace BastionUA.EditorTools
{
    public static class PlayerBuildUtility
    {
        private const string BootScenePath = "Assets/Scenes/Boot.unity";

        public static bool BuildWindows(bool development, string buildFolder, string buildFileName)
        {
            return BuildPlayer(
                BuildTarget.StandaloneWindows64,
                development,
                buildFolder,
                buildFileName);
        }

        public static bool BuildAndroid(bool development, string buildFolder, string buildFileName)
        {
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Android, BuildTarget.Android))
            {
                Debug.LogError("[PlayerBuildUtility] Android build target is not supported. Install Android Build Support in Unity Hub.");
                return false;
            }

            EditorUserBuildSettings.buildAppBundle = false;
            return BuildPlayer(
                BuildTarget.Android,
                development,
                buildFolder,
                buildFileName);
        }

        private static bool BuildPlayer(
            BuildTarget buildTarget,
            bool development,
            string buildFolder,
            string buildFileName)
        {
            try
            {
                var outputPath = Path.Combine(buildFolder, buildFileName);
                Directory.CreateDirectory(buildFolder);

                var options = new BuildPlayerOptions
                {
                    scenes = new[] { BootScenePath },
                    locationPathName = outputPath,
                    target = buildTarget,
                    options = development
                        ? BuildOptions.Development | BuildOptions.AllowDebugging
                        : BuildOptions.None
                };

                var report = BuildPipeline.BuildPlayer(options);
                if (report.summary.result != BuildResult.Succeeded)
                {
                    Debug.LogError($"[PlayerBuildUtility] Build failed: {report.summary.result}");
                    return false;
                }

                Debug.Log(
                    $"[PlayerBuildUtility] Build succeeded ({buildTarget}, {(development ? "dev" : "release")}): {outputPath}");
                return true;
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[PlayerBuildUtility] Build error: {exception}");
                return false;
            }
        }
    }
}
