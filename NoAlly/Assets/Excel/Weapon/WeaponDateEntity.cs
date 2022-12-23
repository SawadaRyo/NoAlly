using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public struct WeaponDateEntity
{
    [SerializeField,Tooltip("•Ší‚ÌƒvƒŒƒnƒu")]
    public WeaponBase Weapon;
    [SerializeField,Tooltip("•Ší–¼")]
    public WeaponType Type;
    [SerializeField,Tooltip("•Ší‚Ì•¨—UŒ‚—Í")]
    public float RigitPower;
    [SerializeField, Tooltip("•Ší‚Ì—‹UŒ‚—Í")]
    public float ElekePower;
    [SerializeField, Tooltip("•Ší‚Ì‰ŠUŒ‚—Í")]
    public float FirePower;
    [SerializeField, Tooltip("•Ší‚Ì•XŒ‹UŒ‚—Í")]
    public float FrozenPower;
}

[System.Serializable]
public struct Weapons
{
    public WeaponDateEntity[] Entitys;
}
