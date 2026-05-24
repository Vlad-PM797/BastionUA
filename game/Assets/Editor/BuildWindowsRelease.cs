using UnityEditor;
using UnityEngine;

namespace BastionUA.EditorTools
{
    public static class BuildWindowsRelease
    {
        private const string BuildFolder = "Builds/WindowsRelease";
        private const string BuildFileName = "BastionUA.exe";

        [MenuItem("BastionUA/Build Windows Release")]
        public static void BuildWindowsReleasePlayer()
        {
            PlayerBuildUtility.BuildWindows(false, BuildFolder, BuildFileName);
        }
    }
}
