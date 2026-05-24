using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace BastionUA.EditorTools
{
    public static class WindowsBuildUtility
    {
        private const string BootScenePath = "Assets/Scenes/Boot.unity";

        public static bool BuildWindows(bool development, string buildFolder, string buildFileName)
        {
            try
            {
                var outputPath = Path.Combine(buildFolder, buildFileName);
                Directory.CreateDirectory(buildFolder);

                var options = new BuildPlayerOptions
                {
                    scenes = new[] { BootScenePath },
                    locationPathName = outputPath,
                    target = BuildTarget.StandaloneWindows64,
                    options = development
                        ? BuildOptions.Development | BuildOptions.AllowDebugging
                        : BuildOptions.None
                };

                var report = BuildPipeline.BuildPlayer(options);
                if (report.summary.result != BuildResult.Succeeded)
                {
                    Debug.LogError($"[WindowsBuildUtility] Build failed: {report.summary.result}");
                    return false;
                }

                Debug.Log($"[WindowsBuildUtility] Build succeeded ({(development ? "dev" : "release")}): {outputPath}");
                return true;
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[WindowsBuildUtility] Build error: {exception}");
                return false;
            }
        }
    }
}
