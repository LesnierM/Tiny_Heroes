using PGCTools.MethodExtensions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace PGCTools.Debugger
{
    public class MobileDebugConsole : SingletonPersitent<MobileDebugConsole>
    {
        #region Serialized Fields
        [SerializeField]
        private GameObject _mobileDebugConsoleObject;
        [SerializeField]
        private TextMeshProUGUI _textField;
        #endregion

        #region Mono
        protected override void AwakeFromSingleton()
        {
            if (Application.isEditor)
                _mobileDebugConsoleObject.HideObject();
        }
        #endregion

        #region Private Methods
        private string getColorRedTag()
        {
            return "<color=\"red\">";
        }
        private string getColorWhiteTag()
        {
            return "<color=\"white\">";
        }
        private string getCloseColorTag()
        {
            return "</color>";
        }
        #endregion

        #region Public Methods
        public void LogMessage(string message)
        {
            _textField.text += getColorWhiteTag() + message + getCloseColorTag() + "\n";
            if (Application.isEditor)
                Debug.Log(message);
        }
        public void LogError(string message)
        {
            _textField.text += getColorRedTag() + message + getCloseColorTag() + "\n";
            if (Application.isEditor)
                Debug.LogError(message);
        }
        #endregion
    }
}
