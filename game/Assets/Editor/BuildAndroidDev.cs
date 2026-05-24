using UnityEditor;
using UnityEngine;

namespace BastionUA.EditorTools
{
    public static class BuildAndroidDev
    {
        private const string BuildFolder = "Builds/Android";
        private const string BuildFileName = "BastionUA-dev.apk";

        [MenuItem("BastionUA/Build Android Dev")]
        public static void BuildAndroidDevelopment()
        {
            PlayerBuildUtility.BuildAndroid(true, BuildFolder, BuildFileName);
        }
    }
}
