using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public struct WeaponDataEntity
{
    [SerializeField,Header("����̋@�\")]
    public WeaponBase Base;
    [SerializeField, Header("����̍U�����[�V����")]
    public WeaponAction Action;
    [SerializeField, Header("����̃^�C�v")]
    public WeaponType Type;
    [SerializeField, Header("����̕����U����")]
    public float[] RigitPower;
    [SerializeField, Header("����̗��U����")]
    public float[] ElekePower;
    [SerializeField, Header("����̉��U����")]
    public float[] FirePower;
    [SerializeField, Header("����̕X���U����")]
    public float[] FrozenPower;
    [SerializeField, Header("���ߍU����1�i�K")]
    public float _chargeLevel1;
    [SerializeField, Header("���ߍU����2�i�K")]
    public float _chargeLevel2;
}

[System.Serializable]
public struct Weapons
{
    public WeaponDataEntity[] Entitys;
}
