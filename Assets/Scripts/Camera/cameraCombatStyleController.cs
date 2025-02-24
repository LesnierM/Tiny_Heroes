using PGCTools.MethodExtensions;
using System.Linq;
using TinyHero.Player;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

namespace TinyHero.Camera
{
    public class cameraCombatStyleController : MonoBehaviour
    {
        #region Private Fields
        private CinemachineInputAxisController _axiInputController;
        private CinemachineOrbitalFollow _orbitalFollow;
        #endregion

        #region Private Fields
        private Transform _target;
        private Transform _mainCamera;
        #endregion

        #region Mono
        private void Awake()
        {
            getReferences();
            subscribeToEvents();
        }
        private void Update()
        {
            rotate();
        }
        private void OnDestroy()
        {
            unsubscribeToEvents();
        }
        #endregion

        #region Private Methods
        private void rotate()
        {
            if (_target == null)
                return;

            var directionVector = (_target.position - _mainCamera.position).normalized;
            var rotation = Quaternion.LookRotation(directionVector).eulerAngles;

            _orbitalFollow.HorizontalAxis.Value = rotation.y;
        }
        private void getReferences()
        {
            _axiInputController = GetComponent<CinemachineInputAxisController>();
            _orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
            _mainCamera = UnityEngine.Camera.main.transform;
        }
        private void subscribeToEvents()
        {
            playerCombatController.OnCombatStyleStarted += OnCombatStyleStarted;
            playerCombatController.OnCombatStyleEnded += OnCombatStyleEnded;
        }
        private void unsubscribeToEvents()
        {
            playerCombatController.OnCombatStyleStarted -= OnCombatStyleStarted;
            playerCombatController.OnCombatStyleEnded -= OnCombatStyleEnded;
        }
        private InputAxisControllerBase<CinemachineInputAxisController.Reader>.Controller getController()
        {
            return _axiInputController.Controllers.FirstOrDefault(data => data.Name.Equals("Look Orbit X"));
        }
        #endregion

        #region Events
        private void OnCombatStyleEnded()
        {
            getController().Enabled = true;
            _target = default;
        }
        private void OnCombatStyleStarted(Transform selectedTarget)
        {
            getController().Enabled = false;
            _target = selectedTarget;
        }
        #endregion
    }
}
