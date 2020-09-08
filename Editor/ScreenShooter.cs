using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;

namespace BoDyTools
{
    public class ScreenShooter : EditorWindow
    {
        private bool isPaused = false;
        
        private string folderPath = Directory.GetCurrentDirectory() + "/Screenshots/"; //Path.Combine(Application.dataPath, "Screenshots");

        [MenuItem("Window/ScreenShooter")]
        public static void ShowWindow()
        {
            GetWindow<ScreenShooter>("ScreenShooter");
        }

        private void OnGUI()
        {
            minSize = new Vector2(100, 100);
            Event m_Event = Event.current;

            if (m_Event.type == EventType.KeyDown)
            {
                if (m_Event.keyCode == KeyCode.P)
                {
                    Pause();
                }
            }

            if (GUILayout.Button(isPaused ? "Play" : "Pause"))
            {
                Pause();
            }
            if (GUILayout.Button("Next Frame"))
            {
                WaitDelayAsync();
            }
            if (GUILayout.Button("Take Screenshot"))
            {
                TakeScreenshot();
            }
            if (GUILayout.Button("Open Folder"))
            {
                OpenFolder();
            }
        }

        private async Task WaitDelayAsync()
        {
            Pause();
            await Task.Delay(1);
            Pause();
        }

        private void TakeScreenshot()
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            Vector2 screenSize = GetMainGameViewSize();

            var screenshotName = string.Format("Screenshot_{0}x{1}_{2}.png", screenSize.x, screenSize.y, System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"));
            ScreenCapture.CaptureScreenshot(Path.Combine(folderPath, screenshotName));
            Debug.Log(folderPath + screenshotName);
        }

        private void Pause()
        {
            isPaused = !isPaused;
            if (isPaused == true)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        private Vector2 GetMainGameViewSize()
        {
            System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
            System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            object Res = GetSizeOfMainGameView.Invoke(null, null);
            return (Vector2)Res;
        }

        private void OpenFolder()
        {
            var file = Directory.EnumerateFiles(folderPath).FirstOrDefault();
            if (!string.IsNullOrEmpty(file))
            {
                EditorUtility.RevealInFinder(Path.Combine(folderPath, file));
            }
            else
            {
                EditorUtility.RevealInFinder(folderPath);
            }
        }
    }
}