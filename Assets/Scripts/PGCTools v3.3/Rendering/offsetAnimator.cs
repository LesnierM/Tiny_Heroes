using PGCTools.Enums;
using PGCTools.Rendering.Enums;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace PGCTools.Rendering
{
    public class offsetAnimator : MonoBehaviour
    {
        const string MAIN_TEXTURE_NAME = "_MainTex";

        #region Serialized Fields
        [SerializeField, Range(0f, 5f)]
        private float _baseSpeed;
        [SerializeField]
        private bool _invertDirection;
        [SerializeField, Space(5)]
        private Axis _axys;
        [SerializeField, Space(5)]
        private bool _animate;
        #endregion

        #region Private Fields
        private Image _imageRenderer;

        private SpriteRenderer _spriteRenderer;

        private Material _material;
        #endregion

        #region Mono
        private void Awake()
        {
            initialize();
        }
        private void Update()
        {
            animate();
        }
        #endregion

        #region Private Methods
        private void initialize()
        {
            if (TryGetComponent(out _imageRenderer))
                _imageRenderer.material = _material = Instantiate(_imageRenderer.material);
            else if (TryGetComponent(out _spriteRenderer))
                _spriteRenderer.material = _material = Instantiate(_spriteRenderer.material);
        }
        private void animate()
        {
            if (!_animate)
                return;

            var currentOffsetValue = _material.GetTextureOffset(MAIN_TEXTURE_NAME);

            var deltaPosition = currentOffsetValue + (getDeltaSpeed() * getAxyVector()*getDirection());

            _material.SetTextureOffset(MAIN_TEXTURE_NAME, deltaPosition);
        }
        private int getDirection()
        {
            return _invertDirection ? -1 : 1;
        }
        private float getDeltaSpeed()
        {
            return (_baseSpeed) * Time.deltaTime;
        }
        private Vector2 getAxyVector()
        {
            switch (_axys)
            {
                case Axis.X:
                    return Vector3.right;
                case Axis.Y:
                    return Vector3.up;
                case Axis.XY:
                    return Vector3.right + Vector3.up;
                case Axis.YZ:
                    return Vector3.up + Vector3.right;
                case Axis.All:
                    return Vector3.one;
                default:
                    return Vector3.zero;
            }
        }
        #endregion

        #region Properties
        public bool Aniamte { get => _animate; set => _animate = value; }
        #endregion
    }
}