using PGCTools.Enums;
using PGCTools.MethodExtensions;
using TinyHero.Enemy;
using TinyHero.Input;
using UnityEngine;

namespace TinyHero.Player
{
    [RequireComponent(typeof(playerCombatController))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(playerMoveController))]
    public class playerRotationController : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField]
        private float _rotationSpeed = 8f;
        #endregion

        #region Private Fields
        private playerCombatController _combatController;
        private playerMoveController _moveController;
        private playerEvadeController _evadeController;

        private Transform _combatTarget;
        #endregion

        #region Mono
        private void Awake()
        {
            getReferences();
            subscribeToEvents();
        }
        private void LateUpdate()
        {
            rotate();
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
        }
        private void unsubscribeFromEvents()
        {
            enemySelectorController.OnEnemySelected -= OnEnemySelected;
        }
        private void getReferences()
        {
            _combatController = GetComponent<playerCombatController>();
            _moveController = GetComponent<playerMoveController>();
            _evadeController = GetComponent<playerEvadeController>();
        }
        private void rotate()
        {
            if (InputManager.Instance.MoveInput.sqrMagnitude == 0 && !_combatController.IsIncombat)
                return;

            if (!_combatController.IsIncombat)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_moveController.MoveDirection.ZerOtherAxys(Vector3.forward, Vector3.right)), _rotationSpeed * Time.deltaTime);
            else
            {
                var directionVector = Vector3.zero;

                if (!_evadeController.IsEvading||(_evadeController.Direction!=Directions.Right&& _evadeController.Direction != Directions.Left))
                    directionVector = (_combatTarget.position - transform.position).normalized;
                else
                    directionVector = _moveController.MoveDirection.ZerOtherAxys(Vector3.forward, Vector3.right);

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionVector), _rotationSpeed * Time.deltaTime);
            }
        }
        #endregion

        #region Events
        private void OnEnemySelected(GameObject obj)
        {
            _combatTarget = obj.transform;
        }
        #endregion

        #region Proeprties
        #endregion
    }
}
