using System;
using System.Collections;
using UnityEngine;

public abstract class WeaponBase : ObjectVisual, IWeapon
{
    [SerializeField, Header("武器の物理攻撃力")]
    protected float _rigitPower = 5;
    [SerializeField, Header("武器の雷攻撃力")]
    protected float _elekePower = 0;
    [SerializeField, Header("武器の炎攻撃力")]
    protected float _firePower = 0;
    [SerializeField, Header("武器の氷結攻撃力")]
    protected float _frozenPower = 0;
    [SerializeField, Header("溜め攻撃第1段階")]
    protected float _chargeLevel1 = 1f;
    [SerializeField, Header("溜め攻撃第2段階")]
    protected float _chargeLevel2 = 3f;
    [SerializeField, Header("武器の攻撃判定レイヤー")]
    protected LayerMask _enemyLayer = ~0;
    [SerializeField, Header("武器のオーナー")]
    protected HitOwner _owner = HitOwner.Player;
    [SerializeField, Header("武器の攻撃判定の中心点")]
    Transform _center = default;
    [SerializeField,Header("武器の斬撃エフェクト")]
    protected ParticleSystem _myParticleSystem = default;

    [Tooltip("武器の攻撃判定箇所の大きさ")]
    protected Vector3 _harfExtents = new Vector3(0.25f, 1.2f, 0.1f);
    [Tooltip("この武器のデータ")]
    protected WeaponDataEntity _weaponData;
    [Tooltip("武器が変形中かどうか")]
    protected WeaponDeformation _isDeformated = WeaponDeformation.None;
    [Tooltip("")]
    bool _attack = false;
    [Tooltip("武器の属性")]
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
    public virtual void WeaponAttackMovement() { }
    public virtual void WeaponAttackMovement(Collider target) { }
    public virtual void WeaponMode(ElementType type)
    {
        _rigitPower = _weaponData.RigitPower[(int)_isDeformated];
        _firePower = _weaponData.FirePower[(int)_isDeformated];
        _elekePower = _weaponData.ElekePower[(int)_isDeformated];
        _frozenPower = _weaponData.FrozenPower[(int)_isDeformated];
        _type = type;

    }
    public void AttackOfRenge(bool isAttack)
    {
        if (isAttack)
        {
            //ToDoここの処理を3Dではなく2Dにする
            Collider[] objectsInRenge = Physics.OverlapBox(_center.position, _harfExtents, Quaternion.identity, _enemyLayer);
            if (objectsInRenge.Length > 0)
            {
                foreach (Collider obj in objectsInRenge)
                {
                    if (obj.TryGetComponent<IHitBehavorOfAttack>(out IHitBehavorOfAttack enemyHp))
                    {
                        enemyHp.BehaviorOfHit(this, _type, _owner);
                    }
                    else if (obj.TryGetComponent<IHitBehavorOfGimic>(out IHitBehavorOfGimic hitObj))
                    {
                        hitObj.BehaviorOfHit(_type);
                    }
                }
            }
        }
    }
    public IEnumerator LoopJud(bool isAttack)
    {
        _attack = isAttack;
        _myParticleSystem.Play();
        while (_attack)
        {
            AttackOfRenge(_attack);
            yield return null;
        }
        _myParticleSystem.Stop();
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


