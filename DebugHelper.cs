using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineerLevelFixer
{
    class DebugHelper
    {
        public static void Debug(object message) { if (ConfigLoader.getDebug()) { UnityEngine.Debug.Log(string.Concat("[EngineerLevelFixer] ", message.ToString())); } }
        public static void Info(object message) { UnityEngine.Debug.Log(string.Concat("[EngineerLevelFixer] ", message.ToString())); }
        public static void Warn(object message) { UnityEngine.Debug.LogWarning(string.Concat("[EngineerLevelFixer] ", message.ToString())); }
        public static void Error(object message) { UnityEngine.Debug.LogError(string.Concat("[EngineerLevelFixer] ", message.ToString())); }
    }
}
