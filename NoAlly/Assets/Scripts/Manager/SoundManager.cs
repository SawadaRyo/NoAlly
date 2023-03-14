using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO ��ŃI�u�W�F�N�g�v�[���`���ōĐ�����悤�ɂ���

public class SoundManager : Object, IObjectGenerator
{
    [Tooltip("")]
    SoundObjectPool<SoundObject, SoundManager> _pool = new();
    [Tooltip("")]
    SoundObjectPool<SoundObject, SoundManager>.ObjectKey[] _keys;
    [Tooltip("�T�E���h��ScriptableObject")]
    Dictionary<SoundUsage, SoundScriptable> _soundDataBase = new Dictionary<SoundUsage, SoundScriptable>();
    [Tooltip("���݃��[�v�����Ă���T�E���h")]
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
    /// �T�E���h�̎w��
    /// </summary>
    /// <param name="type">�T�E���h�̎��</param>
    /// <param name="soundNumber">�T�E���h���o�^����Ă���v�f�ԍ�</param>
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