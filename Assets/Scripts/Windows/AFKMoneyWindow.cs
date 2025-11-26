using System;
using TMPro;
using UnityEngine;

namespace MiniIT.Windows
{
    public class AFKMoneyWindow : BaseWindow
    {
        [SerializeField] private TMP_Text afkTimeText = null;
        [SerializeField] private TMP_Text afkRewardText = null;

        public void SetAFKInfo(double seconds, int reward)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            string timeText = FormatTimeSpan(timeSpan);
            afkTimeText.text = timeText;
            afkRewardText.text = reward.ToString();
        }

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan.Days > 0)
                return $"{timeSpan.Days}d {timeSpan.Hours}h {timeSpan.Minutes}m";
            else if (timeSpan.Hours > 0)
                return $"{timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s";
            else if (timeSpan.Minutes > 0)
                return $"{timeSpan.Minutes}m {timeSpan.Seconds}s";
            else
                return $"{timeSpan.Seconds}s";
        }
    }
}