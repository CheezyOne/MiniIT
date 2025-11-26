using UnityEngine;
using Zenject;
using MiniIT.InputSystem;

namespace MiniIT.Zones
{
    public class CharacterZone : Zone
    {
        [SerializeField] private RectTransform rectTransform = null;
        [SerializeField] private string zoneId = string.Empty;

        [Inject] protected InputPanel inputPanel = null;

        protected Character currentCharacter = null;

        public bool IsOccupied => currentCharacter != null;
        public string ZoneId => zoneId;
        public int CurrentCharacterLevel => currentCharacter.CharacterInfo.Level;

        public override void OnPointerUp(Character character)
        {
            SetCharacter(character);
        }

        public override void OnPointerDown()
        {
            if (currentCharacter == null)
            {
                return;
            }

            ValidPointerDown();
        }

        public virtual void SetCharacter(Character character)
        {
            currentCharacter = character;
            currentCharacter.transform.position = transform.position;
            currentCharacter.transform.SetParent(transform);
            StretchCharacter();
        }

        public override bool IsAbleToTake(Character character)
        {
            return currentCharacter == null;
        }

        protected virtual void RemoveCharacter()
        {
            currentCharacter = null;
        }

        protected virtual void ValidPointerDown()
        {
            inputPanel.SetDraggedCharacter(currentCharacter);
            RemoveCharacter();
        }

        private void StretchCharacter()
        {
            currentCharacter.Animator.enabled = false;
            currentCharacter.RectTransform.sizeDelta = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
            currentCharacter.Animator.enabled = true;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            zoneId = $"zone_{gameObject.name}_{rectTransform.transform.position}";
        }
#endif
    }
}