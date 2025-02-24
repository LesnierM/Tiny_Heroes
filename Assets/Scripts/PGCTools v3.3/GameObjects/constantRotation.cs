using PGCTools.Rendering.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGCTools.GameObjects
{
    public class constantRotation : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField]
        private float _rotationSpeed;
        [SerializeField]
        private Rendering.Enums.Axis _axy;
        #endregion

        #region Mono Methods
        private void Update()
        {
            rotate();
        }
        #endregion

        #region Private Methods
        private void rotate()
        {
            transform.Rotate(getVectorFromAxy(), _rotationSpeed * Time.deltaTime);
        }
        private Vector3 getVectorFromAxy()
        {
            switch (_axy)
            {
                case Axis.Y:
                    return Vector3.up;
                case Axis.X:
                    return Vector3.right;
                case Axis.Z:
                    return Vector3.forward;
                case Axis.XY:
                    return Vector3.right + Vector3.up;
                case Axis.XZ:
                    return Vector3.right + Vector3.forward;
                case Axis.YZ:
                    return Vector3.up + Vector3.forward;
                case Axis.All:
                    return Vector3.up + Vector3.forward + Vector3.right;
                default:
                    return Vector3.zero;
            }

        }
        #endregion
    }
}
