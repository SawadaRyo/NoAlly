using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [Tooltip("ïêäÌÇÃï®óùçUåÇóÕ")]
    [SerializeField] protected float _rigitPower = 5;
    [Tooltip("ïêäÌÇÃóãçUåÇóÕ")]
    [SerializeField] protected float _elekePower = 0;
    [Tooltip("ïêäÌÇÃâäçUåÇóÕ")]
    [SerializeField] protected float _firePower = 0;
    [Tooltip("ïêäÌÇÃïXåãçUåÇóÕ")]
    [SerializeField] protected float _frozenPower = 0;
    [Tooltip("")]
    [SerializeField] Transform _center = default;
    [Tooltip("ïêäÌÇÃçUåÇîªíËÉåÉCÉÑÅ[")]
    [SerializeField] protected LayerMask _enemyLayer = ~0;
    [Tooltip("ïêäÌÇÃRenderer")]
    [SerializeField] protected Renderer[] _weaponRenderer = default;

    [Tooltip("")]
    bool _attack = false;

    protected Vector3 _harfExtents = new Vector3(0.25f, 1.2f, 0.1f);
    [Tooltip("ïêäÌÇ™égópíÜÇ©Ç«Ç§Ç©")]
    protected bool _operated = false;
    [Tooltip("ïêäÌÇ™ïœå`íÜÇ©Ç«Ç§Ç©")]
    protected bool _isDeformated = false;
    [Tooltip("ïêäÌÇÃéaåÇÉGÉtÉFÉNÉg")]
    protected ParticleSystem _myParticleSystem = default;

    public bool Deformated => _isDeformated;
    public bool Operated { get => _operated; set => _operated = value; }
    public float RigitPower { get => _rigitPower; set => _rigitPower = value; }
    public float ElekePower { get => _elekePower; set => _elekePower = value; }
    public float FirePower { get => _firePower; set => _firePower = value; }
    public float FrozenPower { get => _frozenPower; set => _frozenPower = value; }

    public virtual void Awake()
    {
        //Startä÷êîÇ≈åƒÇ—ÇΩÇ¢èàóùÇÕÇ±ÇÃä÷êîÇ…
        RendererActive(false);
        _myParticleSystem = GetComponentInChildren<ParticleSystem>();
    }
    public virtual void Update()
    {
        //Updateä÷êîÇ≈åƒÇ—ÇΩÇ¢èàóùÇÕÇ±ÇÃä÷êîÇ…
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
                    if (obj.TryGetComponent<EnemyStatus>(out EnemyStatus enemyHp))
                    {
                        enemyHp.DamageMethod(_rigitPower, _firePower, _elekePower, _frozenPower,MainMenu.Instance.Type);
                    }
                    else if(obj.TryGetComponent<IHit>(out IHit hitObj))
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


