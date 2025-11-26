using UnityEngine;
using UnityEngine.UI;
using MiniIT.Managers;

namespace MiniIT.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        [SerializeField] private Button closeButton = null;

        public virtual void Init()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(OnCloseButton);
            }
        }

        public void OnCloseButton()
        {
            SoundsManager.Instance.PlaySound(SoundType.Button);
            Close();
        }

        public void Close()
        {
            WindowsManager.Instance?.CloseWindow(GetType());
        }

        public virtual void OnClose() { }
    }
}