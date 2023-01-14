using System;
using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
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
    [SerializeField, Header("����̍U������̒��S�_")]
    Transform _center = default;
    [SerializeField,Header("����̃I�[�i�[")]
    WeaponOwner _owner = WeaponOwner.Player;

    [Tooltip("����̍U������ӏ��̑傫��")]
    protected Vector3 _harfExtents = new Vector3(0.25f, 1.2f, 0.1f);
    [Tooltip("���킪�ό`�����ǂ���")]
    protected bool _isDeformated = false;
    [Tooltip("����̎a���G�t�F�N�g")]
    protected ParticleSystem _myParticleSystem = default;
    [Tooltip("")]
    bool _attack = false;
    [Tooltip("����̑���")]
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
            //ToDo�����̏�����3D�ł͂Ȃ�2D�ɂ���
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


