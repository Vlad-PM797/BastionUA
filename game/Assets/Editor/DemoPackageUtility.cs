using System.IO;
using System.IO.Compression;
using UnityEditor;
using UnityEngine;

namespace BastionUA.EditorTools
{
    public static class DemoPackageUtility
    {
        private const string WindowsReleaseFolder = "Builds/WindowsRelease";
        private const string WindowsExeName = "BastionUA.exe";
        private const string WindowsDataFolder = "BastionUA_Data";
        private const string OutputZipName = "Builds/BastionUA-Windows-demo.zip";

        [MenuItem("BastionUA/Package Windows Release Demo")]
        public static void PackageWindowsReleaseDemo()
        {
            try
            {
                var exePath = Path.Combine(WindowsReleaseFolder, WindowsExeName);
                var dataPath = Path.Combine(WindowsReleaseFolder, WindowsDataFolder);

                if (!File.Exists(exePath) || !Directory.Exists(dataPath))
                {
                    Debug.LogError(
                        "[DemoPackageUtility] Release build missing. Run BastionUA → Build Windows Release first.");
                    return;
                }

                if (File.Exists(OutputZipName))
                {
                    File.Delete(OutputZipName);
                }

                var buildRoot = Path.GetFullPath(WindowsReleaseFolder);
                var outputPath = Path.GetFullPath(OutputZipName);
                ZipFile.CreateFromDirectory(buildRoot, outputPath, CompressionLevel.Optimal, false);

                Debug.Log($"[DemoPackageUtility] Demo package created: {outputPath}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[DemoPackageUtility] Packaging failed: {exception}");
            }
        }
    }
}
