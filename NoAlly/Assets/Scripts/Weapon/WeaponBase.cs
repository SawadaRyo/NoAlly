using System;
using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField, Tooltip("ïêäÌÇÃï®óùçUåÇóÕ")]
    protected float _rigitPower = 5;
    [SerializeField, Tooltip("ïêäÌÇÃóãçUåÇóÕ")]
    protected float _elekePower = 0;
    [SerializeField, Tooltip("ïêäÌÇÃâäçUåÇóÕ")]
    protected float _firePower = 0;
    [SerializeField,Tooltip("ïêäÌÇÃïXåãçUåÇóÕ")]
    protected float _frozenPower = 0;
    
    [SerializeField, Tooltip("ó≠ÇﬂçUåÇëÊ1íiäK")] 
    protected float _chargeLevel1 = 1f;
    [SerializeField, Tooltip("ó≠ÇﬂçUåÇëÊ2íiäK")] 
    protected float _chargeLevel2 = 3f;

    [SerializeField, Tooltip("ïêäÌÇÃçUåÇîªíËÉåÉCÉÑÅ[")]
    protected LayerMask _enemyLayer = ~0;
    [SerializeField, Tooltip("ïêäÌÇÃRenderer")]
    protected Renderer[] _weaponRenderer = default;
    [SerializeField, Tooltip("ïêäÌÇÃçUåÇîªíËÇÃÉZÉìÉ^Å[")]
    Transform _center = default;

    [Tooltip("ïêäÌÇÃçUåÇîªíËâ”èäÇÃëÂÇ´Ç≥")]
    protected Vector3 _harfExtents = new Vector3(0.25f, 1.2f, 0.1f);
    [Tooltip("ïêäÌÇ™égópíÜÇ©Ç«Ç§Ç©")]
    protected bool _operated = false;
    [Tooltip("ïêäÌÇ™ïœå`íÜÇ©Ç«Ç§Ç©")]
    protected bool _isDeformated = false;
    [Tooltip("ïêäÌÇÃéaåÇÉGÉtÉFÉNÉg")]
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
        //Startä÷êîÇ≈åƒÇ—ÇΩÇ¢èàóùÇÕÇ±ÇÃä÷êîÇ…
        RendererActive(false);
        _myParticleSystem = GetComponentInChildren<ParticleSystem>();
    }
    public virtual void Update()
    {
        //Updateä÷êîÇ≈åƒÇ—ÇΩÇ¢èàóùÇÕÇ±ÇÃä÷êîÇ…
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
            //ToDoÇ±Ç±ÇÃèàóùÇ3DÇ≈ÇÕÇ»Ç≠2DÇ…Ç∑ÇÈ
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


