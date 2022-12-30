using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public struct WeaponDateEntity
{
    [SerializeField,Header("•Ší‚Ì‹@”\")]
    public WeaponBase Base;
    [SerializeField, Header("•Ší‚ÌUŒ‚ƒ‚[ƒVƒ‡ƒ“")]
    public WeaponAction Action;
    [SerializeField, Header("•Ší‚Ìƒ^ƒCƒv")]
    public WeaponType Type;
    [SerializeField, Header("•Ší‚ğ“üè‚µ‚Ä‚¢‚é‚©")]
    public bool IsGetWeapon;
    [SerializeField, Header("•Ší‚Ì•¨—UŒ‚—Í")]
    public float RigitPower;
    [SerializeField, Header("•Ší‚Ì—‹UŒ‚—Í")]
    public float ElekePower;
    [SerializeField, Header("•Ší‚Ì‰ŠUŒ‚—Í")]
    public float FirePower;
    [SerializeField, Header("•Ší‚Ì•XŒ‹UŒ‚—Í")]
    public float FrozenPower;
}

[System.Serializable]
public struct Weapons
{
    public WeaponDateEntity[] Entitys;
}
