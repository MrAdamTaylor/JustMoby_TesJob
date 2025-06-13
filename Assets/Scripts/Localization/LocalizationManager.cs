using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Localization
{
    public class LocalizationManager 
    {
        public static LocalizationData Data { get; set; }

        private static List<ILocalizable> _localizables = new();
    
        public void LoadLocalization()
        {
            string path = Path.Combine(Application.dataPath+MessagesKey.ADDITIONAL_PATH, "localization.json");
            string json = File.ReadAllText(path);
            Data = JsonConvert.DeserializeObject<LocalizationData>(json);
        
            Data.SetRus();
        }
    
        public void AddILocalizable(ILocalizable localizable)
        {
            _localizables.Add(localizable);
            localizable.Data = Data;
        }

        [MenuItem("Lang/Rus")]
        public static void ChangeToRus()
        {
            Data.SetRus();
            for (int i = 0; i < _localizables.Count; i++)
            {
                _localizables[i].Localize();
            }
        }
    
        [MenuItem("Lang/Eng")]
        public static void ChangeToEng()
        {
            for (int i = 0; i < _localizables.Count; i++)
            {
                _localizables[i].Localize();
            }
            Data.SetEng();
        }

    }
}
