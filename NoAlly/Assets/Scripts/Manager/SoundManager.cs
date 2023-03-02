using System.Collections.Generic;
using UnityEngine;

//TODO ��ŃI�u�W�F�N�g�v�[���`���ōĐ�����悤�ɂ���

public class SoundManager : Object, IObjectGenerator
{
    [Tooltip("")]
    SoundObjectPool<SoundObject, SoundManager> _pool = new();
    [Tooltip("")]
    SoundObjectPool<SoundObject, SoundManager>.ObjectKey[] _keys;
    [Tooltip("�T�E���h��ScriptableObject")]
    SoundScriptable[] _soundDataBase = null;
    [Tooltip("���݃��[�v�����Ă���T�E���h")]
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
    /// �T�E���h�̎w��
    /// </summary>
    /// <param name="type">�T�E���h�̎��</param>
    /// <param name="soundNumber">�T�E���h���o�^����Ă���v�f�ԍ�</param>
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
    /// �T�E���h�I�u�W�F�N�g�̐����ƃT�E���h�̍Đ�
    /// </summary>
    /// <param name="sounds">�T�E���h�f�[�^</param>
    /// <param name="soundNumber">�T�E���h�f�[�^�ɂ��Ă�ꂽ�ԍ�</param>
    void SoundPlay(SoundElements[] sounds, int soundNumber)
    {
        if (soundNumber >= sounds.Length)
        {
            Debug.LogError("�w�肵���ԍ��Ƀf�[�^������܂���");
            return;
        }
        //�T�E���h�I�u�W�F�N�g�𐶐�
        SoundElements sound = sounds[soundNumber];
        SoundObject audioSource = _pool.Instantiate(_keys[(int)_usage], soundNumber);
        _loopingSoundObj = audioSource;

        //���[�v�����Ȃ��ꍇ�I�u�W�F�N�g��j������
        if (!sound.IsLoop)
        {
            _loopingSoundObj.Disactive(sounds[soundNumber].Clip.length);
        }
    }
}