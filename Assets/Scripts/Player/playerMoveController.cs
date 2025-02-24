using PGCTools.MethodExtensions;
using TinyHero.Enemy;
using TinyHero.Input;
using UnityEngine;

namespace TinyHero.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class playerMoveController : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField]
        private float _walkSpeed;
        [SerializeField]
        private float _runSpeed;
        [SerializeField]
        private float _rotationSpeed;
        #endregion

        #region Private Fields
        private Transform _mainCamera;
        private Transform _combatTarget;

        private CharacterController _characterController;
        private playerCombatController _combatController;

        private Vector3 _input;
        #endregion

        #region Mono
        private void Awake()
        {
            getReferences();
            subscribeToEvents();
        }
        private void Update()
        {
            readInput();
            move();
            rotate();
        }
        private void OnDestroy()
        {
            unsubscribeFromEvents();
        }
        #endregion

        #region Private Methods
        private void getReferences()
        {
            _combatController = GetComponent<playerCombatController>();
            _characterController = GetComponent<CharacterController>();
            _mainCamera = UnityEngine.Camera.main.transform;
        }
        private void subscribeToEvents()
        {
            enemySelectorController.OnEnemySelected += OnEnemySelected;
        }
        private void unsubscribeFromEvents()
        {
            enemySelectorController.OnEnemySelected -= OnEnemySelected;
        }
        private void move()
        {
            var speed = getSpeed();
            var cameraForward = getForward();
            var cameraRight = cameraForward.GetRightVectorFromWorldUp(); ;

            this.DebugMessage($"Camera forward {cameraForward}");
            this.DebugMessage($"Camera right {cameraRight}");

            var deltaMove = (cameraForward * _input.z + cameraRight * _input.x) * speed * Time.deltaTime;

            _characterController.Move(deltaMove);
        }
        private void readInput()
        {
            _input = InputManager.Instance.MoveInput.ChangeYToZ();
        }
        private void rotate()
        {
            if (_input.sqrMagnitude == 0)
                return;

            if (!_combatController.IsIncombat)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_characterController.velocity.ZerOtherAxys(Vector3.forward, Vector3.right)), _rotationSpeed * Time.deltaTime);
            else
            {
                var directionVector = (_combatTarget.position - transform.position).normalized;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionVector), _rotationSpeed * Time.deltaTime);
            }
        }
        private float getSpeed()
        {
            return !InputManager.Instance.IsRunningPressed || _combatController.IsIncombat ? _walkSpeed : _runSpeed;
        }
        private Vector3 getForward()
        {
            return (transform.position.ZerOtherAxys(Vector3.forward, Vector3.right) - _mainCamera.position.ZerOtherAxys(Vector3.forward, Vector3.right)).normalized;
        }
        #endregion

        #region Events
        private void OnEnemySelected(GameObject obj)
        {
            _combatTarget = obj.transform;
        }
        #endregion
    }
}
