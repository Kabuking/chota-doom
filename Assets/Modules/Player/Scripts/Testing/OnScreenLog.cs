using System.Collections.Generic;
using UnityEngine;

namespace Modules.Player.Scripts.Testing
{
    public class OnScreenLog: MonoBehaviour
    {
        private static List<string> logMessages = new List<string>();
        private const int maxMessages = 10; // Maximum number of messages to display
        private static readonly object lockObject = new object();

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            lock (lockObject)
            {
                logMessages.Add(logString);
                if (logMessages.Count > maxMessages)
                {
                    logMessages.RemoveAt(0);
                }
            }
        }

        void OnGUI()
        {
            GUILayout.BeginVertical();
            foreach (string log in logMessages)
            {
                GUILayout.Label(log);
            }
            GUILayout.EndVertical();
        }

        public static void Log(string message)
        {
            lock (lockObject)
            {
                logMessages.Add(message);
                if (logMessages.Count > maxMessages)
                {
                    logMessages.RemoveAt(0);
                }
            }
        }
    }
}