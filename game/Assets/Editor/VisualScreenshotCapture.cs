using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace BastionUA.EditorTools
{
    [InitializeOnLoad]
    public static class VisualScreenshotCapture
    {
        private const string BootScenePath = "Assets/Scenes/Boot.unity";
        private const string DefaultOutputFolder = "docs/visual";
        private const string PendingKey = "BastionUA_VisualScreenshotPending";
        private const string OutputPathKey = "BastionUA_VisualScreenshotPath";
        private const string FramesRemainingKey = "BastionUA_VisualScreenshotFrames";

        static VisualScreenshotCapture()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            if (SessionState.GetInt(PendingKey, 0) == 1 && EditorApplication.isPlaying)
            {
                ScheduleCaptureAfterFrames(90);
            }
        }

        [MenuItem("BastionUA/Visual/Capture HUD Screenshot (Play Mode)")]
        public static void CaptureHudFromPlayModeMenu()
        {
            if (!EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog(
                    "BastionUA",
                    "Enter Play mode first, then run this menu again.",
                    "OK");
                return;
            }

            var outputPath = GetOutputPath("after_hud_playmode.png");
            CaptureScreen(outputPath);
            Debug.Log($"[VisualScreenshotCapture] Saved {outputPath}");
        }

        public static void CaptureAfterHudBatch()
        {
            BeginBatchCapture("after_v1_hud.png");
        }

        public static void CaptureBeforeHudBatch()
        {
            BeginBatchCapture("before_v1_hud.png");
        }

        private static void BeginBatchCapture(string fileName)
        {
            try
            {
                if (!File.Exists(BootScenePath))
                {
                    Debug.LogError($"[VisualScreenshotCapture] Boot scene missing at {BootScenePath}");
                    EditorApplication.Exit(1);
                    return;
                }

                SessionState.SetString(OutputPathKey, GetOutputPath(fileName));
                SessionState.SetInt(PendingKey, 1);
                SessionState.SetInt(FramesRemainingKey, 0);

                EditorSceneManager.OpenScene(BootScenePath, OpenSceneMode.Single);
                EditorApplication.EnterPlaymode();
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[VisualScreenshotCapture] Batch capture failed: {exception}");
                ClearSession();
                EditorApplication.Exit(1);
            }
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (SessionState.GetInt(PendingKey, 0) != 1)
            {
                return;
            }

            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                ScheduleCaptureAfterFrames(90);
            }
        }

        private static void ScheduleCaptureAfterFrames(int frameCount)
        {
            SessionState.SetInt(FramesRemainingKey, frameCount);
            EditorApplication.update -= WaitForFramesAndCapture;
            EditorApplication.update += WaitForFramesAndCapture;
        }

        private static void WaitForFramesAndCapture()
        {
            if (SessionState.GetInt(PendingKey, 0) != 1)
            {
                EditorApplication.update -= WaitForFramesAndCapture;
                return;
            }

            if (!EditorApplication.isPlaying)
            {
                return;
            }

            var framesRemaining = SessionState.GetInt(FramesRemainingKey, 0);
            if (framesRemaining > 0)
            {
                SessionState.SetInt(FramesRemainingKey, framesRemaining - 1);
                return;
            }

            EditorApplication.update -= WaitForFramesAndCapture;

            try
            {
                var outputPath = SessionState.GetString(OutputPathKey, string.Empty);
                if (string.IsNullOrEmpty(outputPath))
                {
                    Debug.LogError("[VisualScreenshotCapture] Output path missing.");
                    EditorApplication.Exit(1);
                    return;
                }

                CaptureScreen(outputPath);
                Debug.Log($"[VisualScreenshotCapture] Saved {outputPath}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[VisualScreenshotCapture] Capture failed: {exception}");
                ClearSession();
                EditorApplication.Exit(1);
                return;
            }

            ClearSession();
            EditorApplication.ExitPlaymode();
            EditorApplication.delayCall += () => EditorApplication.Exit(0);
        }

        private static void CaptureScreen(string outputPath)
        {
            var directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            ScreenCapture.CaptureScreenshot(outputPath, 1);
        }

        private static void ClearSession()
        {
            SessionState.EraseInt(PendingKey);
            SessionState.EraseString(OutputPathKey);
            SessionState.EraseInt(FramesRemainingKey);
        }

        private static string GetOutputPath(string fileName)
        {
            var projectRoot = Directory.GetParent(Application.dataPath)?.FullName;
            var repoRoot = Directory.GetParent(projectRoot ?? string.Empty)?.FullName;
            var outputFolder = string.IsNullOrEmpty(repoRoot)
                ? Path.Combine(projectRoot ?? ".", DefaultOutputFolder)
                : Path.Combine(repoRoot, DefaultOutputFolder);
            return Path.GetFullPath(Path.Combine(outputFolder, fileName));
        }
    }
}
