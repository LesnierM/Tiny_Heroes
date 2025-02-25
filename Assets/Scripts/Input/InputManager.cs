using System;
using UnityEngine;

namespace TinyHero.Input
{
    public class InputManager : PGCTools.SingletonPersitent<InputManager>
    {
        #region Actions
        public static event Action OnTargetSelectionButtonPressed;
        public static event Action OnNext;
        public static event Action OnPrevious;
        public static event Action OnJump;
        #endregion

        #region Private Fields
        private InputActions _inputController;
        #endregion

        #region Mono
        private void Awake()
        {
            lockMouse();
            initialize();
            subscribeToEvents();
        }
        private void OnDestroy()
        {
            unsubscribeFromEents();
        }
        #endregion

        #region Private Methods
        private void subscribeToEvents()
        {
            _inputController.Player.CombatTrigger.performed += OnCombatTriggered;
            _inputController.Player.TargetSelection.performed += OnTargetSelection;
            _inputController.Player.Jump.performed += OnJumpEvent;
        }      
        private void unsubscribeFromEents()
        {
            _inputController.Player.CombatTrigger.performed -= OnCombatTriggered;
            _inputController.Player.TargetSelection.performed -= OnTargetSelection;
            _inputController.Player.Jump.performed -= OnJumpEvent;
        }
        private void initialize()
        {
            _inputController = new InputActions();

            _inputController.Enable();
        }
        private void lockMouse()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        #endregion

        #region Events
        private void OnTargetSelection(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            var value = obj.ReadValue<Vector2>();

            if (value.y > 0)
                OnNext?.Invoke();
            else
                OnPrevious?.Invoke();
        }
        private void OnCombatTriggered(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnTargetSelectionButtonPressed?.Invoke();
        }
        private void OnJumpEvent(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnJump?.Invoke();
        }
        #endregion

        #region Properties
        public bool JumpingPressedInThisFrame => _inputController.Player.Jump.WasPressedThisFrame();
        public bool IsJumpingPressed => _inputController.Player.Jump.IsPressed();
        public bool IsRunningPressed => _inputController.Player.Sprint.IsPressed();
        public Vector2 MoveInput => _inputController.Player.Move.ReadValue<Vector2>();
        #endregion
    }
}
