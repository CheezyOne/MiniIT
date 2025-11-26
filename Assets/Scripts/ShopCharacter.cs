using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using MiniIT.Managers;

namespace MiniIT
{
    public class ShopCharacter : MonoBehaviour
    {
        [SerializeField] private Image characterImage = null;
        [SerializeField] private TMP_Text characterName = null;
        [SerializeField] private TMP_Text characterPrice = null;
        [SerializeField] private Button purchaseButton = null;
        [SerializeField] private GameObject noPlacesWarning = null;

        [Inject] private CharactersSpawner spawner = null;
        private int price = 0;
        private CharacterInfo characterInfo = null;

        [Inject]
        public void Construct(CharactersSpawner spawner)
        {
            this.spawner = spawner;
        }

        public void Init(CharacterInfo characterInfo)
        {
            this.characterInfo = characterInfo;
            characterName.text = this.characterInfo.Name;
            price = this.characterInfo.Price;
            characterPrice.text = price.ToString();
            characterImage.sprite = this.characterInfo.Sprite;
            UpdatePurchaseButton();
        }

        public void OnPurchaseButton()
        {
            spawner.SpawnCharacter(characterInfo);
            SoundsManager.Instance.PlaySound(SoundType.Purchase);
            MoneyManager.Instance.SpendMoney(price);
        }

        private void UpdatePurchaseButton()
        {
            if (!spawner.HasEmptyZone())
            {
                noPlacesWarning.SetActive(true);
                purchaseButton.interactable = false;
                return;
            }

            purchaseButton.interactable = MoneyManager.Instance.CanSpendMoney(price);
        }

        private void OnEnable()
        {
            EventBus.onMoneyChanged += UpdatePurchaseButton;
        }

        private void OnDisable()
        {
            EventBus.onMoneyChanged -= UpdatePurchaseButton;
        }
    }
}