using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Tooltip("ScriptableObject")]
    SoundScriptable _soundDataBase = null;
    [Tooltip("BGM")]
    GameObject _loopingBGM = null;
    /// <summary>
    /// _soundDataBase??v???p?e?B
    /// </summary>
    public SoundScriptable SoundDataBase { get => _soundDataBase; set => _soundDataBase = value; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type">?T?E???h????</param>
    /// <param name="soundNumber">SoundScriptable????????????T?E???h??v?f???
    /// ???T?E???h??SoundScriptable??????????</param>
    public void CallSound(SoundType type, int soundNumber)
    {
        switch (type)
        {
            case SoundType.BGM:
                if (_loopingBGM)
                {
                    Destroy(_loopingBGM);
                }
                SoundPlay(_soundDataBase.Sounds.BGM, soundNumber);
                break;
            case SoundType.SE:
                SoundPlay(_soundDataBase.Sounds.SE, soundNumber);
                break;
        }
    }

    /// <summary>
    /// ?T?E???h?????I?u?W?F?N?g??????????
    /// </summary>
    /// <param name="sounds">?T?E???h??v?f???w?????\????</param>
    /// <param name="soundNumber">?T?E???h??v?f???</param>
    void SoundPlay(SoundElements[] sounds, int soundNumber)
    {
        if (soundNumber >= sounds.Length)
        {
            Debug.LogError("?T?E???h??????????");
            return;
        }
        //?I?u?W?F?N?g????????????
        SoundElements sound = sounds[soundNumber];
        GameObject soundPlayer = Instantiate(_soundDataBase.SoundPlayer);
        AudioSource audioSource = soundPlayer.GetComponent<AudioSource>();

        audioSource.clip = sound.Clip;
        audioSource.volume = sound.Volume;
        audioSource.loop = sound.IsLoop;
        audioSource.Play();
        _loopingBGM = soundPlayer;

        //???[?v????????A?w?????b??????
        if (!sound.IsLoop)
        {
            Destroy(audioSource, _soundDataBase.WaitDestorySecond);
        }
    }
}
