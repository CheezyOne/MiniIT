using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniIT.Managers
{
    public class GameInitializer : MonoBehaviour
    {
        private const string GAME_SCENE_NAME = "Game";

        private void OnSavesLoaded()
        {
            SceneManager.LoadScene(GAME_SCENE_NAME);
        }

        protected void OnEnable()
        {
            EventBus.onSavesLoaded += OnSavesLoaded;
        }

        private void OnDisable()
        {
            EventBus.onSavesLoaded -= OnSavesLoaded;
        }
    }
}