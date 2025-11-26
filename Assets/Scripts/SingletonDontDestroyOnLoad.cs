using UnityEngine;

namespace MiniIT
{
    public class SingletonDontDestroyOnLoad<T> : MonoBehaviour where T : SingletonDontDestroyOnLoad<T>
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance != null && instance != (T)this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}