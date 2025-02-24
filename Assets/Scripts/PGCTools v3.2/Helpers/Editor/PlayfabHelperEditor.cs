using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PGCTools.Helpers.Playfab;
using PGCTools.MethodExtensions;
#if Playfab
using PlayFab.Json;

[CustomEditor(typeof(playfabHelperMono))]
public class gameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var _object = target as playfabHelperMono;
        if (!PlayfabHelper.IsBusy&& PlayfabHelper.LogInResult!=null&&!_object.FunctionName.IsNullOrEmptyOrWhiteSpaced()&&GUILayout.Button("Execute"))
            _object.executeCloudScript();
    }
}
   
#endif