using UnityEngine;

namespace MiniIT
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
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
            instance = (T)this;
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}