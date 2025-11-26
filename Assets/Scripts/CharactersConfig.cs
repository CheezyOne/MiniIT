using System;
using UnityEngine;

namespace MiniIT
{

    [CreateAssetMenu(menuName = "Game/Characters Config")]
    public class CharactersConfig : ScriptableObject
    {
        [SerializeField] private CharacterInfo[] characterInfos = null;

        public CharacterInfo[] CharacterInfos => characterInfos;
    }

    [Serializable]
    public class CharacterInfo
    {
        public Sprite Sprite;
        public int Price;
        public int Level;
        public string Name;
        public int Profit;
    }
}