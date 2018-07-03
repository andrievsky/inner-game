using UnityEngine;

namespace Code.Logger
{
    // To avoid interception with UnityEngine.ILogger
    public interface ILog
    {
        void Log(string msg);
    }

    public class Logger : ILog
    {
        public void Log(string msg)
        {
            Debug.Log(msg);
        }
    }
}