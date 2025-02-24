using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(PGCTools.Debugger.DebugLogger))]
public class debugLoggerEditor :Editor
{
    public override void OnInspectorGUI()
    {
        var _object = target as PGCTools.Debugger.DebugLogger;
        PGCTools.Debugger.DebugLogger.Instance._sessionsToDebug =(PGCTools.Debugger.DebugSession) EditorGUILayout.EnumFlagsField("Sessions", PGCTools.Debugger.DebugLogger.Instance._sessionsToDebug);
    }
}
