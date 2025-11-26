using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MiniIT.Zones;

namespace MiniIT.Managers
{
    public class CharactersSpawner : MonoBehaviour
    {
        [SerializeField] private Character characterPrefab = null;
        [SerializeField] private ProfitZone[] profitZones = null;
        [SerializeField] private CharacterZone[] fightingZones = null;
        [SerializeField] private CharactersConfig charactersConfig = null;

        private Dictionary<string, CharacterZone> allZonesDictionary = new();
        private List<CharacterZone> allZones = new();

        public ProfitZone[] ProfitZones => profitZones;

        private void Awake()
        {
            allZones.AddRange(profitZones);
            allZones.AddRange(fightingZones);

            foreach (CharacterZone zone in allZones)
            {
                allZonesDictionary[zone.ZoneId] = zone;
            }

            LoadZonesState();
        }

        public void SpawnCharacter(CharacterInfo characterInfo)
        {
            for (int i = 0; i < profitZones.Length; i++)
            {
                if (!profitZones[i].IsOccupied)
                {
                    Character newCharacter = PoolManager.Instance.InstantiateObject(characterPrefab, profitZones[i].transform.position, Quaternion.identity, profitZones[i].transform);
                    newCharacter.SetInfo(characterInfo);
                    profitZones[i].SetCharacter(newCharacter);
                    break;
                }
            }
        }

        public bool HasEmptyZone()
        {
            foreach (CharacterZone zone in profitZones)
            {
                if (!zone.IsOccupied)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Saves characters in zones</summary>
        private void SaveZonesState()
        {
            SaveLoadSystem.data.OccupiedZones.Clear();

            foreach (CharacterZone zone in allZones)
            {
                if (!zone.IsOccupied)
                {
                    continue;
                }

                ZoneSaveData zoneData = new ZoneSaveData
                {
                    ZoneId = zone.ZoneId,
                    CharacterLevel = zone.CurrentCharacterLevel
                };

                SaveLoadSystem.data.OccupiedZones.Add(zoneData);
            }

            SaveLoadSystem.Instance.Save();
        }

        /// <summary>Loads saved characters in zones</summary>
        private void LoadZonesState()
        {
            foreach (ZoneSaveData zoneData in SaveLoadSystem.data.OccupiedZones)
            {
                if (allZonesDictionary.TryGetValue(zoneData.ZoneId, out CharacterZone zone))
                {
                    Character character = CreateCharacterByLevel(zoneData.CharacterLevel);
                    zone.SetCharacter(character);
                }
            }
        }

        private Character CreateCharacterByLevel(int level)
        {
            CharacterInfo characterInfo = charactersConfig.CharacterInfos.FirstOrDefault(info => info.Level == level);

            Character character = PoolManager.Instance.InstantiateObject(
                characterPrefab,
                Vector3.zero,
                Quaternion.identity,
                null
            );

            character.SetInfo(characterInfo);
            return character;
        }

        private void OnEnable()
        {
            EventBus.onCharacterDropped += SaveZonesState;
        }

        private void OnDisable()
        {
            EventBus.onCharacterDropped -= SaveZonesState;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveZonesState();
            }
        }

        private void OnApplicationQuit()
        {
            SaveZonesState();
        }
    }

    [Serializable]
    public class ZoneSaveData
    {
        public string ZoneId;
        public int CharacterLevel;
    }
}