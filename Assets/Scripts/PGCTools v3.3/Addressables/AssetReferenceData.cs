using PGCTools.MethodExtensions;
using System;
using System.Threading.Tasks;
using UnityEngine;
#if Addressables
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif

namespace PGCTools.AddressablesHelpers
{
#if Addressables
    [Serializable]
    public class AssetReferenceData<T> where T : UnityEngine.Object
    {
        #region Actions
        public event Action<T> OnAssetLoaded;
        #endregion

        #region Serialized Fields
        [SerializeField]
        private AssetReferenceT<T> _reference;
        #endregion

        #region Private Fields
        private AsyncOperationHandle<T> _operationHandle;
        #endregion

        #region Private Tasks
        /// <summary>
        /// used to invoke on laoded asset when it is already laoded to make time its user subscribe to the event.
        /// </summary>
        /// <returns></returns>
        private async Task invokeEvent()
        {
            await Task.Delay((Time.deltaTime * 1000).ToInt());

            OnAssetLoaded?.Invoke(Asset);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Loads and asset into memory.
        /// </summary>
        /// <param name="addInstance">If will use an already loaded asset instance or add a new one.</param>
        public void LoadAsset(bool addInstance = false)
        {
            if (_reference.RuntimeKeyIsValid() && (!_operationHandle.IsValid() || (_operationHandle.Result == null || addInstance)))
            {
                _operationHandle = Addressables.LoadAssetAsync<T>(_reference.RuntimeKey);
                _operationHandle.Completed += OnLoadCompleted;
            }
            else if (_operationHandle.IsValid() && _operationHandle.Result != null)
                invokeEvent();
        }
        public void ReleaseAsset()
        {
            if (IsLoaded)
                Addressables.ReleaseInstance(_operationHandle);
        }
        #endregion

        #region Events
        private void OnLoadCompleted(AsyncOperationHandle<T> result)
        {
            _operationHandle.Completed -= OnLoadCompleted;

            if (result.Status == AsyncOperationStatus.Succeeded)
                invokeEvent();//add hre because it was raising an error
        }
        #endregion

        #region Properties
        public bool IsValid => _reference.RuntimeKeyIsValid();
        public T Asset
        {
            get
            {
                if (IsLoaded)
                    return _operationHandle.Result;
                return null;
            }
        }
        public bool IsLoaded => _operationHandle.IsValid() && _operationHandle.Result != null;
        #endregion
    }
#endif
}