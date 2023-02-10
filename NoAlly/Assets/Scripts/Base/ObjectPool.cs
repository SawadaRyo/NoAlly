using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<TObj,TOwner,TKey> 
    where TObj : UnityEngine.Object, IObjectPool
    where TOwner: UnityEngine.Object, IObjectGenerator
    where TKey :Enum
{
    ObjectKey _objectKey = null;
    TObj _baseObj = null;
    
    Transform _parent = null;
    List<TObj> _pool = new List<TObj>();
    Dictionary<ObjectKey, List<TObj>> _poolList = new Dictionary<ObjectKey, List<TObj>>();
    int _index = 0;

    public class ObjectKey
    {
        TOwner _owner = null;
        TKey _key;

        public TOwner Owner => _owner;
        public TKey Key => _key;

        public ObjectKey(TOwner owner,TKey key)
        {
            _owner = owner;
            _key = key;
        }
    }

    public ObjectKey SetBaseObj(TObj obj, TOwner owner,Transform parent, TKey key)
    {
        _baseObj = obj;
        _parent = parent;
        _objectKey = new(owner, key);
        return _objectKey;
    }

    public void SetCapacity(ObjectKey key,int size)
    {
        List<TObj> objList = new List<TObj>();
        for (int i = _pool.Count - 1; i < size; ++i)
        {
            TObj obj = default(TObj);
            if (_parent)
            {
                obj = GameObject.Instantiate(_baseObj, _parent);
            }
            else
            {
                obj = GameObject.Instantiate(_baseObj);
            }
            obj.DisactiveForInstantiate(key.Owner);
            objList.Add(obj);
        }
        Dictionalize(key, objList);
    }

    void Dictionalize(ObjectKey key, List<TObj> value)
    {
        _poolList.Add(key, value);
    }

    public TObj Instantiate()
    {
        TObj ret = null;
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

    public TObj Instantiate(ObjectKey key)
    {
        TObj ret = null;
        List<TObj> valueList = _poolList.GetValueOrDefault(key);
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



