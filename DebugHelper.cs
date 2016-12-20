namespace EngineerLevelFixer {
  class DebugHelper {
    private static string MOD_ID = "[EngineerLevelFixer] ";

    public static void Debug(object message) { if (ConfigLoader.getDebug()) { UnityEngine.Debug.Log(string.Concat(MOD_ID, message.ToString())); } }
    public static void Info(object message) { UnityEngine.Debug.Log(string.Concat(MOD_ID, message.ToString())); }
    public static void Warn(object message) { UnityEngine.Debug.LogWarning(string.Concat(MOD_ID, message.ToString())); }
    public static void Error(object message) { UnityEngine.Debug.LogError(string.Concat(MOD_ID, message.ToString())); }
  }
}
