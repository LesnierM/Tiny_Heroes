using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PGCTools.Ui
{
    public class canvasFader : MonoBehaviour
    {
        #region Actions
        public event Action OnFadedIn;
        public event Action OnFadedOut;
        #endregion

        #region Serialized Fields
        [SerializeField] CanvasGroup _canvasGroup;
        [Space(5)]
        [SerializeField] float _duration;
        [Header("Events")]
        [SerializeField] UnityEvent OnFadeIn;
        [SerializeField] UnityEvent OnFadeOut;
        #endregion

        #region Private Methods
        [ContextMenu("Fadein")]
        private void FadeIn()
        {
            FadeIn(true);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fades the selected canvas group in.
        /// </summary>
        public void FadeIn(bool blockRaycast = false)
        {
            _canvasGroup.blocksRaycasts = blockRaycast;
            StopAllCoroutines();
            StartCoroutine(fade(FadeTypes.In));
        }
        /// <summary>
        /// Fades teh selected canvas group out.
        /// </summary>
        [ContextMenu("Fadeout")]
        public void FadeOut()
        {
            _canvasGroup.blocksRaycasts = false;
            StopAllCoroutines();
            StartCoroutine(fade(FadeTypes.Out));
        }
        #endregion

        #region Private Coroutines
        private IEnumerator fade(FadeTypes fadeType)
        {
            if (fadeType.Equals(default))
                yield break;

            float _source = fadeType.Equals(FadeTypes.In) ? 0 : 1;
            float _target = fadeType.Equals(FadeTypes.In) ? 1 : 0;
            float _normalizedTime = 0f;

            while (_normalizedTime < 1f)
            {
                _normalizedTime += Time.deltaTime / _duration;

                _canvasGroup.alpha = Mathf.Lerp(_source, _target, _normalizedTime);

                yield return null;
            }

            //invoke events
            if (fadeType.Equals(FadeTypes.In))
            {
                OnFadedIn?.Invoke();
                OnFadeIn?.Invoke();
            }

            else if (fadeType.Equals(FadeTypes.Out))
            {
                OnFadedOut?.Invoke();
                OnFadeOut?.Invoke();
            }
        }
        #endregion

        #region Properties
        public bool IsShown { get => _canvasGroup.alpha == 1; }
        #endregion

        #region Enums
        protected enum FadeTypes
        {
            None, In, Out
        }
        #endregion
    }
}
