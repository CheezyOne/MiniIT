using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniIT.Managers
{
    public class SaveLoadSystem : SingletonDontDestroyOnLoad<SaveLoadSystem>
    {
        public static PlayerData data;

        private const string PROGRESS_KEY = "progress";

        private void Start()
        {
            Load();
        }

        public void Save()
        {
            string jsonString = JsonUtility.ToJson(data);

#if UNITY_EDITOR
            PlayerPrefs.SetString(PROGRESS_KEY, jsonString);
            PlayerPrefs.Save();
#else
        string filePath = Application.persistentDataPath + "/save.json";
        System.IO.File.WriteAllText(filePath, jsonString);
#endif
        }

        private void Load()
        {
            string jsonString = "";

#if UNITY_EDITOR
            if (PlayerPrefs.HasKey(PROGRESS_KEY))
            {
                jsonString = PlayerPrefs.GetString(PROGRESS_KEY);
            }
#else
        string filePath = Application.persistentDataPath + "/save.json";

        if (System.IO.File.Exists(filePath))
        {
            jsonString = System.IO.File.ReadAllText(filePath);
        }
#endif

            if (!string.IsNullOrEmpty(jsonString))
            {
                data = JsonUtility.FromJson<PlayerData>(jsonString);
            }
            else
            {
                data = new PlayerData();
            }

            EventBus.onSavesLoaded?.Invoke();
        }
    }

    [Serializable]
    public class PlayerData
    {
        public bool SoundsOn = true;
        public int Level = 0;
        public int Money = 25;
        public List<ZoneSaveData> OccupiedZones = new();
        public string LastExitTime = DateTime.UtcNow.ToString("O");
    }
}