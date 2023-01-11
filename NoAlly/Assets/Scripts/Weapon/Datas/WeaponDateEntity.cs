using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public struct WeaponDateEntity
{
    [SerializeField, Header("����̃v���n�u")]
    public ObjectVisual Prefab;
    [SerializeField,Header("����̋@�\")]
    public WeaponBase Base;
    [SerializeField, Header("����̍U�����[�V����")]
    public WeaponAction Action;

    [SerializeField, Header("����̃^�C�v")]
    public WeaponType Type;

    [SerializeField, Header("�������肵�Ă��邩")]
    public bool IsGetWeapon;

    [SerializeField, Header("����̕����U����")]
    public float RigitPower;
    [SerializeField, Header("����̗��U����")]
    public float ElekePower;
    [SerializeField, Header("����̉��U����")]
    public float FirePower;
    [SerializeField, Header("����̕X���U����")]
    public float FrozenPower;

    [SerializeField, Header("���ߍU����1�i�K")]
    public float _chargeLevel1;
    [SerializeField, Header("���ߍU����2�i�K")]
    public float _chargeLevel2;
}

[System.Serializable]
public struct Weapons
{
    public WeaponDateEntity[] Entitys;
}
