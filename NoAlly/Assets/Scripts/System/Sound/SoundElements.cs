using UnityEngine;

/// <summary>
/// サウンドの項目
/// </summary>
[System.Serializable]
public class SoundElements
{
    [SerializeField,Tooltip("サウンドのクリップ")] public AudioClip Clip; 
    [SerializeField,Tooltip("ループするかどうか")] public bool IsLoop; 
    [SerializeField,Range(0f,1f),Tooltip("ループする時間")] public float Volume; 
}

/// <summary>
/// BGMとSEの配列
/// </summary>
[System.Serializable]
public class SoundArray
{
    [SerializeField,Tooltip("BGMデータの配列")] public SoundElements[] BGM; 
    [SerializeField,Tooltip("SEデータの配列")] public SoundElements[] SE;  
}
