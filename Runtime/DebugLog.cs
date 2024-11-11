namespace OmiLAXR
{
    public interface IDebugSender
    {
        DebugLog DebugLog { get; }
    }
    public delegate string BuildMessageResponse(string prefix = "OmiLAXR");
    public class DebugLog
    {
        private readonly string _prefix;
        public DebugLog(string prefix)
        {
            _prefix = prefix;
        }
        
        public static readonly DebugLog OmiLAXR = new DebugLog("OmiLAXR");
        public static readonly DebugLog xAPI = new DebugLog("OmiLAXR.xAPI");
        
        private static string BuildMessage(string prefix, string message, params object[] ps)
            => $"[{prefix}]: {((ps != null && ps.Length > 0) ? string.Format(message, ps) : message)}";
        public void Print(string message, params object[] ps)
            => UnityEngine.Debug.Log(BuildMessage(_prefix, message, ps));
        public void Error(string message, params object[] ps)
            => UnityEngine.Debug.LogError(BuildMessage(_prefix, message, ps));
        public void Warning(string message, params object[] ps)
            => UnityEngine.Debug.LogWarning(BuildMessage(_prefix, message, ps));
    }
}