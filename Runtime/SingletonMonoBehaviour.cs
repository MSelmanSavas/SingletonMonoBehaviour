using UnityEngine;

namespace SingletonMonoBehaviour
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        public static T Instance
        {
            get
            {
                if (Application.isEditor && !Application.isPlaying)
                {
                    return _editorInstance;
                }

                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        _instance = obj.AddComponent<T>();
                    }
                }

                return _instance;
            }

            protected set
            {
                if (Application.isEditor && !Application.isPlaying)
                {
                    _editorInstance = value;
                    return;
                }

                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        _instance = obj.AddComponent<T>();
                    }
                }

                _instance = value;
            }
        }

        protected static T _instance;

        protected static T _editorInstance;

        protected virtual bool dontDestroyOnLoad => false;

#if UNITY_EDITOR

        /// <summary>
        /// Used for SingletonMonoBehaviour logic. Please use OnValidateInternal instead;
        /// </summary>
        protected void OnValidate()
        {
            if (UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null ||
                UnityEditor.PrefabUtility.IsPartOfPrefabAsset(gameObject))
                return;

            _editorInstance = this as T;
            OnValidateInternal();
        }
#endif

        protected virtual void OnValidateInternal() { }

        /// <summary>
        /// Used for SingletonMonoBehaviour logic. Please use AwakeInternal instead;
        /// </summary>
        protected void Awake()
        {
            //Debug.LogError("Awake Instance : " + _instance);
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;

            if (dontDestroyOnLoad)
                DontDestroyOnLoad(this);

            AwakeInternal();
        }

        protected virtual void AwakeInternal() { }
    }

}
