using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundObjectPool<TObj, TOwner>
    where TObj : Object, ISoundObjectPool<TOwner>
    where TOwner : Object, IObjectGenerator
{
    ObjectKey _objectKey = null;
    TObj _baseObj = null;

    Transform _parent = null;
    readonly List<TObj> _pool = new List<TObj>();
    readonly Dictionary<ObjectKey, List<TObj>> _poolList = new();
    //readonly int _index = 0;

    public class ObjectKey
    {
        TOwner _owner = null;

        public TOwner Owner => _owner;

        public ObjectKey(TOwner owner)
        {
            _owner = owner;
        }
    }

    public ObjectKey SetBaseObj(TObj obj, TOwner owner)
    {
        _baseObj = obj;
        _objectKey = new(owner);
        return _objectKey;
    }

    public ObjectKey SetBaseObj(TObj obj, TOwner owner, Transform parent)
    {
        _baseObj = obj;
        _parent = parent;
        _objectKey = new(owner);
        return _objectKey;
    }

    public void SetCapacity(ObjectKey key, int size)
    {
        List<TObj> objList = new();
        int currentCount = _pool.Count;
        for (int i = _pool.Count; i < currentCount + size; ++i)
        {
            TObj obj;
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

        if (!_poolList.ContainsKey(key))
        {
            Dictionalize(key, objList);
        }
        else
        {
            _poolList[key].AddRange(objList);
        }
    }

    void Dictionalize(ObjectKey key, List<TObj> value)
    {
        _poolList.Add(key, value);
    }

    public TObj Instantiate(ObjectKey key,int soundNumber)
    {
        TObj ret = null;
        List<TObj> valueList = _poolList.GetValueOrDefault(key);
        for (int i = 0; i < valueList.Count; ++i)
        {
            int newSize = 0;
            int index = i % valueList.Count;
            if (valueList[index] == null)
            {
                newSize = (index - valueList.Count) - 1;
                SetCapacity(key, newSize);
                continue;
            }
            else if (valueList[index] != null && valueList[index].IsActive) continue;

            valueList[index].Create(soundNumber);
            ret = valueList[index];
            break;
        }

        return ret;
    }
}