using System;
using TinyHero.Enemy;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace TinyHero.Player
{
    public class playerCombatController : MonoBehaviour
    {
        #region Actions
        public static event Action<Transform> OnCombatStyleStarted;
        public static event Action OnCombatStyleEnded;
        #endregion

        #region Private Fields
        private bool _isInCombat;
        #endregion

        #region Mono
        private void Awake()
        {
            subscribeToEvents();
        }
        private void OnDestroy()
        {
            unsubscribeFromEvents();
        }
        #endregion

        #region Private Methods
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
        #endregion

        #region Events
        private void OnEnemyDeselection()
        {
            OnCombatStyleEnded?.Invoke();
            _isInCombat = false;
        }
        private void OnEnemySelected(GameObject obj)
        {
            OnCombatStyleStarted?.Invoke(obj.transform);
            _isInCombat = true;
        }
        #endregion

        #region Properties
        public bool IsIncombat { get => _isInCombat; }
        #endregion
    }
}
