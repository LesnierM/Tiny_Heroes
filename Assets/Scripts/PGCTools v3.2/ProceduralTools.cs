using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGCTools
{
    public static class ProceduralTools
    {
        /// <summary>
        /// Returns one of these numebrs randomly -1 y 1.
        /// </summary>
        public static int getPlaneRandomDirection()
        {
            int _result = 0;
            int _result1 = Random.Range(-1, 2);
            int _result2 = Random.Range(-1, 2);
            if (_result1 == 0 || _result2 == 0)
                _result = _result1 + _result2;
            else
                _result = _result1;
            return _result;
        }
    }
}
