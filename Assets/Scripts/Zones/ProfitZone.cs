using System.Collections;
using UnityEngine;
using MiniIT.Managers;

namespace MiniIT.Zones
{
    public class ProfitZone : CharacterZone
    {
        [SerializeField] private float profitTime = 3;
        [SerializeField] private Transform profitTextPosition = null;
        [SerializeField] private ProfitText profitText = null;
        [SerializeField] private CharactersConfig charactersConfig = null;

        private WaitForSeconds profitWait = null;
        private Coroutine profitRoutine = null;

        private void Awake()
        {
            profitWait = new(profitTime);
        }

        public override void OnPointerUp(Character character)
        {
            SetCharacter(character);
        }

        public override void SetCharacter(Character character)
        {
            if (currentCharacter == null)
            {
                base.SetCharacter(character);
            }
            else
            {
                StopCoroutine(profitRoutine);
                PoolManager.Instance.DestroyObject(currentCharacter.gameObject);
                character.SetInfo(charactersConfig.CharacterInfos[character.CharacterInfo.Level]);
                base.SetCharacter(character);
            }

            profitRoutine = StartCoroutine(GetProfit());
        }

        public override bool IsAbleToTake(Character character)
        {
            return currentCharacter == null || (currentCharacter.CharacterInfo.Level == character.CharacterInfo.Level && currentCharacter.CharacterInfo.Level < charactersConfig.CharacterInfos.Length);
        }

        public int GetMoneyAfterTime(int seconds)
        {
            return currentCharacter.CharacterInfo.Profit * seconds / (int)profitTime;
        }

        protected override void RemoveCharacter()
        {
            base.RemoveCharacter();
            StopCoroutine(profitRoutine);
        }

        private IEnumerator GetProfit()
        {
            while (currentCharacter != null)
            {
                yield return profitWait;
                int profit = currentCharacter.CharacterInfo.Profit;
                currentCharacter.PlayAnimation(CharacterAnimationType.Profit);
                PoolManager.Instance.InstantiateObject(profitText, profitTextPosition.position, Quaternion.identity, transform).SetText("+" + profit);
                MoneyManager.Instance.AddMoney(profit);
            }
        }
    }
}