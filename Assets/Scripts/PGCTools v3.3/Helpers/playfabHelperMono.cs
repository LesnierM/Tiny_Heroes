using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGCTools.MethodExtensions;
#if Playfab
using PlayFab.Json;
using PlayFab.ClientModels;
#endif
namespace PGCTools.Helpers.Playfab
{
    public class playfabHelperMono : MonoBehaviour
    {
#if Playfab
        [SerializeField] string _functionName;
        [SerializeField] CloudScriptRevisionOption _revisionType;
        [SerializeField] int _specificRevision;
        [Space(10)]
        [SerializeField] List<CloudScriptParamtersHolder> _parameters;
        [Space(10)]
        [TextArea(8, 8)]
        [SerializeField] string Result;
        [Header("---------------------------LOGS---------------------------------------")]
        [TextArea(10, 10)]
        [SerializeField] string Info;

        public string FunctionName { get => _functionName; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void executeCloudScript()
        {
            Result = string.Empty;
            Info = string.Empty;
            StartCoroutine(executeCloudScriptCorroutine());
        }
        IEnumerator executeCloudScriptCorroutine()
        {
            List<DataStructs.CloudScriptParameterData[]> _data = new List<DataStructs.CloudScriptParameterData[]>();
            foreach (var item in _parameters)
                _data.Add(item.Data);
            yield return PlayfabHelper.ExecuteCloudScript(_functionName, PlayfabHelper.ConvertParameters(_data.ToArray()), _revisionType, _revisionType == CloudScriptRevisionOption.Specific ? _specificRevision : null);
            if (PlayfabHelper.WasLastCloudScriptReqeustSuccessful)
            {
                if (PlayfabHelper.CloudScriptResult.FunctionResult != null)
                    Result = PlayfabHelper.CloudScriptResult.FunctionResult.ToString();
                foreach (var item in PlayfabHelper.CloudScriptResult.Logs)
                    Info += $"---------------------------------------------------------\r\n" +
                        $"Level={item.Level}\r\nMessage={item.Message}\r\nData={item.Data}\r\n";
            }
            else
                Result = PlayfabHelper.LastCloudRequestError.StackTrace;
        }
#endif
    }
    [System.Serializable]
    struct CloudScriptParamtersHolder
    {
        public DataStructs.CloudScriptParameterData[] Data;
    }
}
