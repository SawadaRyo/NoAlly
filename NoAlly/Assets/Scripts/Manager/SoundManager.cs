using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO 後でオブジェクトプール形式で再生するようにする

public class SoundManager : Object, IObjectGenerator
{
    [Tooltip("")]
    SoundObjectPool<SoundObject, SoundManager> _pool = new();
    [Tooltip("")]
    SoundObjectPool<SoundObject, SoundManager>.ObjectKey[] _keys;
    [Tooltip("サウンドのScriptableObject")]
    Dictionary<SoundUsage, SoundScriptable> _soundDataBase = new Dictionary<SoundUsage, SoundScriptable>();
    [Tooltip("現在ループさせているサウンド")]
    SoundObject _loopingSoundObj = null;

    SoundUsage _usage;

    public Dictionary<SoundUsage, SoundScriptable> SoundDataBase { get; private set; }
    public SoundUsage Usage => _usage;

    public void SetSoundData(SoundScriptable[] datas)
    {
        datas.OrderBy(x => x.Usage).ToArray();
        for (int i = 0; i < datas.Length; i++)
        {
            _usage = (SoundUsage)i;
            _keys[i] = _pool.SetBaseObj(datas[i].SoundPlayer, this);
            _pool.SetCapacity(_keys[i], 10);
            _soundDataBase.Add(_usage, datas[i]);
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
        soundObject = _pool.Instantiate(_keys[(int)_usage], type, soundNumber);
        _loopingSoundObj = soundObject;
    }
}