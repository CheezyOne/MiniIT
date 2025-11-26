namespace MiniIT.Managers
{
    public class MoneyManager : Singleton<MoneyManager>
    {
        public bool CanSpendMoney(int price)
        {
            return SaveLoadSystem.data.Money >= price;
        }

        public void AddMoney(int money)
        {
            SaveLoadSystem.data.Money += money;
            OnMoneyChanged();
        }

        public void SpendMoney(int price)
        {
            SaveLoadSystem.data.Money -= price;
            OnMoneyChanged();
        }

        private void OnMoneyChanged()
        {
            SaveLoadSystem.Instance.Save();
            EventBus.onMoneyChanged?.Invoke();
        }
    }
}