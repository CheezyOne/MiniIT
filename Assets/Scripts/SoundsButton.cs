using UnityEngine;
using UnityEngine.UI;
using MiniIT.Managers;

namespace MiniIT
{
    public class SoundsButton : MonoBehaviour
    {
        [SerializeField] private Sprite soundsOn = null;
        [SerializeField] private Sprite soundsOff = null;
        [SerializeField] private Image soundsImage = null;

        public void Awake()
        {
            SetSprite();
        }

        public void OnSoundsButton()
        {
            SaveLoadSystem.data.SoundsOn = !SaveLoadSystem.data.SoundsOn;
            SaveLoadSystem.Instance.Save();
            SoundsManager.Instance.PlaySound(SoundType.Button);
            SetSprite();
        }

        private void SetSprite()
        {
            if (SaveLoadSystem.data.SoundsOn)
            {
                soundsImage.sprite = soundsOn;
            }
            else
            {
                soundsImage.sprite = soundsOff;
            }
        }
    }
}