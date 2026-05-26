using System.Collections;
using System.IO;
using BastionUA.Core;
using UnityEngine;

namespace BastionUA.UI
{
    public static class GameScreenshotUtility
    {
        public static void TryScheduleCapture(MonoBehaviour host)
        {
            var outputPath = TryGetScreenshotPath();
            if (string.IsNullOrEmpty(outputPath))
            {
                return;
            }

            host.StartCoroutine(CaptureAndQuit(outputPath));
        }

        private static string TryGetScreenshotPath()
        {
            try
            {
                var args = System.Environment.GetCommandLineArgs();
                for (var index = 0; index < args.Length - 1; index++)
                {
                    if (args[index] == GameConstants.ScreenshotCommandLineArg)
                    {
                        return args[index + 1].Trim('"');
                    }
                }
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning($"[GameScreenshotUtility] Command-line parse failed: {exception.Message}");
            }

            return null;
        }

        private static IEnumerator CaptureAndQuit(string outputPath)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(GameConstants.ScreenshotCaptureDelaySeconds);
            yield return new WaitForEndOfFrame();

            try
            {
                var directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                ScreenCapture.CaptureScreenshot(outputPath, 1);
                Debug.Log($"[GameScreenshotUtility] Saved screenshot: {outputPath}");
            }
            catch (System.Exception exception)
            {
                Debug.LogError($"[GameScreenshotUtility] Capture failed: {exception}");
            }

            yield return new WaitForSeconds(GameConstants.ScreenshotQuitDelaySeconds);
            Application.Quit(0);
        }
    }
}
