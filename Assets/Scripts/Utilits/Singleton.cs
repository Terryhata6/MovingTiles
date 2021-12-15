using UnityEngine;

namespace Core.UtilitsSpace
{
    public class Singleton<T> : MonoBehaviour 
    where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var objs = FindObjectsOfType(typeof(T), true) as T[];
                    if (objs.Length > 0)
                    {
                        _instance = objs[0];
                    }
                    if (objs.Length > 1)
                    {
                        Debug.LogWarning($"More than one instance of {typeof(T).Name} in scene");   
                    }
                    if (_instance == null)
                    {
                        var obj = new GameObject();
                        obj.name = (typeof(T)).ToString();
                        obj.hideFlags = HideFlags.HideAndDontSave;
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
    }
}