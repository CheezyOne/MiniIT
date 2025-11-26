using System.Collections;
using TMPro;
using UnityEngine;
using MiniIT.Managers;

namespace MiniIT.Zones
{
    public class FreeCharacterZone : CharacterZone
    {
        [SerializeField] private Character characterPrefab = null;
        [SerializeField] private CharactersConfig charactersConfig = null;
        [SerializeField] private TMP_Text timerText = null;
        [SerializeField] private int spawnTime = 300;
        [SerializeField] private int newCharacterLevelStep = 1;

        private int remainingSpawnTime = 0;
        private WaitForSeconds waitSecond = null;
        private Coroutine spawnRoutine = null;

        private void Awake()
        {
            waitSecond = new(1f);
            spawnRoutine = StartCoroutine(SpawnFreeCharacter());
        }

        public override void OnPointerUp(Character character)
        {
            SetCharacter(character);
        }

        protected override void ValidPointerDown()
        {
            StopCoroutine(spawnRoutine);
            inputPanel.SetDraggedCharacter(currentCharacter);
        }

        public override bool IsAbleToTake(Character character)
        {
            return currentCharacter == character;
        }

        private IEnumerator SpawnFreeCharacter()
        {
            remainingSpawnTime = spawnTime;

            while (remainingSpawnTime > 0)
            {
                remainingSpawnTime--;
                int minutes = remainingSpawnTime / 60;
                int seconds = remainingSpawnTime % 60;
                timerText.text = $"{minutes}:{seconds:00}";
                yield return waitSecond;
            }

            int newCharacterIndex = 0;
            newCharacterIndex += SaveLoadSystem.data.Level / newCharacterLevelStep;
            Character newCharacter = PoolManager.Instance.InstantiateObject(characterPrefab, transform.position, Quaternion.identity, transform);
            newCharacter.SetInfo(charactersConfig.CharacterInfos[newCharacterIndex]);
            SetCharacter(newCharacter);
        }

        private void OnCharacterDropped()
        {
            if (currentCharacter == null || currentCharacter.transform.position == transform.position)
                return;

            spawnRoutine = StartCoroutine(SpawnFreeCharacter());
            currentCharacter = null;
        }

        private void OnEnable()
        {
            EventBus.onCharacterDropped += OnCharacterDropped;
        }

        private void OnDisable()
        {
            EventBus.onCharacterDropped -= OnCharacterDropped;
        }
    }
}