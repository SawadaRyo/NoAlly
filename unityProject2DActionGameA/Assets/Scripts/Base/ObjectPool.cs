using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : UnityEngine.Object,IObjectPool
{
    T m_baseObj = null;
    Transform m_parent = null;
    List<T> m_pool = new List<T>();
    int m_index = 0;

    public void SetBaseObj(T obj, Transform parent)
    {
        m_baseObj = obj;
        m_parent = parent;
    }

    public void Pooling(T obj)
    {
        obj.DisactiveForInstantiate();
        m_pool.Add(obj);
    }

    public void SetCapacity(int size)
    {
        //if (size < Pool.Count) return;

        for (int i = m_pool.Count - 1; i < size; ++i)
        {
            T obj = default(T);
            if (m_parent)
            {
                obj = GameObject.Instantiate(m_baseObj, m_parent);
            }
            else
            {
                obj = GameObject.Instantiate(m_baseObj);
            }
            Pooling(obj);
        }
    }

    public T Instantiate()
    {
        T ret = null;
        for (int i = 0; i < m_pool.Count; ++i)
        {
            int newSIze = 0;
            int index = (m_index + i) % m_pool.Count;
            if (m_pool[index] == null)
            {
                newSIze++;
                continue;
            }
            else if (m_pool[index] != null && m_pool[index].IsActive) continue;

            m_pool[index].Create();
            ret = m_pool[index];
            break;
        }

        return ret;
    }
}
