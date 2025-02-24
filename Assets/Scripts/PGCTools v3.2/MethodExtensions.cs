using System;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using PGCTools.Enums;
using System.Text;
using UnityEngine.UI;

#if TMPro
using TMPro;
#endif
#if Playfab
using PlayFab.Json;
using PlayFab;
#endif
#if UseAddressables
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using JetBrains.Annotations;
#endif
#if FUSION_WEAVER
using Fusion;
#endif
#if Localization
using UnityEngine.Localization;
#endif

namespace PGCTools.MethodExtensions
{
    public static class BoundsExtension
    {
        /// <summary>
        /// Gets a random position inside the bounds.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 GetRandomPosition(this Bounds value)
        {
            Vector3 result = default;

            UnityEngine.Random.InitState(DateTime.Now.Millisecond);

            result.x = UnityEngine.Random.Range(value.min.x, value.max.x);
            result.y = UnityEngine.Random.Range(value.min.y, value.max.y);
            result.z = UnityEngine.Random.Range(value.min.z, value.max.z);

            return result;
        }
    }
    public static class ByteMethodExtension
    {
        /// <summary>
        /// Converts to bool
        /// </summary>
        /// <param name="value"></param>
        /// <returns>TRUE if it is equl 1 FALSE otherwise.</returns>
        public static bool ToBool(this byte value)
        {
            return value == 1;
        }
    }
    public static class Vector3Extension
    {
        /// <summary>
        /// Checks if the data is equal to default or no.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDefault(this Vector3 value)
        {
            return value == default;
        }
        /// <summary>
        /// Converts a direction vector into direction enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Directions ToDirectionEnum(this Vector3 value)
        {
            if (value == Vector3.down) return Directions.Down;
            if (value == Vector3.up) return Directions.Up;
            if (value == Vector3.forward) return Directions.Front;
            if (value == Vector3.back) return Directions.Back;
            if (value == Vector3.right) return Directions.Right;
            if (value == Vector3.left) return Directions.Left;
            return default;
        }
        /// <summary>
        /// Converts vector3 to vector3Int.
        /// </summary>
        public static Vector3Int ToVector3Int(this Vector3 value)
        {
            return new Vector3Int(value.x.FloorToInt(), value.y.FloorToInt(), value.z.FloorToInt());
        }
        /// <summary>
        /// Converts vector3 to vector2Int.
        /// </summary>
        public static Vector2Int ToVector2Int(this Vector3 value)
        {
            return new Vector2Int(value.x.FloorToInt(), value.y.FloorToInt());
        }
        /// <summary>
        /// Returns the right vector normalized.
        /// </summary>
        /// <param name="vector"></param>
        public static Vector3 GetRightVectorFromWorldUp(this Vector3 vector)
        {
            return Vector3.Cross(Vector3.up, vector.normalized).normalized;
        }
        /// <summary>
        /// Zero not expecified vectors.
        /// </summary>
        public static Vector3 ZerOtherAxys(this Vector3 vector3, params Vector3[] axys)
        {
            Vector3 _result = default;

            foreach (var axy in axys)
                _result += axy.Multiply(vector3);

            return _result;
        }
        /// <summary>
        /// Multiply each component of the vectors.
        /// </summary>
        /// <param name="otherVector">The vector to multiply to this one.</param>
        public static Vector3 Multiply(this Vector3 vector3, Vector3 otherVector)
        {
            Vector3 _result = default;
            _result.x = vector3.x * otherVector.x;
            _result.y = vector3.y * otherVector.y;
            _result.z = vector3.z * otherVector.z;
            return _result;
        }
        /// <summary>
        /// Converts fromt world position to screen position.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="camera">The camera to use for conversion.if null,main camera is used.</param>
        /// <returns></returns>
        public static Vector2 WorldToScreenPoint(this Vector3 value, Camera camera = null)
        {
            if (camera != null)
                return camera.WorldToScreenPoint(value);
            else
                return Camera.main.WorldToScreenPoint(value);
        }
        /// <summary>
        /// Converts y axy to z axy of input source
        /// </summary>
        /// <param name="vector"></param>
        public static Vector3 ChangeYToZ(this Vector3 value)
        {
            return new Vector3(value.x, 0, value.y);
        }
        /// <summary>
        /// Returns the screen position of a 3d object.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="canvas">The canvas parent of the ui element.</param>
        /// <param name="camera">The camera rendering teh ui element.</param>
        /// <returns></returns>
        public static Vector2 GetScreenPositionOf3dObject(this Vector3 value, Canvas canvas, Camera camera = null)
        {
            var scaleFactor = canvas.scaleFactor;
            Vector2 screenPosition = value.WorldToScreenPoint(camera);

            return screenPosition / scaleFactor;
        }
    }
    public static class Vector2Extension
    {
        /// <summary>
        /// Covnerts a direction vector into direction enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PGCTools.Enums.Directions GetDirection(this Vector2 value)
        {
            var x = value.x;
            var y = value.y;

            if (x > 0 || x == 0)
            {
                if (x > y.Absolute())
                    return Directions.Right;
                else if (y > 0)
                    return Directions.Up;
                else if (y < 0)
                    return Directions.Down;
            }
            else if (x < 0)
            {
                if (x > y.Absolute())
                    return Directions.Left;
                else if (y > 0)
                    return Directions.Up;
                else if (y < 0)
                    return Directions.Down;
            }

            return default;
        }
        /// <summary>
        /// Checks if the data is equal to default or no.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDefault(this Vector2 value)
        {
            return value == default;
        }
        /// <summary>
        /// Converts a direction vector into direction enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Directions ToDirectionEnum(this Vector2 value)
        {
            if (value == Vector2.down) return Directions.Down;
            if (value == Vector2.up) return Directions.Up;
            if (value == Vector2.right) return Directions.Right;
            if (value == Vector2.left) return Directions.Left;
            return default;
        }
        /// <summary>
        /// Converts the current screen point position to world point using the main camera.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2 FromScreenPointToWorld(this Vector2 value)
        {
            var vector3Conversion = (Vector3)value;
            vector3Conversion.z = 10f;
            return Camera.main.ScreenToWorldPoint(vector3Conversion);
        }
        /// <summary>
        /// Converts vector2 to vector2Int.
        /// </summary>
        public static Vector2Int FloorToVector2Int(this Vector2 value)
        {
            return new Vector2Int(value.x.FloorToInt(), value.y.FloorToInt());
        }
        public static Vector2Int RoundToVector2Int(this Vector2 value)
        {
            return new Vector2Int(value.x.roundToInt(), value.y.roundToInt());
        }
        /// <summary>
        /// Converts vector2 to vector3Int.
        /// </summary>
        public static Vector3Int ToVector3Int(this Vector2 value)
        {
            return new Vector3Int(value.x.FloorToInt(), value.y.FloorToInt(), 0);
        }
        /// <summary>
        /// Converts y axy to z axy of input source
        /// </summary>
        /// <param name="vector"></param>
        public static Vector3 ChangeYToZ(this Vector2 value)
        {
            return new Vector3(value.x, 0, value.y);
        }
        /// <summary>
        /// Swap values of X axy and Y axy.
        /// </summary>
        public static Vector2 SwapValues(this Vector2 value)
        {
            return new Vector2(value.y, value.x);
        }
        /// <summary>
        /// Inverts the x value (Ex:If x is positive turns it negative and viceversa).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2 InvertXAxy(this Vector2 value)
        {
            return new Vector2(value.x * -1, value.y);
        }
        /// <summary>
        /// Inverts the y value (Ex:If y is positive turns it negative and viceversa).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2 InvertYAxy(this Vector2 value)
        {
            return new Vector2(value.x, value.y * -1);
        }
        /// <summary>
        /// Rounds its components.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2 Unnormalized(this Vector2 value)
        {
            return new Vector2(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
        }
        /// <summary>
        /// Gets a random value between teh value of x and the value of y.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetRandomValueInBetween(this Vector2 value)
        {
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            return UnityEngine.Random.Range(value.x, value.y);
        }
    }
    public static class VectorIntExtension
    {
        /// <summary>
        /// Gets a random value in between its axys.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetRandomValueInBetween(this Vector2Int value)
        {
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            return UnityEngine.Random.Range(value.x, value.y);
        }
    }
    public static class GameObjectExtension
    {
        /// <summary>
        /// Activates the game object.
        /// </summary>
        /// <param name="ob"></param>
        public static void ShowObject(this GameObject ob)
        {
            ob.SetActive(true);
        }
        /// <summary>
        /// Deactivates the game object.
        /// </summary>
        /// <param name="ob"></param>
        public static void HideObject(this GameObject ob)
        {
            ob.SetActive(false);
        }
        /// <summary>
        /// Destroys the object.
        /// </summary>
        public static void Destroy(this GameObject go)
        {
            GameObject.Destroy(go);
        }
        /// <summary>
        /// Gets the  hirarchy parh of a gme object in the scene
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetHierarchyPath(this GameObject value)
        {
            string result = string.Empty;

            if (value == null)
                return string.Empty;

            Transform parent = value.transform;

            do
            {
                result += parent.gameObject.name + "/";
                parent = parent.parent;
            } while (parent != null);

            var parentNames = result.Split("/");
            result = string.Empty;

            for (int i = parentNames.Length - 2; i >= 0; i--)
                result += "/" + parentNames[i];

            return result;
        }
    }
    public static class StringExtension
    {
        /// <summary>
        /// Returns if the string is null or empty or it is only white spaces.
        /// </summary>
        public static bool IsNullOrEmptyOrWhiteSpaced(this string ob)
        {
            return string.IsNullOrEmpty(ob) || string.IsNullOrWhiteSpace(ob);
        }
        /// <summary>
        /// Converts the string to an int.
        /// </summary>
        public static int ToInt(this string obj)
        {
            return int.Parse(obj);
        }
        /// <summary>
        /// Converts the string to a float.
        /// </summary>
        public static float ToFloat(this string obj)
        {
            return float.Parse(obj);
        }
        /// <summary>
        /// Gets its equivalent in the provided enum.
        /// </summary>
        /// <typeparam name="T">The enum.</typeparam>
        /// <param name="value"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }
        /// <summary>
        /// Converts a string of value 'true' or 'false' to bolean.
        /// </summary>
        public static bool ToBoolean(this string value)
        {
            return bool.Parse(value);
        }
        /// <summary>
        /// REmoves the characters from the string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="character">The characters to remove.</param>
        /// <returns></returns>
        public static string RemoveCharacters(this string value,string character)
        {
            return value.Replace(character, "");
        }
#if Playfab
        /// <summary>
        /// Converts a string to plyfabjsonobject.
        /// </summary>
        public static JsonObject ToJsonObject(this string value)
        {
            try
            {
                return (JsonObject)PlayFab.PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer).DeserializeObject(value);
            }
            catch (Exception e)
            {
                return new JsonObject();
            }
        }
#endif
    }
    public static class SelectableExtension
    {
        /// <summary>
        /// Makes it selectable.
        /// </summary>
        public static void Enable(this Selectable ob)
        {
            ob.interactable = true;
        }
        /// <summary>
        /// Makes it not selectable.
        /// </summary>
        public static void Disable(this Selectable ob)
        {
            ob.interactable = false;
        }
    }
    public static class BehaviourExtension
    {
        /// <summary>
        /// Disables an oject.
        /// </summary>
        public static void Disable(this UnityEngine.Behaviour go)
        {
            go.enabled = false;
        }
        /// <summary>
        /// Enables object.
        /// </summary>
        public static void Enable(this UnityEngine.Behaviour go)
        {
            go.enabled = true;
        }
        /// <summary>
        /// Destroys the object.
        /// </summary>
        public static void Destroy(this UnityEngine.Behaviour go)
        {
            GameObject.Destroy(go);
        }
        /// <summary>
        /// Hides hte game object containing this script.
        /// </summary>
        public static void HideGameObject(this UnityEngine.Behaviour go)
        {
            if (go.gameObject != null && go.gameObject.activeSelf)
                go.gameObject.SetActive(false);
        }
        /// <summary>
        /// Shows hte game object containing this script.
        /// </summary>
        public static void ShowGameObject(this UnityEngine.Behaviour go)
        {
            if (go.gameObject != null && !go.gameObject.activeSelf)
                go.gameObject.SetActive(true);
        }
        public static void DebugMessage(this Behaviour value, string message,LogType type = LogType.Log)
        {
            switch (type)
            {
                case LogType.Error:
                    Debug.LogError(value.name + ":" + message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(value.name + ":" + message);
                    break;
                case LogType.Log:
                    Debug.Log(value.name + ":" + message);
                    break;
            }
        }
    }
    public static class ComponentExtension
    {
        /// <summary>
        /// Destroys the object.
        /// </summary>
        public static void Destroy(this Component go)
        {
            GameObject.Destroy(go);
        }
        /// <summary>
        /// Destroyes the game object this component is attached to.
        /// </summary>
        public static void DestroyGameObject(this Component go)
        {
            GameObject.Destroy(go.gameObject);
        }
    }
    public static class ArrayExtension
    {
        /// <summary>
        /// Checks if the array is null or empty.
        /// </summary>
        public static bool IsNullOrEmpty(this System.Array ob)
        {
            return ob == null || ob.Length == 0;
        }
        /// <summary>
        ///Gets a random object from the list. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ob"></param>
        /// <returns></returns>
        public static T GetRandomValue<T>(this T[] ob) where T : class
        {
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            if (ob != null && ob.Length > 0)
            {
                var randomIndex = UnityEngine.Random.Range(0, ob.Length);
                return ob[randomIndex];
            }
            else
                return null;
        }
        public static string GetUTF8String(this byte[] ob)
        {
            if (ob != null && ob.Length > 0)
                return ASCIIEncoding.UTF8.GetString(ob);
            else return default;
        }
        /// <summary>
        /// Converts an array into a lsit of the same type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this T[] value)
        {
            return new List<T>(value);
        }
    }
    public static class InputActionExtension
    {
        /// <summary>
        /// Gets the device that triggered the input.
        /// </summary>
        public static string GetDevice(this UnityEngine.InputSystem.InputAction.CallbackContext value)
        {
            string _result = string.Empty;
            _result = value.action.activeControl.ToString().Split('/')[1];
            return _result;
        }
    }
    public static class FloatExtension
    {
        /// <summary>
        /// Converts the seconds count to a format time like(HH:MM:SS).
        /// </summary>
        public static string GetInTimeFormat(this float value)
        {
            string _result = string.Empty;
            int _hours = default;
            int _minutes = default;
            int _seconds = default;

            int _value = _seconds = value.FloorToInt();
            //get minuts and seconds
            if (_value >= 60)
            {
                _minutes = _value / 60;
                _seconds = _value % 60;
            }
            //get hours
            if (_minutes >= 60)
            {
                _hours = _minutes / 60;
                _minutes = _minutes % 60;
            }

            _result = $"{(_hours < 10 ? "0" + _hours : _hours)}:{(_minutes < 10 ? "0" + _minutes : _minutes)}:{(_seconds < 10 ? "0" + _seconds : _seconds)}";

            return _result;
        }
        /// <summary>
        /// Converts floor to int.
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public static int FloorToInt(this float ob)
        {
            return Mathf.FloorToInt(ob);
        }
        public static int roundToInt(this float ob)
        {
            return Mathf.RoundToInt(ob);
        }
        /// <summary>
        /// Converts the flot always to a positive value..
        /// </summary>
        /// <param name="value">The value to get its positive value.</param>
        /// <returns>The value turn into a positive number.</returns>
        public static float Absolute(this float value)
        {
            return Mathf.Abs(value);
        }
        /// <summary>
        /// Ruturns the decimals of a float.
        /// </summary>
        public static int GetDecimals(this float value)
        {
            string _result = string.Empty;
            string _valueString = value.ToString();
            if (!_valueString.Contains("."))
                return 0;
            int _periodIndex = _valueString.IndexOf(".");
            _result = _valueString.Substring(_periodIndex + 1, _valueString.Length - _periodIndex);
            return int.Parse(_result);
        }
        /// <summary>
        /// Returns the same number limited to the specified decimals.
        /// </summary>
        /// <param name="precision">The amount of decimals to return.</param>
        public static float GetWithPrecision(this float value, int precision)
        {
            float _result = default;
            bool _isNegative = default;
            if (!value.ToString().Contains("."))
                _result = value;
            else
            {
                string _value = value.ToString();
                _isNegative = _value.Contains("-");
                if (_isNegative)
                    _value = _value.Remove(0, 1);
                int _decimalStartindex = _value.IndexOf(".");
                if (_decimalStartindex + precision >= _value.Length - 1)
                    _result = value;
                else
                    _result = float.Parse(_value.Substring(0, _decimalStartindex + 1 + precision));
            }
            return _result * (_isNegative ? -1 : 1);
        }
        public static int RemoveComa(this float value)
        {
            int _result = default;
            string _value = value.ToString();
            int _decimalIndex = value.ToString().IndexOf(".");
            if (_decimalIndex == -1)
                _result = (int)value;
            else
                _result = int.Parse(value.ToString().Remove(_decimalIndex, 1));
            return _result;
        }
        /// <summary>
        /// inverts the value (Ex:If it is positive turns it negative and viceversa).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float Invert(this float value)
        {
            return value * -1;
        }
        /// <summary>
        /// Converts current float value into int.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this float value)
        {
            return (int)value;
        }
        /// <summary>
        /// Checks if a random generated number is less than the number.
        /// </summary>
        /// <param name="value">The percent to check.From 0+1</param>
        /// <returns></returns>
        public static bool MeetProbability(this float value)
        {
            if (value > 1)
                return false;

            UnityEngine.Random.InitState(DateTime.Now.Millisecond);

            var random = UnityEngine.Random.Range(0f, 1f);

            return random < value;
        }
        public static float GetJumpVerticalVelocity(this float value)
        {
            return MathF.Sqrt(Physics.gravity.y * -2.0f * value);
        }
    }
    public static class CameraExtension
    {
        /// <summary>
        /// Is the object inside camera spectrun?
        /// </summary>
        public static bool IsObjectColliderInSpectrun(this Camera camera, Bounds colliderBounds)
        {
            Plane[] _planes = GeometryUtility.CalculateFrustumPlanes(camera);
            if (GeometryUtility.TestPlanesAABB(_planes, colliderBounds))
                return true;
            return false;
        }
    }
    public static class MaterialExtension
    {
        public enum BlendMode
        {
            Opaque,
            Cutout,
            Fade,
            Transparent
        }
        /// <summary>
        /// Creates a new material based on this one with the new selected render mode.
        /// </summary>
        /// <param name="blendMode">The new render mode.</param>
        public static Material GetMaterialWithRenderMode(this Material standardShaderMaterial, BlendMode blendMode)
        {
            Material _newMaterial = new Material(standardShaderMaterial);
            switch (blendMode)
            {
                case BlendMode.Opaque:
                    _newMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    _newMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    _newMaterial.SetInt("_ZWrite", 1);
                    _newMaterial.DisableKeyword("_ALPHATEST_ON");
                    _newMaterial.DisableKeyword("_ALPHABLEND_ON");
                    _newMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    _newMaterial.renderQueue = -1;
                    break;
                case BlendMode.Cutout:
                    _newMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    _newMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    _newMaterial.SetInt("_ZWrite", 1);
                    _newMaterial.EnableKeyword("_ALPHATEST_ON");
                    _newMaterial.DisableKeyword("_ALPHABLEND_ON");
                    _newMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    _newMaterial.renderQueue = 2450;
                    break;
                case BlendMode.Fade:
                    _newMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    _newMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    _newMaterial.SetInt("_ZWrite", 0);
                    _newMaterial.DisableKeyword("_ALPHATEST_ON");
                    _newMaterial.EnableKeyword("_ALPHABLEND_ON");
                    _newMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    _newMaterial.renderQueue = 3000;
                    break;
                case BlendMode.Transparent:
                    _newMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    _newMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    _newMaterial.SetInt("_ZWrite", 0);
                    _newMaterial.DisableKeyword("_ALPHATEST_ON");
                    _newMaterial.DisableKeyword("_ALPHABLEND_ON");
                    _newMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    _newMaterial.renderQueue = 3000;
                    break;
            }
            return _newMaterial;
        }

    }
    public static class IntExtension
    {
        /// <summary>
        /// Covnerts an int to its representation in binary.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string ToBinaryRepresentation(this int n)
        {
            char[] b = new char[32];
            int pos = 31;
            int i = 0;

            while (i < 32)
            {
                if ((n & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return "Binary:" + new string(b);
        }
        /// <summary>
        /// Converts a single int value to an int array;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int[] ToArray(this int value)
        {
            return new int[] { value };
        }
        /// <summary>
        /// Adds the index array to value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="indexes"></param>
        /// <returns></returns>
        public static int AddToMask(this int value, int[] indexes)
        {
            foreach (var item in indexes)
                value |= (1 << item);

            return value;
        }
        /// <summary>
        /// Removes teh indexes from mask.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="indexes"></param>
        public static int RemoveFromMask(this int value, int[] indexes)
        {
            foreach (var item in indexes)
                value &= ~(1 << item);

            return value;
        }
        /// <summary>
        /// Converts the seconds count to a format time like(HH:MM:SS).
        /// </summary>
        public static string GetInTimeFormat(this int value)
        {
            string _result = string.Empty;
            int _hours = default;
            int _minutes = default;
            int _seconds = default;

            int _value = value;
            if (_value < 60)
                _seconds = _value;
            //get minutes and seconds
            if (_value >= 60)
            {
                _minutes = _value / 60;
                _seconds = _value % 60;
                _value = _minutes;
            }
            //get hours
            if (_value >= 60)
            {
                _hours = _minutes / 60;
                _minutes = _minutes % 60;
            }

            _result = $"{(_hours < 10 ? "0" + _hours : _hours)}:{(_minutes < 10 ? "0" + _minutes : _minutes)}:{(_seconds < 10 ? "0" + _seconds : _seconds)}";

            return _result;
        }
        /// <summary>
        /// Converts to bool.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>TRUE if it is different than 0 FALSE otehrwise.</returns>
        public static bool ToBool(this int value)
        {
            return value != 0;
        }
        /// <summary>
        /// Convrts an interget to a vector2int where each property is equal to the value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2Int ToVector2(this int value)
        {
            return Vector2Int.one * value;
        }
        public static T ToEnum<T>(this int value) where T : Enum
        {
            return (T)(object)value;
        }
        /// <summary>
        /// Checks if a random generated number is less than the number.
        /// </summary>
        /// <param name="value">The percent to check.From 0+100</param>
        /// <returns></returns>
        public static bool MeetProbability(this int value)
        {
            if (value > 100)
                return false;

            UnityEngine.Random.InitState(DateTime.Now.Millisecond);

            var random = UnityEngine.Random.Range(0f, 1f);

            value /= 100;

            return random < value;
        }
    }
    public static class TransformExtension
    {
        /// <summary>
        /// Destroy all children of this object.
        /// </summary>
        public static void ClearContent(this Transform transform)
        {
            if (transform == null)
                return;

            foreach (Transform item in transform)
                item.DestroyGameObject();
        }
    }
    public static class EnumExtension
    {
        /// <summary>
        /// Gets a random value.
        /// </summary>
        /// <typeparam name="T">The type of anum</typeparam>
        /// <param name="value"></param>
        /// <param name="skip">The min value accepted.</param>
        /// <returns></returns>>
        public static T GetRandomValue<T>(this T value, int skip = 1) where T : Enum
        {
            string[] values = Enum.GetNames(typeof(T));

            if (skip >= values.Length)
            {
                Debug.LogWarning("Invalid skip value");
                return value;
            }

            UnityEngine.Random.InitState(DateTime.Now.Millisecond);

            string _stringValue = values[UnityEngine.Random.Range(skip, values.Length)];

            return _stringValue.ToEnum<T>();
        }
        /// <summary>
        /// Gets a random value.
        /// </summary>
        /// <typeparam name="T">The type of anum</typeparam>
        /// <param name="value"></param>
        /// <param name="exclude">The values that should be excluded.</param>
        /// <returns></returns>
        public static T GetRandomValue<T>(this T value, T[] exclude) where T : Enum
        {
            List<string> selectableValues = new List<string>();

            var stringValues = Enum.GetNames(typeof(T));

            if (exclude.Length >= stringValues.Length)
            {
                Debug.LogWarning("Invalid exclude value");
                return value;
            }
            foreach (var item in stringValues)
            {
                if (exclude.Contains(item.ToEnum<T>()))
                    continue;
                selectableValues.Add(item);
            }

            UnityEngine.Random.InitState(DateTime.Now.Millisecond);

            string _stringValue = selectableValues[UnityEngine.Random.Range(0, selectableValues.Count)];

            return _stringValue.ToEnum<T>();
        }
        /// <summary>
        /// Gets a list of the enum type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<T> GetAllValuesWithoutNone<T>(this T value, params T[] exclude) where T : Enum
        {
            List<T> result = new List<T>();

            foreach (var item in value.GetStringValues())
            {
                if (item == "None" || exclude != default && exclude.Contains(item.ToEnum<T>()))
                    continue;

                if (item.Equals("None"))
                    continue;

                result.Add(item.ToEnum<T>());
            }

            return result;
        }
        /// <summary>
        /// Converts an enum string value into another enum with the same string value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="data">The enum.</param>
        /// <returns>The enum with teh correspinding string value or none if it is not found.</returns>
        public static T ToEnum<T>(this T value, Enum data) where T : Enum
        {
            if (!Enum.GetNames(typeof(T)).Contains(data.ToString()))
                return default;

            return data.ToString().ToEnum<T>();
        }
        /// <summary>
        /// Converts an enum to its value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt<T>(this T value) where T : Enum
        {
            return (int)(object)value;
        }
        /// <summary>
        /// Checks if the enum value is different from None.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid<T>(this T value) where T : Enum
        {
            return !value.ToString().Equals("None");
        }
        /// <summary>
        /// Return all values in a string array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] GetStringValues<T>(this T value) where T : Enum
        {
            return Enum.GetNames(typeof(T));
        }
        /// <summary>
        /// Converts a direction enum to its representation in vector3.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 GetDirectionVector<T>(this T value) where T : Enum
        {
            var direction = value.ToString().ToEnum<Directions>();

            switch (direction)
            {
                case Directions.None:
                    return default;
                case Directions.Up:
                    return Vector3.up;
                case Directions.Down:
                    return Vector3.down;
                case Directions.Right:
                    return Vector3.right;
                case Directions.Left:
                    return Vector3.left;
                case Directions.Front:
                    return Vector3.forward;
                case Directions.Back:
                    return Vector3.back;
                default:
                    return default;
            }
        }
        /// <summary>
        /// Returns the count of the elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetValueCout<T>(this T value) where T : Enum
        {
            return Enum.GetNames(typeof(T)).Length;
        }
#if Localization
        /// <summary>
        /// Gets the localized string of the provided table using value as key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="tableName">The table where the value is key.</param>
        /// <returns></returns>
        public static string GetLocalizedString<T>(this T value, string tableName) where T : Enum
        {
            return new LocalizedString(tableName, value.ToString()).GetLocalizedString();
        }
#endif
    }
    public static class ObjectExtension
    {
        /// <summary>
        /// Serializes to send over network.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Serialize(this object data)
        {
            BinaryFormatter _bf = new BinaryFormatter();
            MemoryStream _ms = new MemoryStream();

            _bf.Serialize(_ms, data);

            return _ms.ToArray();
        }
    }
    public static class SpriteRendererExtension
    {
        /// <summary>
        /// Scale the sprite renderer game obejct to fill screen.
        /// </summary>
        /// <param name="value"></param>
        public static void FillScreen(this SpriteRenderer value)
        {
            Vector2 _topRightCorner = new Vector2(1, 1);
            Vector2 _edgeVector = Camera.main.ViewportToWorldPoint(_topRightCorner);
            var _screenHeightInUnits = _edgeVector.y * 2;
            var _screenWidthInUnits = _edgeVector.x * 2;
            //get spriterenderer bounding size
            var _rendererBoundSize = value.bounds.size;

            value.transform.localScale = new Vector3(_screenWidthInUnits / _rendererBoundSize.x, _screenHeightInUnits / _rendererBoundSize.y, 1);
        }
    }
    public static class ListExtension
    {
        /// <summary>
        /// Gets a random value from list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetRandomValue<T>(this List<T> value, params T[] ignore)
        {
            List<T> tempList = new List<T>(value);

            if (ignore != null)
            {
                foreach (var item in ignore)
                    tempList.Remove(item);
            }

            if (tempList.Count == 0)
                return default;

            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            return tempList[UnityEngine.Random.Range(0, tempList.Count)];
        }
    }
    public static class BoolExtension
    {
        /// <summary>
        /// Converts to bool.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns 1 if it is true 0 if false.</returns>
        public static int ToInt(this bool value)
        {
            return (value ? 1 : 0);
        }
    }
    public static class DictionaryExtension
    {
        /// <summary>
        /// Gets the value of an entry of the given key if exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="J"></typeparam>
        /// <param name="value"></param>
        /// <param name="keyObject"></param>
        /// <returns></returns>
        public static KeyValuePair<T, J> GetValue<T, J>(this Dictionary<T, J> value, T keyObject) where T : UnityEngine.Object where J : UnityEngine.Object
        {
            foreach (var item in value)
            {
                if (item.Key.Equals(keyObject))
                    return item;
            }
            return default;
        }
        /// <summary>
        /// /// Gets the key of an entry of the given value if exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="J"></typeparam>
        /// <param name="value"></param>
        /// <param name="valueObject"></param>
        /// <returns></returns>
        public static KeyValuePair<T, J> GetKey<T, J>(this Dictionary<T, J> value, J valueObject) where T : UnityEngine.Object where J : UnityEngine.Object
        {
            foreach (var item in value)
            {
                if (item.Value != null && item.Value.Equals(valueObject))
                    return item;
            }
            return default;
        }
    }
#if UseAddressables
	public static class AddressablesExtension
	{
		[Obsolete]
		/// <summary>
		/// Loads a reference and call the action specified with the loaded asset.
		/// </summary>
		/// <param name="callback">The action to execute after loading.</param>
		/// <param name="parameters">The list of parameters needed for the callback.</param>
		public static IEnumerator LoadReferenceAs<T>(this AssetReference reference, Action<object[]> callback, params object[] parameters)
		{
			bool _loaded = default;
			if (reference.Asset == null)
				Addressables.LoadAssetAsync<T>(reference).Completed += (result) => callAction(result.Result, out _loaded);
			else
				callAction(default, out _loaded, true);
			while (!_loaded)
				yield return null;

			void callAction(T asset, out bool loaded, bool useReference = false)
			{
				object[] _newPrameters = new object[parameters.Length + 1];
				parameters.CopyTo(_newPrameters, 0);
				_newPrameters[parameters.Length] = useReference ? reference.Asset : asset;
				loaded = true;
				callback(_newPrameters);
			}
		}

	}
#endif
#if UseSaveData
    public static class SaveDataClassExtension
    {
        /// <summary>
        /// Saves the data to file.
        /// </summary>
        public static void SaveDataToFile(this SaveDataClass obj, int index)
        {
            string _saveFilePath = Path.Combine(Application.persistentDataPath, "data.dat");
            PGCTools.Classes.SaveDataClass[] _saveDataToSave = SaveData.LoadData();
            if (_saveDataToSave != null && index == _saveDataToSave.Length)
            {
                Debug.LogError("Save slot index out of range.");
                return;
            }
            else if (_saveDataToSave == null)
            {
                //initialize save data
                _saveDataToSave = new Classes.SaveDataClass[GameManager.Instance.MaxSavedSlots];
            }
            _saveDataToSave[index] = obj;
            FileStream _fStream = null;
            if (!File.Exists(_saveFilePath))
               _fStream=File.Create(_saveFilePath);
            else
             _fStream= new FileStream(_saveFilePath, FileMode.Truncate, FileAccess.Write);
            new BinaryFormatter().Serialize(_fStream, _saveDataToSave);
            _fStream.Close();
        }
    }
#endif
#if TMPro
#if Localization
    public static class TextMeshProExtension
    {
        /// <summary>
        /// Sets the text to empty string.
        /// </summary>
        public static void Clear(this TMP_InputField ob)
        {
            ob.text = "";
        }
        /// <summary>
        /// Sets a localized text with the given table nad key references.
        /// </summary>
        /// <param name="tableReference">The table where the key is found.</param>
        /// <param name="keyReference">The key of the localized text.</param>
        /// <param name="optionalTExtToAppendAtTheEnd">An optional string to append at the end.</param>
        public static void SetLocalizedText(this TextMeshProUGUI value, string tableReference, string keyReference,string optionalTExtToAppendAtTheEnd=default)
        {
            var text = new LocalizedString(tableReference, keyReference).GetLocalizedString();

            value.text = text+optionalTExtToAppendAtTheEnd;
        }
    }
#endif
#endif
#if FUSION_WEAVER
	public static class NetworkdictionaryExstension
	{
		/// <summary>
		/// Gets a random value.
		/// </summary>
		/// <typeparam name="K">The dictionary key type</typeparam>
		/// <typeparam name="T">the dictionary value type.</typeparam>
		public static KeyValuePair<K,T> GetRandomIndex<K,T> (this NetworkDictionary<K, T> value)
		{
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);

			var _randomIndex = UnityEngine.Random.Range(0, value.Count());

			int _counter = default;

			foreach (var item in value)
			{
				if (_counter == _randomIndex)
					return item;

				_counter++;
			}

			return default;
        }
	}
	public static class RunnerExtensions
	{
		/// <summary>
		/// Gets the component of type from the local player.
		/// </summary>
		/// <typeparam name="T">The type of the component to get.</typeparam>
		public static T GetLocalPlayerComponentOf<T>(this NetworkRunner runner) where T : Component
		{
			if (runner != null && runner.IsRunning)
			{
				if (runner.TryGetPlayerObject(runner.LocalPlayer, out var _playerNetworkObject) && _playerNetworkObject.TryGetComponent<T>(out var _component))
					return _component;
			}

			return null;
		}
	}
#endif
}