using UnityEngine;
using MiniIT.Managers;

namespace MiniIT.Windows
{
    public class OptionsWindow : BaseWindow
    {
        public void OnExitButton()
        {
            SoundsManager.Instance.PlaySound(SoundType.Button);
            Application.Quit();
        }
    }
}