using TMPro;
using UnityEngine;
using MiniIT.Managers;

namespace MiniIT
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text text = null;

        private void Awake()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            text.text = SaveLoadSystem.data.Money.ToString();
        }

        private void OnEnable()
        {
            EventBus.onMoneyChanged += UpdateText;
        }

        private void OnDisable()
        {
            EventBus.onMoneyChanged -= UpdateText;
        }
    }
}