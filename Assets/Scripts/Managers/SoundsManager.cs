using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniIT.Managers
{
    public class SoundsManager : Singleton<SoundsManager>
    {
        [SerializeField] private SoundsData[] soundsDatas = null;

        private Dictionary<SoundType, AudioClipData> soundIdPairs = new();

        protected override void Awake()
        {
            base.Awake();

            foreach (SoundsData soundData in soundsDatas)
            {
                soundIdPairs.Add(soundData.SoundType, soundData.AudioClipData);
            }
        }

        public void PlaySound(SoundType soundType, bool stopPreviousSound = false)
        {
            if (!SaveLoadSystem.data.SoundsOn)
            {
                return;
            }

            if (!soundIdPairs.ContainsKey(soundType))
            {
                Debug.LogWarning($"There's no audioclip for sound type {soundType}!");
                return;
            }

            if (stopPreviousSound)
            {
                soundIdPairs[soundType].AudioSource.Stop();
            }

            soundIdPairs[soundType].AudioSource?.PlayOneShot(soundIdPairs[soundType].AudioClip);
        }

        [Serializable]
        public class AudioClipData
        {
            public AudioClip AudioClip;
            public AudioSource AudioSource;
        }

        [Serializable]
        public class SoundsData
        {
            public SoundType SoundType;
            public AudioClipData AudioClipData;
        }
    }

    public enum SoundType
    {
        /// <summary>All buttons sound</summary>
        Button = 0,
        /// <summary>Character purchase sound</summary>
        Purchase = 1,
    }
}