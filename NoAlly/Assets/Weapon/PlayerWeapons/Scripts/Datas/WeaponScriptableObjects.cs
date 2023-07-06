using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatas", menuName = "ScriptableObjects/WeaponDatasObject", order = 1)]
public class WeaponScriptableObjects : ScriptableObject
{
    [SerializeField, Tooltip("Še•Ší‚Ìƒf[ƒ^")]
    WeaponDataEntity[] _weaponDatas;

    public WeaponDataEntity[] WeaponDatas => _weaponDatas;
}

[System.Serializable]
public class WeaponDataEntity
{
    [SerializeField, Header("•Ší‚Ìƒ^ƒCƒv")]
    WeaponType _type = WeaponType.NONE;
    [SerializeField, Header("•Ší‚Ì•¨—UŒ‚—Í")]
    float[] _rigitPower = new float[2];
    [SerializeField, Header("•Ší‚Ì‰ŠUŒ‚—Í")]
    float[] _firePower = new float[2];
    [SerializeField, Header("•Ší‚Ì—‹UŒ‚—Í")]
    float[] _elekePower = new float[2];
    [SerializeField, Header("•Ší‚Ì•XŒ‹UŒ‚—Í")]
    float[] _frozenPower = new float[2];
    [SerializeField, Header("—­‚ßUŒ‚‚Ì‚½‚ßŽžŠÔ")]
    float[] _chargeLevels = new float[2] { 1f, 3f };


    public WeaponType Type => _type;
    public float[] RigitPower => _rigitPower;
    public float[] FirePower => _firePower;
    public float[] ElekePower => _elekePower;
    public float[] FrozenPower => _frozenPower;
    public float[] ChargeLevels => _chargeLevels;
}
