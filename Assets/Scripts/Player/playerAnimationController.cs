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
        private playerGroundCheckController _groundCheckController;
        private playerJumpController _jumpController;
        private playerCombatController _combatController;
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
                if (!_combatController.IsIncombat)
                    nonCombatLogic();
                else
                    combatLogic();
            }
        }
        private void combatLogic()
        {
            Vector2 input = InputManager.Instance.MoveInput;

            if (input.sqrMagnitude != 0)
            {
                if (input.y > 0)
                    changeState(PlayerAnimationStateTypes.Walk_Forward_Battle);
                else if (input.y < 0)
                    changeState(PlayerAnimationStateTypes.Walk_Back_Battle);
                else if (input.y == 0 && input.x > 0)
                    changeState(PlayerAnimationStateTypes.Walk_Right_Side_Battle);
                else if (input.y == 0 && input.x < 0)
                    changeState(PlayerAnimationStateTypes.Walk_Left_Side_Battle);
            }
            else
                changeState(PlayerAnimationStateTypes.Idle_Battle);
        }
        private void nonCombatLogic()
        {
            if (InputManager.Instance.MoveInput.sqrMagnitude != 0)
            {
                if (InputManager.Instance.IsRunningPressed)
                    changeState(PlayerAnimationStateTypes.Run);
                else
                    changeState(PlayerAnimationStateTypes.Walk_Normal);
            }
            else
                changeState(PlayerAnimationStateTypes.Idle_Normal);
        }

        private void changeState(PlayerAnimationStateTypes newState)
        {
            if (_currentState == newState)
                return;

            _currentState = newState;

            _animator.CrossFade(newState.ToString().RemoveCharacters("_"), _animationTransitionDuration);

            this.DebugMessage($"State changed to {_currentState}");
        }
        private void getReferences()
        {
            _animator = GetComponentInChildren<Animator>();
            _combatController = GetComponent<playerCombatController>();
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
