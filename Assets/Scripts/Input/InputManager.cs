using UnityEngine;

namespace TinyHero.Input
{
    public class InputManager : PGCTools.SingletonPersitent<InputManager>
    {
        #region Private Fields
        private InputContorller _inputController;
        #endregion

        #region Mono
        private void Awake()
        {
            lockMouse();
            initialize();
        }
        #endregion

        #region Private Methods
        private void initialize()
        {
            _inputController = new InputContorller();

            _inputController.Enable();
        }
        private void lockMouse()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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
