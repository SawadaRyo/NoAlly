using System;
using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField, Tooltip("����̕����U����")]
    protected float _rigitPower = 5;
    [SerializeField, Tooltip("����̗��U����")]
    protected float _elekePower = 0;
    [SerializeField, Tooltip("����̉��U����")]
    protected float _firePower = 0;
    [SerializeField,Tooltip("����̕X���U����")]
    protected float _frozenPower = 0;
    
    [SerializeField, Tooltip("���ߍU����1�i�K")] 
    protected float _chargeLevel1 = 1f;
    [SerializeField, Tooltip("���ߍU����2�i�K")] 
    protected float _chargeLevel2 = 3f;

    [SerializeField, Tooltip("����̍U�����背�C���[")]
    protected LayerMask _enemyLayer = ~0;
    [SerializeField, Tooltip("�����Renderer")]
    protected Renderer[] _weaponRenderer = default;
    [SerializeField, Tooltip("����̍U������̃Z���^�[")]
    Transform _center = default;

    [Tooltip("����̍U������ӏ��̑傫��")]
    protected Vector3 _harfExtents = new Vector3(0.25f, 1.2f, 0.1f);
    [Tooltip("���킪�g�p�����ǂ���")]
    protected bool _operated = false;
    [Tooltip("���킪�ό`�����ǂ���")]
    protected bool _isDeformated = false;
    [Tooltip("����̎a���G�t�F�N�g")]
    protected ParticleSystem _myParticleSystem = default;
    [Tooltip("")]
    bool _attack = false;

    public bool Deformated => _isDeformated;
    public bool Operated { get => _operated; set => _operated = value; }
    public float RigitPower { get => _rigitPower; set => _rigitPower = value; }
    public float ElekePower { get => _elekePower; set => _elekePower = value; }
    public float FirePower { get => _firePower; set => _firePower = value; }
    public float FrozenPower { get => _frozenPower; set => _frozenPower = value; }

    public virtual void Start()
    {
        //Start�֐��ŌĂт��������͂��̊֐���
        RendererActive(false);
        _myParticleSystem = GetComponentInChildren<ParticleSystem>();
    }
    public virtual void Update()
    {
        //Update�֐��ŌĂт��������͂��̊֐���
    }
    public virtual void WeaponAttackMovement() { }
    public virtual void WeaponAttackMovement(Collider target) { }
    public virtual void WeaponMode(ElementType type) { }
    public virtual void RendererActive(bool stats)
    {
        Array.ForEach(_weaponRenderer, x => x.enabled = stats);
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
                        enemyHp.BehaviorOfHit(this,MainMenu.Instance.Type);
                    }
                    else if(obj.TryGetComponent<IHitBehavorOfGimic>(out IHitBehavorOfGimic hitObj))
                    {
                        hitObj.BehaviorOfHit(MainMenu.Instance.Type);
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


