using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGCTools.Debugger
{
    public  class DebugLogger:PGCTools.SingletonPersitent<DebugLogger>
    {
        public DebugSession _sessionsToDebug;
        /// <summary>
        /// Message logger.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="session">The session the message belongs to.</param>
        /// <param name="type">Type of log.</param>
        public static void Log(string message,DebugSession session=DebugSession.None,LogType type= LogType.Log)
        {
            if (session == DebugSession.None || (Instance._sessionsToDebug & session) != 0)
            {
                string _session = session == DebugSession.None ? "General" : session.ToString();
                string _message = $"{_session}:{message}";
                switch (type)
                {
                    case LogType.Error:
                        Debug.LogError(_message);
                        break;
                    case LogType.Warning:
                        Debug.LogWarning(_message);
                        break;
                    case LogType.Log:
                        Debug.Log(_message);
                        break;
                }
            }
        }
    }
    [System.Flags]
    public enum DebugSession:byte
    {
        None,
        Fusion,
        Input,
        Animator=1<<3,
        Playfab=1<<4,
        Player=1<<5
    }
}

