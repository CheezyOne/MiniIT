using UnityEngine;
using MiniIT.Windows;
using MiniIT.Managers;

namespace MiniIT
{
    public class GameButtons : MonoBehaviour
    {
        [SerializeField] private OptionsWindow optionsWindow = null;
        [SerializeField] private ShopWindow shopWindow = null;

        public void OnOptionsButton()
        {
            SoundsManager.Instance.PlaySound(SoundType.Button);
            WindowsManager.Instance.OpenWindow(optionsWindow);
        }

        public void OnShopButton()
        {
            SoundsManager.Instance.PlaySound(SoundType.Button);
            WindowsManager.Instance.OpenWindow(shopWindow);
        }
    }
}