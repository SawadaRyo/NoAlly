using System;
using System.Collections;
using UnityEngine;


public abstract class WeaponBase : IWeaponBase
{
    [Tooltip("����̃I�[�i�[")]
    protected ObjectOwner _owner = ObjectOwner.PLAYER;

    [Tooltip("����̍U���� �v�f1:����̕����U����,�v�f2:����̉��U����,�v�f3:����̗��U����,�v�f4:����̕X���U����")]
    protected float[] _weaponPower = new float[Enum.GetValues(typeof(ElementType)).Length];
    [Tooltip("���ߒi�K�B�v�f1:���ߍU����1�i�K,�v�f2:���ߍU����2�i�K")]
    protected float[] _chargeLevels = null;
    [Tooltip("���̕���̃f�[�^")]
    protected WeaponDataEntity _weaponData;
    [Tooltip("���킪�ό`�����ǂ���")]
    protected WeaponDeformation _isDeformated = WeaponDeformation.NONE;
    [Tooltip("����̃^�C�v")]
    protected WeaponType _weaponType;
    [Tooltip("����̑���")]
    protected ElementType _elementType;

    public float[] WeaponPower => _weaponPower;
    public ElementType ElementType => _elementType;
    public WeaponDeformation Deformated => _isDeformated;
    public ObjectOwner Owner => _owner;
    public float[] ChargeLevels => _chargeLevels;

    public virtual void AttackMovement(Collider target) { }

    public WeaponBase(WeaponDataEntity weaponData)
    {
        _weaponData = weaponData;
        _weaponType = _weaponData.Type;
        _weaponPower[(int)ElementType.RIGIT] = _weaponData.RigitPower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FIRE] = _weaponData.FirePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.ELEKE] = _weaponData.ElekePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FROZEN] = _weaponData.FrozenPower[(int)_isDeformated];
        _chargeLevels = _weaponData.ChargeLevels;
    }
    public virtual void WeaponModeToElement(ElementType elementType)
    {
        _weaponPower[(int)ElementType.RIGIT] = _weaponData.RigitPower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FIRE] = _weaponData.FirePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.ELEKE] = _weaponData.ElekePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FROZEN] = _weaponData.FrozenPower[(int)_isDeformated];
        _elementType = elementType;
    }
}


