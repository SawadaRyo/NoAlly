using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    AudioSource _source = default;

    public SoundManager()
    {

    }
}

public class SoundData
{
    string _key = "";
    string _soundName = "";
    AudioClip _clip = null;

    public SoundData(string key, string soundName)
    {
        _key = key;
        _soundName = soundName;
        _clip = Resources.Load(_soundName) as AudioClip;
    }
}

