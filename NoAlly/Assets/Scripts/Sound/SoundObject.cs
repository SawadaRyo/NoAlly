using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundObject : ObjectBase, ISoundObjectPool<SoundManager>
{
    AudioSource _audioSource = null;
    SoundScriptable _soundScriptable = null;

    public SoundManager Owner { get; private set; }

    public void Create(SoundType type, int soundNumber)
    {
        _isActive = true;
        SoundElements soundElements = null;
        switch (type)
        {
            case SoundType.BGM:
                soundElements = _soundScriptable.Sounds.BGM[soundNumber];
                break;
            case SoundType.SE:
                soundElements = _soundScriptable.Sounds.SE[soundNumber];
                break;
        }
        _audioSource.clip = soundElements.Clip;
        _audioSource.volume = soundElements.Volume;
        _audioSource.loop = soundElements.IsLoop;
        _audioSource.Play();

        if (!_audioSource.loop)
        {
            Disactive(soundElements.Clip.length);
        }
    }

    public virtual void Disactive()
    {
        _isActive = false;
        _audioSource.Stop();
    }
    public virtual async void Disactive(float interval)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(interval));
        _isActive = false;
        _audioSource.Stop();
    }

    public void DisactiveForInstantiate(SoundManager owner)
    {
        Owner = owner;
        _soundScriptable = Owner.SoundDataBase[Owner.Usage];
    }
}
