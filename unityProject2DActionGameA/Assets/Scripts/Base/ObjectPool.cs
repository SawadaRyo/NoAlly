using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : UnityEngine.Object, IObjectPool
{
    T _baseObj = null;
    int _key = 0;
    Transform _parent = null;
    List<T> _pool = new List<T>();
    Dictionary<int, List<T>> _poolList = new Dictionary<int, List<T>>();
    int _index = 0;

    public void SetBaseObj(T obj, Transform parent, int key)
    {
        _baseObj = obj;
        _parent = parent;
        _key = key;
    }

    public void Pooling(T obj)
    {
        obj.DisactiveForInstantiate();
        _pool.Add(obj);
    }

    public void SetCapacity(int size)
    {
        List<T> objList = default(List<T>);
        for (int i = _pool.Count - 1; i < size; ++i)
        {
            T obj = default(T);
            if (_parent)
            {
                obj = GameObject.Instantiate(_baseObj, _parent);
            }
            else
            {
                obj = GameObject.Instantiate(_baseObj);
            }
            obj.DisactiveForInstantiate();
            objList.Add(obj);
            Dictionalize(_key, objList);

            //Pooling(obj);
        }
    }

    public void Dictionalize(int key, List<T> value)
    {
        _poolList.Add(key, value);
    }

    public T Instantiate()
    {
        T ret = null;
        for (int i = 0; i < _pool.Count; ++i)
        {
            int newSIze = 0;
            int index = (_index + i) % _pool.Count;
            if (_pool[index] == null)
            {
                newSIze++;
                continue;
            }
            else if (_pool[index] != null && _pool[index].IsActive) continue;

            _pool[index].Create();
            ret = _pool[index];
            break;
        }

        return ret;
    }

    public T Instantiate(int key)
    {
        T ret = null;
        List<T> valueList = _poolList[key];
        for (int i = 0; i < valueList.Count; ++i)
        {
            int newSIze = 0;
            int index = (_index + i) % valueList.Count;
            if (valueList[index] == null)
            {
                newSIze++;
                continue;
            }
            else if (valueList[index] != null && valueList[index].IsActive) continue;

            valueList[index].Create();
            ret = valueList[index];
            break;
        }

        return ret;
    }
}
