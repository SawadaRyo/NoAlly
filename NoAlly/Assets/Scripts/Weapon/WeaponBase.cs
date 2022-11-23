using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [Tooltip("武器の物理攻撃力")]
    [SerializeField] protected float _rigitPower = 5;
    [Tooltip("武器の雷攻撃力")]
    [SerializeField] protected float _elekePower = 0;
    [Tooltip("武器の炎攻撃力")]
    [SerializeField] protected float _firePower = 0;
    [Tooltip("武器の氷結攻撃力")]
    [SerializeField] protected float _frozenPower = 0;
    [Tooltip("")]
    [SerializeField] Transform _center = default;
    [Tooltip("武器の攻撃判定レイヤー")]
    [SerializeField] protected LayerMask _enemyLayer = ~0;
    [Tooltip("武器のRenderer")]
    [SerializeField] protected Renderer[] _weaponRenderer = default;

    [Tooltip("")]
    bool _attack = false;

    protected Vector3 _harfExtents = new Vector3(0.25f, 1.2f, 0.1f);
    [Tooltip("武器が使用中かどうか")]
    protected bool _operated = false;
    [Tooltip("武器が変形中かどうか")]
    protected bool _isDeformated = false;
    [Tooltip("武器の斬撃エフェクト")]
    protected ParticleSystem _myParticleSystem = default;

    public bool Deformated => _isDeformated;
    public bool Operated { get => _operated; set => _operated = value; }
    public float RigitPower { get => _rigitPower; set => _rigitPower = value; }
    public float ElekePower { get => _elekePower; set => _elekePower = value; }
    public float FirePower { get => _firePower; set => _firePower = value; }
    public float FrozenPower { get => _frozenPower; set => _frozenPower = value; }

    public virtual void Awake()
    {
        //Start関数で呼びたい処理はこの関数に
        RendererActive(false);
        _myParticleSystem = GetComponentInChildren<ParticleSystem>();
    }
    public virtual void Update()
    {
        //Update関数で呼びたい処理はこの関数に
    }
    public virtual void OnApplicationQuit() { }
    public virtual void WeaponMovement() { }
    public virtual void WeaponMovement(Collider target) { }
    public virtual void WeaponMode(ElementType type) { }
    public virtual void RendererActive(bool stats)
    {
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = stats;
        }
    }
    public void AttackOfCloseRenge(bool isAttack)
    {
        if (isAttack)
        {
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
            AttackOfCloseRenge(true);
            yield return null;
        }
        _myParticleSystem.Stop();
    }


    public float ChargePower(TypeOfPower top, float magnification)
    {
        float refPower = 0;
        switch (top)
        {
            case TypeOfPower.RIGIT:
                refPower = _rigitPower;
                break;
            case TypeOfPower.ELEKE:
                refPower = _enemyLayer;
                break;
            case TypeOfPower.FIRE:
                refPower = _firePower;
                break;
            case TypeOfPower.FROZEN:
                refPower = _frozenPower;
                break;
        }
        if (magnification < 1)
        {
            magnification = 1;
        }
        return refPower * magnification;
    }
    public enum TypeOfPower
    {
        RIGIT,
        ELEKE,
        FIRE,
        FROZEN
    }
}


