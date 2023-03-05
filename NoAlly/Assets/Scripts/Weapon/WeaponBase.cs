using System;
using System.Collections;
using UnityEngine;


public abstract class WeaponBase : MonoBehaviour, IWeaponBase
{
    [SerializeField, Tooltip("����̃I�[�i�[")]
    protected ObjectOwner _owner;

    [Tooltip("����̍U���� �v�f1:����̕����U����,�v�f2:����̉��U����,�v�f3:����̗��U����,�v�f4:����̕X���U����")]
    protected float[] _weaponPower = new float[Enum.GetValues(typeof(ElementType)).Length];
    [Tooltip("���̕���̃f�[�^")]
    protected WeaponDataEntity _weaponData;
    [Tooltip("���킪�ό`�����ǂ���")]
    protected WeaponDeformation _isDeformated = WeaponDeformation.NONE;
    [Tooltip("����̃^�C�v")]
    protected WeaponType _weaponType;
    [Tooltip("����̑���")]
    protected ElementType _elementType;

    public float[] WeaponPower => _weaponPower;
    public ElementType Type => _elementType;
    public WeaponDeformation Deformated => _isDeformated;
    public ObjectOwner Owner => _owner;

    public virtual void Initialize(WeaponDataEntity weaponData)
    {
        _weaponData = weaponData;
        _weaponPower[(int)ElementType.RIGIT] = _weaponData.RigitPower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FIRE] = _weaponData.FirePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.ELEKE] = _weaponData.ElekePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FROZEN] = _weaponData.FrozenPower[(int)_isDeformated];
    }
    public virtual void WeaponMode(ElementType elementType)
    {
        _weaponPower[(int)ElementType.RIGIT] = _weaponData.RigitPower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FIRE] = _weaponData.FirePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.ELEKE] = _weaponData.ElekePower[(int)_isDeformated];
        _weaponPower[(int)ElementType.FROZEN] = _weaponData.FrozenPower[(int)_isDeformated];
        _elementType = elementType;
    }
}


