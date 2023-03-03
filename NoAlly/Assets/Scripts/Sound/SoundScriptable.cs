using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "ScriptableObjects/SoundScriptable", order = 1)]
public class SoundScriptable : ScriptableObject
{
    [SerializeField,Tooltip("サウンドの種類")]
    SoundUsage _soundUsage;
    [SerializeField, Tooltip("サウンドを再生するオブジェクト")]
    SoundObject _soundPlayer = null;
    [SerializeField, Tooltip("サウンドの配列")]
    SoundArray _soundAllay;

    public SoundUsage Usage => _soundUsage;
    public SoundObject SoundPlayer => _soundPlayer;
    public SoundArray Sounds => _soundAllay;
}
