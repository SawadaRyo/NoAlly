using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO ��ŃI�u�W�F�N�g�v�[���`���ōĐ�����悤�ɂ���

public class SoundManager : Object,IObjectGenerator
{
    ObjectPool<SoundObject,SoundManager,SoundUsage> _pool =new();
    [Tooltip("�T�E���h��ScriptableObject")]
    SoundScriptable[] _soundDataBase = null;
    [Tooltip("���݃��[�v�����Ă���T�E���h")]
    AudioSource _looping = null;

    public Transform GenerateTrance => throw new System.NotImplementedException();

    public void SetSoundData()
    {

    }

    /// <summary>
    /// �T�E���h�̎w��
    /// </summary>
    /// <param name="type">�T�E���h�̎��</param>
    /// <param name="soundNumber">�T�E���h���o�^����Ă���v�f�ԍ�</param>
    public void CallSound(SoundType type, int soundNumber)
    {
        switch (type)
        {
            case SoundType.BGM:
                if (_looping)
                {
                    
                }
                //SoundPlay(_soundDataBase.Sounds.BGM, soundNumber);
                break;
            case SoundType.SE:
                //SoundPlay(_soundDataBase.Sounds.SE, soundNumber);
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
        AudioSource audioSource = Instantiate(_soundDataBase[1].SoundPlayer);

        audioSource.clip = sound.Clip;
        audioSource.volume = sound.Volume;
        audioSource.loop = sound.IsLoop;
        audioSource.Play();
        _looping = audioSource;

        //���[�v�����Ȃ��ꍇ�I�u�W�F�N�g��j������
        if (!sound.IsLoop)
        {
            //Destroy(audioSource, _soundDataBase.WaitDestorySecond);
            Destroy(audioSource, audioSource.clip.length + 1f);
        }
    }
}