using System.Collections.Generic;
using UnityEngine;

//TODO 後でオブジェクトプール形式で再生するようにする

public class SoundManager : Object, IObjectGenerator
{
    [Tooltip("")]
    SoundObjectPool<SoundObject, SoundManager> _pool = new();
    [Tooltip("")]
    SoundObjectPool<SoundObject, SoundManager>.ObjectKey[] _keys;
    [Tooltip("サウンドのScriptableObject")]
    SoundScriptable[] _soundDataBase = null;
    [Tooltip("現在ループさせているサウンド")]
    SoundObject _loopingSoundObj = null;

    SoundUsage _usage;

    public Transform GenerateTrance => throw new System.NotImplementedException();
    public SoundScriptable[] SoundDataBase => _soundDataBase;
    public SoundUsage Usage => _usage;

    public void SetSoundData(SoundScriptable[] datas)
    {
        _soundDataBase = datas;
        for (int i = 0; i < _soundDataBase.Length; i++)
        {
            _usage = (SoundUsage)i;
            _keys[i] = _pool.SetBaseObj(_soundDataBase[i].SoundPlayer, this);
            _pool.SetCapacity(_keys[i], 1);
        }
    }

    /// <summary>
    /// サウンドの指定
    /// </summary>
    /// <param name="type">サウンドの種類</param>
    /// <param name="soundNumber">サウンドが登録されている要素番号</param>
    public void CallSound(SoundUsage usage, SoundType type, int soundNumber)
    {
        _usage = usage;
        switch (type)
        {
            case SoundType.BGM:
                if (_loopingSoundObj)
                {
                    _loopingSoundObj.Disactive();
                }
                SoundPlay(_soundDataBase[(int)_usage].Sounds.BGM, soundNumber);
                break;
            case SoundType.SE:
                SoundPlay(_soundDataBase[(int)_usage].Sounds.SE, soundNumber);
                break;
        }
    }

    /// <summary>
    /// サウンドオブジェクトの生成とサウンドの再生
    /// </summary>
    /// <param name="sounds">サウンドデータ</param>
    /// <param name="soundNumber">サウンドデータにあてられた番号</param>
    void SoundPlay(SoundElements[] sounds, int soundNumber)
    {
        if (soundNumber >= sounds.Length)
        {
            Debug.LogError("指定した番号にデータがありません");
            return;
        }
        //サウンドオブジェクトを生成
        SoundElements sound = sounds[soundNumber];
        SoundObject audioSource = _pool.Instantiate(_keys[(int)_usage], soundNumber);
        _loopingSoundObj = audioSource;

        //ループさせない場合オブジェクトを破棄する
        if (!sound.IsLoop)
        {
            _loopingSoundObj.Disactive(sounds[soundNumber].Clip.length);
        }
    }
}