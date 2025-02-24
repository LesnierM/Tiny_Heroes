using PGCTools.MethodExtensions;
using UnityEngine;

namespace TinyHero.Enemy
{
    public class enemyTargetSelectionController : MonoBehaviour
    {
        #region private Fields
        private Outline[] _outlineControllers;
        #endregion

        #region Mono
        private void Awake()
        {
            getReferences();
            subscribeToEvents();
        }
        private void OnDestroy()
        {
            unsubscribeFromEvents();
        }
        #endregion

        #region Private Mehtods
        private void getReferences()
        {
            _outlineControllers = GetComponentsInChildren<Outline>();
        }
        private void subscribeToEvents()
        {
            enemySelectorController.OnEnemySelected += OnEnemySelected;
            enemySelectorController.OnEnemyDeselection += OnEnemyDeselection;
        }
        private void unsubscribeFromEvents()
        {
            enemySelectorController.OnEnemySelected -= OnEnemySelected;
            enemySelectorController.OnEnemyDeselection -= OnEnemyDeselection;
        }
        private void select()
        {
            foreach (var item in _outlineControllers)
                item.Enable();
        }
        private void deselect()
        {
            foreach (var item in _outlineControllers)
                item.Disable();
        }
        #endregion

        #region Events
        private void OnEnemyDeselection()
        {
            deselect();
        }
        private void OnEnemySelected(GameObject obj)
        {
            if (obj == gameObject)
                select();
            else
                deselect();
        }
        #endregion
    }
}
