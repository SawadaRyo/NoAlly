using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "ScriptableObjects/SoundScriptable", order = 1)]
public class SoundScriptable : ScriptableObject
{
    [SerializeField,Header("サウンドの種類")]
    SoundUsage _soundUsage;
    [SerializeField, Header("サウンドを再生するオブジェクト")]
    SoundObject _soundPlayer = null;
    [SerializeField, Header("サウンドの配列")]
    SoundArray _soundAllay;

    public SoundUsage Usage => _soundUsage;
    public SoundObject SoundPlayer => _soundPlayer;
    public SoundArray Sounds => _soundAllay;
}
