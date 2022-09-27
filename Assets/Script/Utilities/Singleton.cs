using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//单例模式,管理类都要添加单例模式确保整个程序只能new一个来保证程序性能
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get => instance;
    }
    protected virtual void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (T)this;
        }
    }
    protected virtual void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
