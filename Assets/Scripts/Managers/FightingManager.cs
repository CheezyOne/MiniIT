using DG.Tweening;
using Doozy.Runtime.Reactor;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using MiniIT.Zones;

namespace MiniIT.Managers
{
    public class FightingManager : MonoBehaviour
    {
        [SerializeField] private Level[] levels = null;

        [Header("Enemy")]
        [SerializeField] private Image enemy = null;
        [SerializeField] private Vector3 enemyFallAngle = new(0, 180, -90);
        [SerializeField] private float enemyFallTime = 0.3f;

        [Header("Fight")]
        [SerializeField] private TMP_Text requirementText = null;
        [SerializeField] private Color notEnoughForceColor = new(211, 0, 0);
        [SerializeField] private Color enoughForceColor = new(255, 237, 0);
        [SerializeField] private FightingZone[] fightingZones = null;

        [SerializeField] private Progressor progressor = null;

        private bool isAnimating = false;

        private const string VICTORY_TEXT = "ПОБЕДА!";

        private void Start()
        {
            CheckForces();
        }

        /// <summary>If player has put enough characters in fighting zones then we complete this level</summary>
        private void CheckForces()
        {
            if (isAnimating)
            {
                return;
            }

            if (SaveLoadSystem.data.Level >= levels.Length)
            {
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                }

                requirementText.color = enoughForceColor;
                requirementText.text = VICTORY_TEXT;
                return;
            }

            int sumForce = 0;

            foreach (FightingZone zone in fightingZones)
            {
                if (zone.CurrentCharacter == null)
                {
                    continue;
                }

                sumForce += zone.CurrentCharacter.CharacterInfo.Level;
            }

            if (sumForce >= levels[SaveLoadSystem.data.Level].RequieredForce)
            {
                requirementText.color = enoughForceColor;
                CompleteLevel();
            }
            else
            {
                requirementText.color = notEnoughForceColor;
            }

            progressor.SetProgressAt((float)SaveLoadSystem.data.Level / (float)levels.Length);
            enemy.sprite = levels[SaveLoadSystem.data.Level].Sprite;
            requirementText.text = sumForce + "/" + levels[SaveLoadSystem.data.Level].RequieredForce;
        }

        private void CompleteLevel()
        {
            foreach (FightingZone zone in fightingZones)
            {
                if (zone.CurrentCharacter == null)
                {
                    continue;
                }

                isAnimating = true;
                zone.CurrentCharacter.PlayAnimation(CharacterAnimationType.Attack);
                enemy.transform.DOKill();
                enemy.transform.DOLocalRotate(enemyFallAngle, enemyFallTime).OnComplete(() => SetNextLevel());
            }
        }

        private void SetNextLevel()
        {
            SaveLoadSystem.data.Level++;
            SaveLoadSystem.Instance.Save();
            enemy.transform.DOLocalRotate(Vector3.zero, enemyFallTime);
            isAnimating = false;
            CheckForces();
        }

        private void OnEnable()
        {
            EventBus.onCharacterTaken += CheckForces;
            EventBus.onCharacterDropped += CheckForces;
        }

        private void OnDisable()
        {
            EventBus.onCharacterTaken -= CheckForces;
            EventBus.onCharacterDropped -= CheckForces;
        }
    }

    [Serializable]
    public class Level
    {
        public Sprite Sprite;
        public int RequieredForce;
    }
}