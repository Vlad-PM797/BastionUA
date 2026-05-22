using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace BastionUA.EditorTools
{
    public static class BuildWindowsDev
    {
        private const string BuildFolder = "Builds/Windows";
        private const string BuildFileName = "BastionUA.exe";

        [MenuItem("BastionUA/Build Windows Dev")]
        public static void BuildWindowsDevelopment()
        {
            try
            {
                var scenes = new[] { "Assets/Scenes/Boot.unity" };
                var outputPath = Path.Combine(BuildFolder, BuildFileName);
                Directory.CreateDirectory(BuildFolder);

                var options = new BuildPlayerOptions
                {
                    scenes = scenes,
                    locationPathName = outputPath,
                    target = BuildTarget.StandaloneWindows64,
                    options = BuildOptions.Development | BuildOptions.AllowDebugging
                };

                var report = BuildPipeline.BuildPlayer(options);
                if (report.summary.result != BuildResult.Succeeded)
                {
                    Debug.LogError($"[BuildWindowsDev] Build failed: {report.summary.result}");
                    return;
                }

                Debug.Log($"[BuildWindowsDev] Build succeeded: {outputPath}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[BuildWindowsDev] Build error: {exception}");
            }
        }
    }
}
