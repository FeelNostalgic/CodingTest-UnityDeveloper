using System;
using System.IO;
using CodingTest.Data;
using CodingTest.Serializer;
using UnityEngine;

namespace CodingTest.Controllers
{
    public class SaveController
    {
        #region Private Variables

        private ISerializerService _serializer;

        #endregion
        
        #region Public Methods

        public SaveController(ISerializerService serializer)
        {
            _serializer = serializer;
        }
        
        public void SaveData(InputList inputList, string nameFile)
        {
            var path = GetPath(nameFile);
            SaveFile(inputList, path);
        }

        public InputList LoadData(string nameFile)
        {
            var path = GetPath(nameFile);
            return LoadFile<InputList>(path);
        }

        public bool ExitFileWithName(string nameFile)
        {
            var path = GetPath(nameFile);
            return File.Exists(path);
        }
        
        #endregion

        #region Private Methods

        private void SaveFile<T>(T fileToSave, string path)
        {
            try
            {
                string dataToSave = _serializer.Serialize(fileToSave);
                using StreamWriter writer = new StreamWriter(path);
                writer.Write(dataToSave);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private T LoadFile<T>(string path)
        {
            try
            {
                using StreamReader reader = new StreamReader(path);
                return _serializer.Deserialize<T>(reader.ReadToEnd());
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            return default;
        }
        
        private string GetPath(string nameFile)
        {
#if UNITY_EDITOR
            return $"{nameFile}{_serializer.Extension}";
#else
            return $"{Application.persistentDataPath}/{name}{_serializer.Extension}";
#endif
        }
        
        #endregion
    }
}