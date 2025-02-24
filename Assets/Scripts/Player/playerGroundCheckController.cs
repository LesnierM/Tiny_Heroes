using UnityEngine;

namespace TinyHero.Player
{
    [RequireComponent(typeof(SphereCollider))]
    public class playerGroundCheckController : MonoBehaviour
    {
        #region Serialized Fields       
        [SerializeField,Tooltip("Is is the time that grounded will be false until it can check again.to avoid being in jum and the collider is still colling with ground")]
        private float _jumpingTimeDuration;
        [SerializeField]
        private LayerMask _layer;
        #endregion

        #region Private Fields
        private SphereCollider _collider;
        private playerJumpController _jumpController;

        private bool _isGounded;
        private bool _jumpingTime;
        #endregion

        #region Mono
        private void Awake()
        {
            getReferences();
            subscribeToEvents();
        }
        private void Update()
        {
            SetGrounded();
        }
        private void OnDisable()
        {
            unsubscribeFromEvents();
        }
        #endregion

        #region Private Mehtods
        private void disableJumpingTime()
        {
            _jumpingTime = false;
        }
        private void getReferences()
        {
            _collider = GetComponent<SphereCollider>();
            _jumpController = GetComponentInParent<playerJumpController>();
        }
        private void subscribeToEvents()
        {
            _jumpController.OnJump += OnJump;
        }
        private void unsubscribeFromEvents()
        {
            _jumpController.OnJump -= OnJump;
        }
        public void SetGrounded()
        {
            if (_jumpingTime)
                return;

            _isGounded = Physics.CheckSphere(_collider.transform.position, _collider.radius, _layer);
        }
        #endregion

        #region Events
        private void OnJump()
        {
            _jumpingTime = true;
            _isGounded = false;

            Invoke("disableJumpingTime", _jumpingTimeDuration);
        }
        #endregion

        #region Proeprties
        public bool IsGrounded => _isGounded;
        #endregion
    }
}
