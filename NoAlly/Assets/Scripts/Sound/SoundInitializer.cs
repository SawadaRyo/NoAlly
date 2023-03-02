using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInitializer : InitializerBase
{
    [SerializeField]
    int bgmNum = 0;
    [SerializeField, Tooltip("ScriptableObject")]
    SoundScriptable[] _soundScriptableObj = null;

    public override void Init()
    {
        GameManager.InstanceSM = new SoundManager();
        GameManager.InstanceSM.SetSoundData(_soundScriptableObj);
        GameManager.InstanceSM.CallSound(SoundUsage.GAMEBGM,SoundType.BGM, bgmNum);
        Destroy(gameObject);
    }
}