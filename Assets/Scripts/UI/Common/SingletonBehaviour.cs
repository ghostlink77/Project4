using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    protected bool IsDestroyOnLoad { get; set; } = false;
    private static T s_instance;

    public static T Instance => s_instance;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        s_instance = this as T;
        if (IsDestroyOnLoad == false)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        Dispose();
    }

    protected virtual void Dispose()
    {
        s_instance = null;
    }
}
