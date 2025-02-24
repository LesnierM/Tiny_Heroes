using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace PGCTools.Classes
{
    [System.Serializable]
    public class SaveDataClass
    {
        public static SaveDataClass[] LoadData()
        {
            string _saveFilePath = Path.Combine(Application.persistentDataPath, "data.dat");
            SaveDataClass[] _result=null;
            if (!File.Exists(_saveFilePath))
                return null;
            FileStream _fStream = new FileStream(_saveFilePath, FileMode.Open, FileAccess.Read);
            try
            {
                _result = new BinaryFormatter().Deserialize(_fStream) as SaveDataClass[];
            }
            catch { }
            _fStream.Close();
            return _result;
        }
        public static byte[] Serialize(object customType)
        {
            List<byte> _resul = new List<byte>();
            //var _object = (Type)customType;
            byte[] _tempData;
            #region Copy data
            #endregion
            return _resul.ToArray();
        }
        public static SaveDataClass Deserialize(byte[] data)
        {
            var _result = new SaveDataClass();
            #region Read data
            #endregion
            return _result;
        }
    }
}