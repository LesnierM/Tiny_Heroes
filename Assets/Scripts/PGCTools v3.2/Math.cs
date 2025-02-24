using PGCTools.Interfaces;
using UnityEngine;
using UnityEngine.Rendering;

namespace PGCTools
{
    public static class Math
    {
		/// <summary>
		/// Gets the world position of a given pixel coord.
		/// </summary>
		/// <param name="coord">The x or y coordinate to calculate.</param>
		/// <param name="boundsSize">The width or height of sprite container object.</param>
		/// <param name="position">The x or y world position of the sprite container object.</param>
		/// <param name="scale">The x or y scale of the sprite container obejct.</param>
		/// <param name="ppu">The sprite pixel per unit value.</param>
		/// <returns></returns>
		public static float CalculateWorldPosOfPixelCoordinate(int coord, float boundsSize, float position, float scale, float ppu)
		{
			float PixelInWorldSpace = 1.0f / ppu;
			float startPos = position - (boundsSize * 0.5f * scale);

			return startPos + (PixelInWorldSpace * coord) * scale;
		}
		/// <summary>
		/// Gets the angle in a plane of 2 vectors.
		/// </summary>
		/// <returns>The grades in degrees.</returns>
		[System.Obsolete("Use vector2.angle instead.")]
        public static float getAngleOfTwoVectorsOnPlane(Vector3 vector1, Vector3 vector2)
        {
            float _angleResult = 0;
            Vector2 _vector1 = new Vector2(vector1.x, vector1.z).normalized;
            Vector2 _vector2 = new Vector2(vector2.x, vector2.z).normalized;

            float _dotProduct = Vector2.Dot(_vector1, _vector2);

            float _magnitudesResult = _vector1.magnitude * _vector2.magnitude;
            float _dotMagnitudeResult = _dotProduct / _magnitudesResult;
            _angleResult = Mathf.Acos(_dotMagnitudeResult) * Mathf.Rad2Deg;

            return _angleResult.ToString().Equals("NaN") ? 0 : _angleResult;
        }
        /// <summary>
        /// Get the position of target relative to object.
        /// </summary>
        /// <param name="objectTransform">The object.</param>
        /// <param name="directionToTarget">The target obejct to find its relative position.</param>
        /// <param name="axy">The axy.Ex(Vector3.Up returns if target is up or down,Vector.Right if it is right or left and Vector3.forward if it is infront or back)</param>
        /// <param name="deadZone">Is the min value over under 0 to trigger the diraction otherwise is none</param>
        /// <param name="showLog">To show value of the selected axy or not.</param>
        /// <returns>The diraction the target object is relative to this obejct.</returns>
        public static Enums.Directions getObjectRelativePosition(Transform objectTransform, Vector3 directionToTarget, Vector3 axy, float deadZone = 0, bool showLog = false)
        {
            //old algorithm
            //vector1.y = vector2.y = 0;
            //Vector3 _perp = Vector3.Cross(vector1.normalized, vector2.normalized);
            //float _result1 = Vector3.Dot(_perp, Vector3.up);
            //float _result = Vector3.Dot(_perp, Vector3.right);
            //return (_result < 0 ?Enums.Directions.Left :Enums.Directions.Right);
            //new algorithm
            var _result = objectTransform.InverseTransformDirection(directionToTarget.normalized);
            Enums.Directions _directionResult = Enums.Directions.None;
            if (axy == Vector3.up)
            {
                if (showLog)
                    Debug.Log("PGCTools.Math.getObjectRelativePosition " + _result.y);
                if (_result.y > deadZone)
                    _directionResult = Enums.Directions.Up;
                else if (_result.y < -deadZone)
                    _directionResult = Enums.Directions.Down;
            }
            else if (axy == Vector3.right)
            {
                if (showLog)
                    Debug.Log("PGCTools.Math.getObjectRelativePosition " + _result.x);
                if (_result.x > deadZone)
                    _directionResult = Enums.Directions.Right;
                else if (_result.x < -deadZone)
                    _directionResult = Enums.Directions.Left;
            }
            else if (axy == Vector3.forward)
            {
                if (showLog)
                    Debug.Log("PGCTools.Math.getObjectRelativePosition " + _result.z);
                if (_result.z > deadZone)
                    _directionResult = Enums.Directions.Front;
                else if (_result.z < -deadZone)
                    _directionResult = Enums.Directions.Back;
            }else
                Debug.LogError("Bad axy parameter.");
            return _directionResult;
        }
        /// <summary>
        /// Checks if probability is met or not.
        /// </summary>
        /// <param name="probability">The probability in percent</param>
        public static bool CheckProbability(int probability)
        {
            return Random.Range(0f, 1f) < probability / 100f;
        }
        /// <summary>
        /// Gets the value representing the provided percent of the provided value.
        /// </summary>
        /// <param name="percent">The percent you need to get.</param>
        /// <param name="value">The value from where to get the percent from.</param>
        public static float GetPercentOf(int percent,float value)
        {
            return value * percent / 100f;
        }
        /// <summary>
        /// Gets the percent that represents value from especified max.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="max">The max.</param>
        public static float GetPercent(int value, int max)
        {
            if (value == 0 && max == 0)
                return 100;
            return value * 100 / max;
        }
        /// <summary>
        /// Generates a random number and return the item taht met this number.
        /// </summary>
        /// <param name="probabilities">A list of probabilities.</param>
        /// <returns></returns>
        public static T GetIndexByProbabilityFromArray<T>(IProbable[] probabilities)where T:IProbable
        {
            IProbable result = default;

            var value = Random.Range(0f, 1f);

            for (int i = 0; i < probabilities.Length; i++)
            {
                if (value <= probabilities[i].Probability/100f)
                    result= probabilities[i];
            }

            if (result == null)
                Debug.LogWarning("No probability met");

            return (T) result;
        }
    }
}