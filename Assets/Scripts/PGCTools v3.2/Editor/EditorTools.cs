using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

 namespace PGCTools
{
    public static class EditorTools
    {
        /// <summary>
        /// Adds a new enum item.
        /// </summary>
        /// <typeparam name="T">The enum.</typeparam>
        /// <param name="fileName">The name of the enum script (The enum must be in an individual file).</param>
        /// <param name="newEnumEntryName">The name of the new item to add.</param>
        /// <returns>The enum new item.</returns>
        public static T AddNewEnumItem<T>(string fileName, ref string newEnumEntryName) where T : Enum
        {
            var _results = Directory.GetFiles(Directory.GetCurrentDirectory(), fileName, SearchOption.AllDirectories);
            if (_results.Length == 0)
                return (T)Enum.Parse(typeof(T), "-1");
            string _enemyTypesFilePath = _results[0];
            if (string.IsNullOrEmpty(newEnumEntryName))
                return (T)Enum.Parse(typeof(T), "-1");

            //change spaces by underscore
            newEnumEntryName = newEnumEntryName.Replace(' ', '_');

            FileStream _fs = new FileStream(_enemyTypesFilePath, FileMode.Open, FileAccess.ReadWrite);
            StreamReader _sr = new StreamReader(_fs);
            StreamWriter _sw = new StreamWriter(_fs);

            for (int i = 0; i < _fs.Length; i++)
            {
                _fs.Position = i;
                if ((char)_sr.Peek() == '}')
                {
                    _fs.Position += 1;
                    _sw.Write($",{Environment.NewLine}\t{newEnumEntryName}{Environment.NewLine}{'}'}");
                    break;
                }
                _sr.Read();
            }
            newEnumEntryName = "";
            _sw.Flush();
            _sr.Close();
            _fs.Close();
            AssetDatabase.Refresh();
            var _lastFieldIndex = Enum.GetValues(typeof(T)).Length.ToString();
            return (T)Enum.Parse(typeof(T), _lastFieldIndex);
        }
        /// <summary>
        /// Renames an asset.
        /// </summary>
        /// <param name="assetObject">The asset to rename.</param>
        /// <param name="newName">The new name.</param>
        public static void RenameAssetFile(UnityEngine.Object assetObject,string newName)
        {
            var _relativeFilePath = AssetDatabase.GetAssetPath(assetObject);
            AssetDatabase.RenameAsset(_relativeFilePath,newName);
            assetObject.name = newName;
            AssetDatabase.Refresh();
            Selection.activeObject = assetObject;
        }
    }
}