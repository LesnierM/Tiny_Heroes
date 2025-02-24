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
        }
        private void OnEnemySelected(GameObject obj)
        {
            OnCombatStyleStarted?.Invoke(obj.transform);
        }
        #endregion

    }
}
