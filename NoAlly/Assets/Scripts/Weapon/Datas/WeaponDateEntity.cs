using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[System.Serializable]
public struct WeaponDateEntity
{
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
}

[System.Serializable]
public struct Weapons
{
    public WeaponDateEntity[] Entitys;
}
