using System;
using UnityEngine;
using UnityEngine.UI;

namespace MiniIT
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform = null;
        [SerializeField] private Image image = null;
        [SerializeField] private Animator animator = null;
        [SerializeField] private CharacterAnimation[] animations = null;

        private CharacterInfo characterInfo = null;

        public RectTransform RectTransform => rectTransform;
        public Animator Animator => animator;
        public CharacterInfo CharacterInfo => characterInfo;

        public void SetInfo(CharacterInfo characterInfo)
        {
            this.characterInfo = characterInfo;
            image.sprite = this.characterInfo.Sprite;
        }

        public void PlayAnimation(CharacterAnimationType animationType)
        {
            string triggerName = string.Empty;

            foreach (CharacterAnimation animation in animations)
            {
                if (animation.AnimationType == animationType)
                {
                    triggerName = animation.TriggerName;
                }
            }

            if (triggerName == string.Empty)
            {
                return;
            }

            animator.SetTrigger(triggerName);
        }
    }

    [Serializable]
    public struct CharacterAnimation
    {
        public CharacterAnimationType AnimationType;
        public string TriggerName;
    }

    public enum CharacterAnimationType
    {
        /// <summary>Animation of attack FightingZone</summary>
        Attack = 0,
        /// <summary>Animation of getting money in ProfitZone</summary>
        Profit = 1,
    }
}