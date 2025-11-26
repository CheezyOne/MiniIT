using System;
using System.Collections;
using UnityEngine;
using Zenject;
using MiniIT.Zones;
using MiniIT.Windows;

namespace MiniIT.Managers
{
    public class AFKMoneyManager : MonoBehaviour
    {
        [SerializeField] private AFKMoneyWindow afkMoneyWindow = null;
        [SerializeField] private float minAFKSeconds = 60f;
        [SerializeField] private float saveInterval = 5f;

        [Inject] private CharactersSpawner charactersSpawner;

        private WaitForSeconds waitSave;

        private void Start()
        {
            waitSave = new(saveInterval);
            CheckAFKTime();
            StartCoroutine(SaveLastExitRoutine());
        }

        /// <summary>Checks how much time passed since last game exit</summary>
        private void CheckAFKTime()
        {
            if (string.IsNullOrEmpty(SaveLoadSystem.data.LastExitTime))
            {
                return;
            }

            DateTime lastExitTime = DateTime.Parse(SaveLoadSystem.data.LastExitTime);
            DateTime currentTime = DateTime.Now;
            TimeSpan timePassed = currentTime - lastExitTime;
            double totalSeconds = timePassed.TotalSeconds;

            if (totalSeconds >= minAFKSeconds)
            {
                ShowAFKMoney(totalSeconds);
            }
        }

        private void ShowAFKMoney(double seconds)
        {
            int reward = 0;

            foreach (ProfitZone zone in charactersSpawner.ProfitZones)
            {
                if (zone.IsOccupied)
                {
                    reward += zone.GetMoneyAfterTime((int)seconds);
                }
            }

            AFKMoneyWindow window = WindowsManager.Instance.OpenWindow(afkMoneyWindow);
            MoneyManager.Instance.AddMoney(reward);
            window.SetAFKInfo(seconds, reward);
        }

        private IEnumerator SaveLastExitRoutine()
        {
            while (true)
            {
                yield return waitSave;
                SaveLoadSystem.data.LastExitTime = DateTime.UtcNow.ToString("O");
                SaveLoadSystem.Instance.Save();
            }
        }
    }
}