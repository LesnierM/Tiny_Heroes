using System;
using System.Collections.Generic;
using UnityEngine;
#if Addressables
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif
using Object = UnityEngine.Object;

namespace PGCTools.AddressablesHelpers
{
#if Addressables
    public class AddressableReferencesLabelLoader<T> where T : Object
    {
        #region Actions
        public event Action<List<T>> OnAssetsLoaded;
        #endregion

        #region Private Fields
        private AsyncOperationHandle<IList<T>> _operation;

        private List<T> _assets = new List<T>();

        private bool _assetsLoaded;
        #endregion

        #region Public Methods
        public void LoadObjects(AssetLabelReference label)
        {
            _operation = Addressables.LoadAssetsAsync<T>(label, OnAssetLaoded);
            _operation.Completed += AssetsLoaded;
        }
        public void Release()
        {
            _operation.Completed -= AssetsLoaded;
            Addressables.Release(_operation);
        }
        #endregion

        #region Events
        private void OnAssetLaoded(T loaddAsset)
        {
            _assets.Add(loaddAsset);
        }
        private void AssetsLoaded(AsyncOperationHandle<IList<T>> obj)
        {
            OnAssetsLoaded?.Invoke(_assets);
            _assetsLoaded = true;
        }
        #endregion

        #region Properties
        public List<T> Assets { get => _assets; }
        public bool AssetsLoadCompleted { get => _assetsLoaded; }
        #endregion
    }
#endif
}
