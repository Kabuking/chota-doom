using UnityEngine;

namespace Characters.Player.Global
{
    public static class DebugX
    {
        public enum LogLevel { Debug, Info, Warning, Error }

        public static void Log(string message)
        {
            Debug.Log($"{message}");
        }

        public static void LogInfo(string message)
        {
            Debug.Log($"ℹ️ [INFO] {message}");
        }

        public static void LogWarning(string message)
        {
            Debug.LogWarning($"⚠️ [WARNING] :{message}");
        }

        public static void LogError(string message)
        {
            Debug.LogError($"❌ [ERROR] {message}");
        }

        private static string Timestamp()
        {
            return System.DateTime.Now.ToString("HH:mm:ss");
        }
        
        public static void LogWithColor(string message, Color color)
        {
            string colorHex = ColorUtility.ToHtmlStringRGB(color);
            Debug.Log($"<color=#{colorHex}>{message}</color>");
        }
        
        public static void LogWithColorYellow(string message)
        {
            string colorHex = ColorUtility.ToHtmlStringRGB(Color.yellow);
            Debug.Log($"<color=#{colorHex}>{message}</color>");
        }
        
        public static void LogWithColorCyan(string message)
        {
            string colorHex = ColorUtility.ToHtmlStringRGB(Color.cyan);
            Debug.Log($"<color=#{colorHex}>{message}</color>");
        }
        
        public static void LogWithColorRed(string message)
        {
            string colorHex = ColorUtility.ToHtmlStringRGB(Color.red);
            Debug.Log($"<color=#{colorHex}>{message}</color>");
        }
        
        public static void LogWithColorGreen(string message)
        {
            string colorHex = ColorUtility.ToHtmlStringRGB(Color.green);
            Debug.Log($"<color=#{colorHex}>{message}</color>");
        }
        
        public static void LogWithColorWhite(string message)
        {
            string colorHex = ColorUtility.ToHtmlStringRGB(Color.white);
            Debug.Log($"<color=#{colorHex}>{message}</color>");
        }
    }
}
