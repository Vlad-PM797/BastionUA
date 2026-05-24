using UnityEditor;
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
            WindowsBuildUtility.BuildWindows(true, BuildFolder, BuildFileName);
        }
    }
}
