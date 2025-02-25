using PGCTools.Enums;
using PGCTools.MethodExtensions;
using System;
using System.Collections;
using TinyHero.Input;
using UnityEngine;

namespace TinyHero.Player
{
    public class playerEvadeController : MonoBehaviour
    {
        #region Actions
        public static event Action<float> OnNotReady;
        public static event Action<Directions> OnEvadeStarted;
        public static event Action OnEvadeEnded;
        #endregion

        #region Serialized Fields
        [SerializeField]
        private float _evadeInterval;
        [SerializeField]
        private float _evadeDuration;
        [SerializeField]
        private float _evadeWalkedDistance;
        #endregion

        #region Private Fields
        private bool _isEvading;

        private float _nextEvadeTime;

        private Directions _direction;
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

        #region Private Coroutines
        private IEnumerator evadeRoutine()
        {
            var initialPosition = transform.position;
            var targetPosition = getTargetPosition(); ;
            float normalizedTime = 0;

            _isEvading = true;

            while (normalizedTime <= 1)
            {
                normalizedTime += Time.deltaTime / _evadeDuration;

                transform.position = Vector3.Lerp(initialPosition, targetPosition, normalizedTime);

                yield return null;
            }

            _isEvading = false;
            OnEvadeEnded?.Invoke();

            defineNextEvadeTime();
        }
        #endregion

        #region Private Methods
        private Vector3 getTargetPosition()
        {
            var initialPosition = transform.position;
            var forward = transform.forward;
            var right = transform.right;

            switch (_direction)
            {
                case Directions.Right:
                    return initialPosition + right * _evadeWalkedDistance;
                case Directions.Left:
                    return initialPosition - right * _evadeWalkedDistance;
                case Directions.Front:
                    return initialPosition + forward * _evadeWalkedDistance;
                case Directions.Back:
                    return initialPosition - forward * _evadeWalkedDistance;
                default:
                    return default;
            }
        }
        private void defineNextEvadeTime()
        {
            _nextEvadeTime = Time.time + _evadeInterval;
        }
        private void subscribeToEvents()
        {
            playerCombatController.OnCombatStyleStarted += OnCombatStyleStarted;
            playerCombatController.OnCombatStyleEnded += OnCombatStyleEnded;
        }
        private void unsubscribeFromEvents()
        {
            playerCombatController.OnCombatStyleStarted -= OnCombatStyleStarted;
            playerCombatController.OnCombatStyleEnded -= OnCombatStyleEnded;
        }
        private void subscribeToJumpEvents()
        {
            InputManager.OnJump += OnJump;
        }
        private void unsubscribeFromJumpEvents()
        {
            InputManager.OnJump -= OnJump;
        }
        #endregion

        #region Events
        private void OnCombatStyleEnded()
        {
            unsubscribeFromJumpEvents();
        }
        private void OnCombatStyleStarted(Transform obj)
        {
            subscribeToJumpEvents();
        }
        private void OnJump()
        {
            if (Time.time < _nextEvadeTime)
            {
                OnNotReady?.Invoke(_nextEvadeTime);
                this.DebugMessage($"Evade will be ready in {_nextEvadeTime - Time.time}");

                return;
            }


            var input = InputManager.Instance.MoveInput;
            _direction = default;

            if (input.sqrMagnitude == 0 || input.y < 0)
                _direction = Directions.Back;
            else
            {
                if (input.y > 0)
                    _direction = Directions.Front;
                else if (input.x > 0)
                    _direction = Directions.Right;
                else if (input.x < 0)
                    _direction = Directions.Left;
            }

            OnEvadeStarted?.Invoke(_direction);

            this.DebugMessage($"Evade to direction {_direction}");

            StartCoroutine(evadeRoutine());
        }
        #endregion

        #region Prperties
        public bool IsEvading { get => _isEvading; }
        public Directions Direction { get => _direction; }
        #endregion
    }
}
