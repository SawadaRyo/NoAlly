using System;
using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField, Header("•Ší‚Ì•¨—UŒ‚—Í")]
    protected float _rigitPower = 5;
    [SerializeField, Header("•Ší‚Ì—‹UŒ‚—Í")]
    protected float _elekePower = 0;
    [SerializeField, Header("•Ší‚Ì‰ŠUŒ‚—Í")]
    protected float _firePower = 0;
    [SerializeField, Header("•Ší‚Ì•XŒ‹UŒ‚—Í")]
    protected float _frozenPower = 0;
    [SerializeField, Header("—­‚ßUŒ‚‘æ1’iŠK")] 
    protected float _chargeLevel1 = 1f;
    [SerializeField, Header("—­‚ßUŒ‚‘æ2’iŠK")] 
    protected float _chargeLevel2 = 3f;
    [SerializeField, Header("•Ší‚ÌUŒ‚”»’èƒŒƒCƒ„[")]
    protected LayerMask _enemyLayer = ~0;
    [SerializeField, Header("•Ší‚ÌUŒ‚”»’è‚Ì’†S“_")]
    Transform _center = default;
    [SerializeField,Header("•Ší‚ÌƒI[ƒi[")]
    WeaponOwner _owner = WeaponOwner.Player;

    [Tooltip("•Ší‚ÌUŒ‚”»’è‰ÓŠ‚Ì‘å‚«‚³")]
    protected Vector3 _harfExtents = new Vector3(0.25f, 1.2f, 0.1f);
    [Tooltip("•Ší‚ª•ÏŒ`’†‚©‚Ç‚¤‚©")]
    protected bool _isDeformated = false;
    [Tooltip("•Ší‚ÌŽaŒ‚ƒGƒtƒFƒNƒg")]
    protected ParticleSystem _myParticleSystem = default;
    [Tooltip("")]
    bool _attack = false;
    [Tooltip("•Ší‚Ì‘®«")]
    ElementType _type;

    public bool Deformated => _isDeformated;
    public float RigitPower { get => _rigitPower; set => _rigitPower = value; }
    public float ElekePower { get => _elekePower; set => _elekePower = value; }
    public float FirePower { get => _firePower; set => _firePower = value; }
    public float FrozenPower { get => _frozenPower; set => _frozenPower = value; }

    public virtual void Initialize(WeaponDataEntity weaponData)
    {
        _rigitPower = weaponData.RigitPower;
        _firePower = weaponData.FirePower;
        _elekePower = weaponData.ElekePower;
        _frozenPower = weaponData.FrozenPower;
        _myParticleSystem = GetComponentInChildren<ParticleSystem>();
    }
    public virtual void WeaponAttackMovement() { }
    public virtual void WeaponAttackMovement(Collider target) { }
    public virtual void WeaponMode(ElementType type) 
    {
        _type = type;
    }
    public void AttackOfRenge(bool isAttack)
    {
        if (isAttack)
        {
            //ToDo‚±‚±‚Ìˆ—‚ð3D‚Å‚Í‚È‚­2D‚É‚·‚é
            Collider[] objectsInRenge = Physics.OverlapBox(_center.position, _harfExtents, Quaternion.identity, _enemyLayer);
            if (objectsInRenge.Length > 0)
            {
                foreach (Collider obj in objectsInRenge)
                {
                    if (obj.TryGetComponent<IHitBehavorOfAttack>(out IHitBehavorOfAttack enemyHp))
                    {
                        enemyHp.BehaviorOfHit(this,_type,_owner);
                    }
                    else if(obj.TryGetComponent<IHitBehavorOfGimic>(out IHitBehavorOfGimic hitObj))
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


