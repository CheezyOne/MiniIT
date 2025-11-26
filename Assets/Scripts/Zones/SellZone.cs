using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using MiniIT.Managers;
using MiniIT.InputSystem;

namespace MiniIT.Zones
{
    public class SellZone : Zone
    {
        [SerializeField] private TMP_Text sellMoneyText;
        [SerializeField] private int sellDivider = 2;
        [SerializeField] private Image image;

        [Inject] private InputPanel inputPanel;

        public override void OnPointerUp(Character character)
        {
            MoneyManager.Instance.AddMoney(character.CharacterInfo.Price / sellDivider);
            PoolManager.Instance.DestroyObject(character.gameObject);
        }

        private void OnCharacterTaken()
        {
            image.enabled = true;
            sellMoneyText.text = (inputPanel.DraggedCharacter.CharacterInfo.Price / sellDivider).ToString();
        }

        private void OnCharacterDropped()
        {
            image.enabled = false;
            sellMoneyText.text = string.Empty;
        }

        private void OnEnable()
        {
            EventBus.onCharacterTaken += OnCharacterTaken;
            EventBus.onCharacterDropped += OnCharacterDropped;
        }

        private void OnDisable()
        {
            EventBus.onCharacterTaken -= OnCharacterTaken;
            EventBus.onCharacterDropped -= OnCharacterDropped;
        }
    }
}