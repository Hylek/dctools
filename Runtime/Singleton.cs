using UnityEngine;

namespace DCTools
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = (T)FindObjectOfType(typeof(T));
                
                if (_instance == null)
                {
                    SetupInstance();
                }

                return _instance;
            }
        }

        public virtual void Awake() => RemoveDuplicates();

        private static void SetupInstance()
        {
            _instance = (T)FindObjectOfType(typeof(T));
            
            if (_instance != null) return;
            
            var gameObj = new GameObject
            {
                name = typeof(T).Name
            };
            
            _instance = gameObj.AddComponent<T>();
            DontDestroyOnLoad(gameObj);
        }

        private void RemoveDuplicates()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
