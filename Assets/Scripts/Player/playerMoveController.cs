using PGCTools.MethodExtensions;
using System;
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

        private CharacterController _characterController;

        private Vector3 _input;
        #endregion

        #region Mono
        private void Awake()
        {
            getReferences();
        }
        private void Update()
        {
            readInput();
            move();
            rotate();
        }
        #endregion

        #region Private Methods
        private void getReferences()
        {
            _mainCamera = Camera.main.transform;
            _characterController = GetComponent<CharacterController>();
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

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_characterController.velocity.ZerOtherAxys(Vector3.forward,Vector3.right)), _rotationSpeed * Time.deltaTime);
        }
        private float getSpeed()
        {
            return InputManager.Instance.IsRunningPressed ? _runSpeed : _walkSpeed;
        }
        private Vector3 getForward()
        {
            return (transform.position.ZerOtherAxys(Vector3.forward, Vector3.right) - _mainCamera.position.ZerOtherAxys(Vector3.forward, Vector3.right)).normalized;
        }
        #endregion
    }
}
