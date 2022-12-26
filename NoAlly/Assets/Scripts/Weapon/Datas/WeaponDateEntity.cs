using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public struct WeaponDateEntity
{
    [SerializeField,Tooltip("����̃v���n�u")]
    public WeaponBase Weapon;
    [SerializeField,Tooltip("���햼")]
    public WeaponType Type;
    [SerializeField,Tooltip("����̕����U����")]
    public float RigitPower;
    [SerializeField, Tooltip("����̗��U����")]
    public float ElekePower;
    [SerializeField, Tooltip("����̉��U����")]
    public float FirePower;
    [SerializeField, Tooltip("����̕X���U����")]
    public float FrozenPower;
}

[System.Serializable]
public struct Weapons
{
    public WeaponDateEntity[] Entitys;
}
