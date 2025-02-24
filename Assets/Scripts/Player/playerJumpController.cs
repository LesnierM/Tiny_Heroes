using PGCTools.MethodExtensions;
using System;
using TinyHero.Input;
using UnityEngine;

namespace TinyHero.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(playerCombatController))]
    [RequireComponent(typeof(playerAnimationController))]
    public class playerJumpController : MonoBehaviour
    {
        #region Actions
        public event Action OnJump;
        #endregion

        #region Serialized Fields
        [SerializeField]
        private float _jumpHeight;
        [SerializeField, Header("Gravity")]
        private float _groundedGravity;
        [SerializeField]
        private float _gravityScaler;
        #endregion

        #region Private Fields
        private CharacterController _characterController;
        private playerGroundCheckController _groundCheckController;
        private playerAnimationController _animationController;
        private playerCombatController _combatController;

        private float _currentVerticalSpeed;
        #endregion

        #region Mono
        private void Awake()
        {
            getReferences();
        }
        private void Update()
        {
            jump();
        }
        #endregion

        #region Private Mehtods
        private void getReferences()
        {
            _characterController = GetComponent<CharacterController>();
            _groundCheckController = GetComponentInChildren<playerGroundCheckController>();
            _animationController = GetComponentInChildren<playerAnimationController>();
            _combatController = GetComponentInChildren<playerCombatController>();
        }
        private void jump()
        {
            this.DebugMessage($"Is grounded {_groundCheckController.IsGrounded}");

            if (!_combatController.IsIncombat && (_groundCheckController.IsGrounded || _animationController.IsNormalJump || _animationController.IsDoubleJump) && InputManager.Instance.JumpingPressedInThisFrame)
            {
                OnJump?.Invoke();

                _currentVerticalSpeed = _jumpHeight.GetJumpVerticalVelocity();

                this.DebugMessage($"Jumpt height {_currentVerticalSpeed}");
            }
            else
            {
                if (_groundCheckController.IsGrounded)
                    _currentVerticalSpeed = _groundedGravity;
                else
                    _currentVerticalSpeed += Physics.gravity.y * Time.deltaTime * _gravityScaler;
            }

            _characterController.Move(Vector3.up * _currentVerticalSpeed * Time.deltaTime);
        }
        #endregion
    }
}
