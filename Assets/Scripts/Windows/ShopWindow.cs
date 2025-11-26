using UnityEngine;

namespace MiniIT.Windows
{
    public class ShopWindow : BaseWindow
    {
        [SerializeField] private CharactersConfig charactersConfig = null;
        [SerializeField] private ShopCharacter[] shopCharacters = null;

        public override void Init()
        {
            base.Init();

            for (int i = 0; i < shopCharacters.Length; i++)
            {
                shopCharacters[i].Init(charactersConfig.CharacterInfos[i]);
            }
        }
    }
}