using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO 後でオブジェクトプール形式で再生するようにする

public class SoundManager : IObjectGenerator
{
    [Tooltip("")]
    SoundObjectPool<SoundObject, SoundManager,SoundUsage> _pool = new();
    [Tooltip("")]
    Dictionary<SoundUsage,SoundObjectPool<SoundObject, SoundManager,SoundUsage>.ObjectKey> _keys = new();
    [Tooltip("サウンドのScriptableObject")]
    Dictionary<SoundUsage, SoundScriptable> _soundDataBase = new();
    [Tooltip("現在ループさせているサウンド")]
    SoundObject _loopingSoundObj = null;

    SoundUsage _usage;

    public Dictionary<SoundUsage, SoundScriptable> SoundDataBase => _soundDataBase;
    public SoundUsage Usage => _usage;

    public void SetSoundData(SoundScriptable[] datas)
    {
        _soundDataBase = datas.OrderBy(x => x.Usage).ToDictionary(key => key.Usage, x => x);

        for(int index = 0; index < Enum.GetValues(typeof(SoundUsage)).Length; index++)
        {
            _usage = (SoundUsage)index;
            if (_soundDataBase.TryGetValue(_usage, out SoundScriptable soundData))
            {
                _keys.Add(_usage,_pool.SetBaseObj(_soundDataBase[_usage].SoundPlayer, this));
                _pool.SetCapacity(_keys[_usage], 10);
            }
        }
    }

    /// <summary>
    /// サウンドの指定
    /// </summary>
    /// <param name="type">サウンドの種類</param>
    /// <param name="soundNumber">サウンドが登録されている要素番号</param>
    public void CallSound(SoundUsage usage, SoundType type, int soundNumber)
    {
        SoundObject soundObject;
        _usage = usage;
        switch (type)
        {
            case SoundType.BGM:
                if (_loopingSoundObj)
                {
                    _loopingSoundObj.Disactive();
                }
                break;
            case SoundType.SE:
                break;
        }
        soundObject = _pool.Instantiate(_keys[_usage], type, soundNumber);
        _loopingSoundObj = soundObject;
    }
}