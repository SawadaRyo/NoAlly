using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                Type t = typeof(T);
                m_instance = (T)FindObjectOfType(t);
                if (m_instance == null)
                {
                    Debug.LogError($"{t}をアタッチしているGameObjectがありません");
                }
            }
            return m_instance;
        }
    }

    private void Awake()
    {
        CheckIns();
    }
    bool CheckIns()
    {
        if (m_instance == null)
        {
            m_instance = this as T;
            return true;
        }
        else if (m_instance == this)
        {
            return true;
        }
        Destroy(this);
        return false;
    }
}
