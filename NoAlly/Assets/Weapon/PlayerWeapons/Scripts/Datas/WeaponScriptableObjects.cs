using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatas", menuName = "ScriptableObjects/WeaponDatasObject", order = 1)]
public class WeaponScriptableObjects : ScriptableObject
{
    [SerializeField, Tooltip("各武器のデータ")]
    WeaponDataEntity[] _weaponDatas;

    public WeaponDataEntity[] WeaponDatas => _weaponDatas;
}

[System.Serializable]
public class WeaponDataEntity
{
    [SerializeField, Header("使用するWeaponBaseの名前")]
    WeaponClassName _weaponBaseName = WeaponClassName.None;
    [SerializeField, Header("武器のタイプ")]
    WeaponType _type = WeaponType.NONE;
    [SerializeField, Header("武器の物理攻撃力")]
    float[] _rigitPower = new float[2];
    [SerializeField, Header("武器の炎攻撃力")]
    float[] _firePower = new float[2];
    [SerializeField, Header("武器の雷攻撃力")]
    float[] _elekePower = new float[2];
    [SerializeField, Header("武器の氷結攻撃力")]
    float[] _frozenPower = new float[2];
    [SerializeField, Header("溜め攻撃のため時間")]
    float[] _chargeLevels = new float[2] { 1f, 3f };
    [SerializeField, Header("変形必要な属性")]
    ElementType _elementType = ElementType.RIGIT;

    [SerializeField, Range(0.1f, 1f), Header("チャージ中の移動速度の倍率")]
    float _magnificationInCharge = 1f;

    public WeaponClassName WeaponBaseName => _weaponBaseName;
    public WeaponType TypeOfWeapon => _type;
    public float[] RigitPower => _rigitPower;
    public float[] FirePower => _firePower;
    public float[] ElekePower => _elekePower;
    public float[] FrozenPower => _frozenPower;
    public float[] ChargeLevels => _chargeLevels;
    public ElementType ElementDeformation => _elementType;
    public float SpeedInCharge => _magnificationInCharge;
}

public enum WeaponClassName
{
    None,
    WeaponCombat,
    WeaponArrow,
    WeaponShield
}

public struct WeaponPower
{
    public float defaultPower;
    public float elementPower;

    static readonly WeaponPower zeroPower = new(0, 0);

    public static WeaponPower zero => zeroPower;

    public WeaponPower(float defaultPower, float elementPower)
    {
        this.defaultPower = defaultPower;
        this.elementPower = elementPower;
    }
}
