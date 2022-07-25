using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : UnityEngine.Object,IObjectPool
{
    T BaseObj = null;
    Transform Parent = null;
    List<T> Pool = new List<T>();
    int Index = 0;

    public void SetBaseObj(T obj, Transform parent)
    {
        BaseObj = obj;
        Parent = parent;
    }

    public void Pooling(T obj)
    {
        obj.DisactiveForInstantiate();
        Pool.Add(obj);
    }

    public void SetCapacity(int size)
    {
        //既にオブジェクトサイズが大きいときは更新しない
        //if (size < Pool.Count) return;

        for (int i = Pool.Count - 1; i < size; ++i)
        {
            T obj = default(T);
            if (Parent)
            {
                obj = GameObject.Instantiate(BaseObj, Parent);
            }
            else
            {
                obj = GameObject.Instantiate(BaseObj);
            }
            Pooling(obj);
        }
    }

    public T Instantiate()
    {
        T ret = null;
        for (int i = 0; i < Pool.Count; ++i)
        {
            int index = (Index + i) % Pool.Count;
            if (Pool[index] == null)
            {
                SetCapacity(1);
            }
            else if (Pool[index].IsActive) continue;

            Pool[index].Create();
            ret = Pool[index];
            break;
        }

        return ret;
    }
}
