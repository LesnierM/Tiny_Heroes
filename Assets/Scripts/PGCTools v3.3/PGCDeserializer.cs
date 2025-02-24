using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace PGCTools.Serialization
{
    public static class PGCDeserializer
    {
        static BinaryFormatter _bf = new BinaryFormatter();
        static MemoryStream _ms;

        /// <summary>
        /// Deserializes data.
        /// </summary>
        /// <typeparam name="T">The type to deserialize to.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static T Deserializer<T>(byte[] data)where T: class
        {
            _ms = new MemoryStream(data);
            try
            {
                return _bf.Deserialize(_ms) as T;
            }catch (Exception ex)
            {
                Debug.LogError("Error deserializing data." + ex.Message);
            }
            return null;
        }
    }
}
