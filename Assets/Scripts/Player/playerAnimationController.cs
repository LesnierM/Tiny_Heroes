using PGCTools.MethodExtensions;
using TinyHero.Enums.Player;
using TinyHero.Input;
using UnityEngine;

namespace TinyHero.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(playerJumpController))]
    public class playerAnimationController : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField]
        private float _animationTransitionDuration;
        #endregion

        #region Private Fields
        private Animator _animator;
        private CharacterController _characterControler;
        private playerGroundCheckController _groundCheckController;
        private playerJumpController _jumpController;
        #endregion

        #region Private Fields
        private PlayerAnimationStateTypes _currentState;
        #endregion

        #region Mono
        private void Awake()
        {
            getReferences();
            subscribeToEvents();
        }
        private void LateUpdate()
        {
            checkState();
        }
        private void OnDestroy()
        {
            unsubscribeFromEvents();
        }
        #endregion

        #region Private Mehtods
        private void checkState()
        {
            if (_groundCheckController.IsGrounded)
            {
                if (_characterControler.velocity.ZerOtherAxys(Vector3.forward, Vector3.right).sqrMagnitude != 0)
                {
                    if (InputManager.Instance.IsRunningPressed)
                        changeState(PlayerAnimationStateTypes.Run);
                    else
                        changeState(PlayerAnimationStateTypes.Walk_Normal);
                }
                else
                    changeState(PlayerAnimationStateTypes.Idle_Normal);
            }
        }
        private void changeState(PlayerAnimationStateTypes newState)
        {
            if (_currentState == newState)
                return;

            _currentState = newState;

            _animator.CrossFade(newState.ToString().RemoveCharacters("_"), _animationTransitionDuration);
        }
        private void getReferences()
        {
            _animator = GetComponentInChildren<Animator>();
            _characterControler = GetComponent<CharacterController>();
            _groundCheckController = GetComponentInChildren<playerGroundCheckController>();
            _jumpController = GetComponentInChildren<playerJumpController>();
        }
        private void subscribeToEvents()
        {
            _jumpController.OnJump += OnJump;
        }
        private void unsubscribeFromEvents()
        {
            _jumpController.OnJump -= OnJump;
        }
        #endregion

        #region Events
        private void OnJump()
        {
            switch (_currentState)
            {
                case PlayerAnimationStateTypes.Jump_Normal:
                    changeState(PlayerAnimationStateTypes.Jump_Double);
                    break;
                case PlayerAnimationStateTypes.Jump_Double:
                    changeState(PlayerAnimationStateTypes.Jump_Spin);
                    break;
                default:
                    changeState(PlayerAnimationStateTypes.Jump_Normal);
                    break;
            }
        }
        #endregion

        #region Properties
        public bool IsNormalJump => _currentState == PlayerAnimationStateTypes.Jump_Normal;
        public bool IsDoubleJump => _currentState == PlayerAnimationStateTypes.Jump_Double;
        #endregion
    }
}
