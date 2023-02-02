using System;
using System.Collections;
using UnityEngine;

public abstract class WeaponBase : ObjectVisual, IWeapon
{
    [SerializeField, Header("����̕����U����")]
    protected float _rigitPower = 5;
    [SerializeField, Header("����̗��U����")]
    protected float _elekePower = 0;
    [SerializeField, Header("����̉��U����")]
    protected float _firePower = 0;
    [SerializeField, Header("����̕X���U����")]
    protected float _frozenPower = 0;
    [SerializeField, Header("���ߍU����1�i�K")]
    protected float _chargeLevel1 = 1f;
    [SerializeField, Header("���ߍU����2�i�K")]
    protected float _chargeLevel2 = 3f;
    [SerializeField, Header("����̍U�����背�C���[")]
    protected LayerMask _enemyLayer = ~0;
    [SerializeField, Header("����̃I�[�i�[")]
    protected HitOwner _owner = HitOwner.Player;
    [SerializeField, Header("����̍U������̒��S�_")]
    Transform _center = default;
    [SerializeField, Header("����̎a���G�t�F�N�g")]
    protected ParticleSystem _myParticleSystem = default;

    [Tooltip("���̕���̃f�[�^")]
    protected WeaponDataEntity _weaponData;
    [Tooltip("���킪�ό`�����ǂ���")]
    protected WeaponDeformation _isDeformated = WeaponDeformation.NONE;
    [Tooltip("")]
    bool _attack = false;
    [Tooltip("����̑���")]
    ElementType _type;

    public WeaponDeformation Deformated => _isDeformated;
    public float RigitPower { get => _rigitPower; set => _rigitPower = value; }
    public float ElekePower { get => _elekePower; set => _elekePower = value; }
    public float FirePower { get => _firePower; set => _firePower = value; }
    public float FrozenPower { get => _frozenPower; set => _frozenPower = value; }

    public virtual void Initialize(WeaponDataEntity weaponData)
    {
        _weaponData = weaponData;
        _rigitPower = weaponData.RigitPower[(int)_isDeformated];
        _firePower = weaponData.FirePower[(int)_isDeformated];
        _elekePower = weaponData.ElekePower[(int)_isDeformated];
        _frozenPower = weaponData.FrozenPower[(int)_isDeformated];
    }
    public virtual void WeaponAttackMovement(Collider target)
    {
        if (target.TryGetComponent(out IHitBehavorOfAttack characterHp))
        {
            characterHp.BehaviorOfHit(this, _type, _owner);
        }
        else if (target.TryGetComponent(out IHitBehavorOfGimic hitObj))
        {
            hitObj.BehaviorOfHit(_type);
        }
    }
    public virtual void WeaponMode(ElementType type)
    {
        _rigitPower = _weaponData.RigitPower[(int)_isDeformated];
        _firePower = _weaponData.FirePower[(int)_isDeformated];
        _elekePower = _weaponData.ElekePower[(int)_isDeformated];
        _frozenPower = _weaponData.FrozenPower[(int)_isDeformated];
        _type = type;

    }
    void OnTriggerEnter(Collider other)
    {
        WeaponAttackMovement(other);
    }
    public void LoopJud(bool isAttack)
    {
        if(isAttack)
        {
            _myParticleSystem.Play();
        }
        else
        {
            _myParticleSystem.Stop();
        }
        Array.ForEach(_objectCollider, x => x.enabled = isAttack);
    }
    public float ChargePower(ElementType top, float magnification)
    {
        float refPower = 0;
        switch (top)
        {
            case ElementType.RIGIT:
                refPower = _rigitPower;
                break;
            case ElementType.ELEKE:
                refPower = _enemyLayer;
                break;
            case ElementType.FIRE:
                refPower = _firePower;
                break;
            case ElementType.FROZEN:
                refPower = _frozenPower;
                break;
        }
        if (magnification < 1)
        {
            magnification = 1;
        }
        return refPower * magnification;
    }
}


