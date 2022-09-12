using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    AudioSource _audioSource = default;
    List<AudioClip> _footSounds = default;
    public AudioManager(AudioSource audioSource, List<AudioClip> footSounds)
    {
        _audioSource = audioSource;
        _footSounds = footSounds;
    }
}
