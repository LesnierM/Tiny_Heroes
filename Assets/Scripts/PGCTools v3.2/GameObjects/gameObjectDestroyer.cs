using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGCTools.GameObjects
{
    public class gameObjectDestroyer : MonoBehaviour
    {
        #region Actions
        public event Action OnDestroyed;
        #endregion

        #region Serialized Fields
        [SerializeField]
        private float _timeToDestroy;
        #endregion

        #region Mono Methods
        private void Awake()
        {
            Invoke("destroy", _timeToDestroy);
        }
        #endregion

        #region Private Methods
        private void destroy()
        {
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
        #endregion
    }
}
