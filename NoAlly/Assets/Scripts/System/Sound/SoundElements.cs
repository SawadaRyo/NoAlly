using UnityEngine;

/// <summary>
/// サウンドの項目
/// </summary>
[System.Serializable]
public class SoundElements
{
    [SerializeField,Header("サウンドのクリップ")] public AudioClip Clip; 
    [SerializeField,Header("ループするかどうか")] public bool IsLoop; 
    [SerializeField,Range(0f,1f),Header("ループする時間")] public float Volume; 
}

/// <summary>
/// BGMとSEの配列
/// </summary>
[System.Serializable]
public class SoundArray
{
    [SerializeField,Header("BGMデータの配列")] public SoundElements[] BGM; 
    [SerializeField,Header("SEデータの配列")] public SoundElements[] SE;  
}
