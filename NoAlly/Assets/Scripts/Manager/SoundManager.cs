using System.Collections.Generic;
using UnityEngine;

//TODO 後でオブジェクトプール形式で再生するようにする

public class SoundManager : Object, IObjectGenerator
{
    ObjectPool<SoundObject, SoundManager> _pool = new();
    ObjectPool<SoundObject, SoundManager>.ObjectKey _keys;
    [Tooltip("サウンドのScriptableObject")]
    SoundScriptable _soundDataBase = null;
    [Tooltip("現在ループさせているサウンド")]
    SoundObject _loopingBGMObj = null;

    SoundScriptable SoundDataBase => _soundDataBase;
    public Transform GenerateTrance => throw new System.NotImplementedException();

    public void SetSoundData(SoundScriptable datas)
    {
        _soundDataBase = datas;
        _keys = _pool.SetBaseObj(_soundDataBase.SoundPlayer, this);
        _pool.SetCapacity(_keys, 1);

    }

    /// <summary>
    /// サウンドの指定
    /// </summary>
    /// <param name="type">サウンドの種類</param>
    /// <param name="soundNumber">サウンドが登録されている要素番号</param>
    public void CallSound(SoundUsage usage, SoundType type, int soundNumber)
    {
        switch (type)
        {
            case SoundType.BGM:
                if (_loopingBGMObj)
                {
                    _loopingBGMObj.Disactive();
                }
                SoundPlay(_soundDataBase.Sounds.BGM, soundNumber);
                break;
            case SoundType.SE:
                //SoundPlay(_soundDataBase.Sounds.SE, soundNumber);
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
        SoundObject audioSource = _pool.Instantiate(_keys);
        //AudioSource audioSource = Instantiate(_soundDataBase[1].SoundPlayer);

        //audioSource.clip = sound.Clip;
        //audioSource.volume = sound.Volume;
        //audioSource.loop = sound.IsLoop;
        //audioSource.Play();
        _loopingBGMObj = audioSource;

        //ループさせない場合オブジェクトを破棄する
        if (!sound.IsLoop)
        {
            //Destroy(audioSource, _soundDataBase.WaitDestorySecond);
            //Destroy(audioSource, audioSource.clip.length + 1f);
            _loopingBGMObj.Disactive();
        }
    }
}