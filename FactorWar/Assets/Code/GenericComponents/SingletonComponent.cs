using UnityEngine;
using System.Collections;
[AddComponentMenu("Generic/Singleton")]
public abstract class SingletonComponent<T> : MonoBehaviour where T : SingletonComponent<T>
{
    protected static T instance = null;
    public static bool isBeeingDestroyed = false;
    /// <summary>
    /// Returns the instance of this singleton.
    /// </summary>
    /// <value>The instance.</value>
    public static T Instance
    {
        get
        {
            if(isBeeingDestroyed)
            {
                return null;
            }
            if(!instance)
            {
                instance = FindObjectOfType<T>();
            }
            if (!instance)
            {
                GameObject singleton = new GameObject();
                instance = singleton.AddComponent<T>();
                singleton.name = "(singleton)" + typeof(T).ToString();
                DontDestroyOnLoad(singleton);
                Debugger.printLog("[Singleton] An instance of " + typeof(T) + " is needed in the scene, so '" + singleton + "' was created with DontDestroyOnLoad.");
            }
            else
            {
                Debugger.printLog("[Singleton] Using instance already created: " + instance.gameObject.name);
            }
            return instance;
        }
    }
}