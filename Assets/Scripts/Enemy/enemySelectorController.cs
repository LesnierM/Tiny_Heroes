using NUnit.Framework;
using PGCTools.MethodExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using TinyHero.Input;
using TinyHero.Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace TinyHero.Enemy
{
    public class enemySelectorController : MonoBehaviour
    {
        #region Actions
        public static event Action<GameObject> OnEnemySelected;
        public static event Action OnEnemyDeselection;
        #endregion

        #region Serialized Fields
        [SerializeField]
        private float _checkRadius;
        [SerializeField]
        private float _getTargetsInterval;
        [SerializeField]
        private LayerMask _enemyLayerMask;
        #endregion

        #region Private Fields
        private Transform _player;

        private List<Collider> _enemyList;

        private Collider[] _overlappedEnemies = new Collider[20];

        private int _currentTargetIndex;
        #endregion

        #region Mono
        private void Awake()
        {
            getReferences();
            subscribeToEvents();
            initialize();
        }
        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(_player.position, _checkRadius);
        }
        private void OnDestroy()
        {
            unsubscribeFromEvents();
        }
        #endregion

        #region Private Coroutines
        private IEnumerator getTargetsRoutine()
        {
            getTargets();

            if (_enemyList.Count == 0)
                yield break;

            selectTarget();

            while (_enemyList.Count != 0)
            {
                yield return new WaitForSeconds(_getTargetsInterval);

                getTargets();
            }
        }
        #endregion

        #region Private Methods
        private void resetData()
        {
            initialize();
            StopAllCoroutines();

            OnEnemyDeselection?.Invoke();
        }
        private void initialize()
        {
            _currentTargetIndex = -1;
            _enemyList = new List<Collider>();
        }
        private void subscribeToEvents()
        {
            InputManager.OnTargetSelectionButtonPressed += OnTargetSelectionButtonPressed;
            InputManager.OnNext += OnNext;
            InputManager.OnPrevious += OnPrevious;
        }
        private void unsubscribeFromEvents()
        {
            InputManager.OnTargetSelectionButtonPressed -= OnTargetSelectionButtonPressed;
            InputManager.OnNext -= OnNext;
            InputManager.OnPrevious -= OnPrevious;
        }
        private void getReferences()
        {
            _player = FindAnyObjectByType<playerMoveController>().transform;
        }
        private void getTargets()
        {
            var count = Physics.OverlapSphereNonAlloc(_player.position, _checkRadius, _overlappedEnemies, _enemyLayerMask);

            for (int i = 0; i < count; i++)
            {
                var enemy = _overlappedEnemies[i];

                if (!isInList(enemy))
                    _enemyList.Add(enemy);
            }

            if (count == 0)
                resetData();
        }
        private void selectTarget()
        {
            var minDistance = float.MaxValue;

            for (int i = 0; i < _enemyList.Count; i++)
            {
                Collider enemy = _enemyList[i];

                float distance = Vector3.Distance(_player.position, enemy.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;

                    _currentTargetIndex = i;
                }
            }

            invokeEnemySelected();
        }
        private void invokeEnemySelected()
        {
            OnEnemySelected?.Invoke(_enemyList[_currentTargetIndex].gameObject);

            this.DebugMessage($"Enemy selected index={_currentTargetIndex}");
        }
        private void selectNextTarget()
        {
            if (_enemyList.Count < 2)
                return;

            _currentTargetIndex++;

            if (_currentTargetIndex == _enemyList.Count)
                _currentTargetIndex = 0;

            invokeEnemySelected();
        }
        private void selectPreviousTarget()
        {
            if (_enemyList.Count < 2)
                return;

            _currentTargetIndex--;

            if (_currentTargetIndex < 0)
                _currentTargetIndex = _enemyList.Count - 1;

            invokeEnemySelected();
        }
        private bool isInList(Collider collider)
        {
            return _enemyList.Contains(collider);
        }
        #endregion

        #region Events
        private void OnPrevious()
        {
            selectPreviousTarget();
        }
        private void OnNext()
        {
            selectNextTarget();
        }
        private void OnTargetSelectionButtonPressed()
        {
            if (_currentTargetIndex == -1)
                StartCoroutine(getTargetsRoutine());
            else
                resetData();
        }
        #endregion

        #region Properties
        #endregion
    }
}