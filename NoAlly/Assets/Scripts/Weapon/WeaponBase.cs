using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] protected float _rigitPower = 5;
    [SerializeField] protected float _elekePower = 0;
    [SerializeField] protected float _firePower = 0;
    [SerializeField] protected float _frozenPower = 0;
    [SerializeField] protected LayerMask _enemyLayer = ~0;
    [SerializeField] protected Renderer[] _weaponRenderer = default;

    protected bool _operation = false;
    protected Collider _myCollider = default;
    protected ParticleSystem _myParticleSystem = default;

    public bool Operation { get => _operation; set => _operation = value; } 
    public float RigitPower { get => _rigitPower; set => _rigitPower = value; }
    public float ElekePower { get => _elekePower; set => _elekePower = value; }
    public float FirePower { get => _firePower; set => _firePower = value; }
    public float FrozenPower { get => _frozenPower; set => _frozenPower = value; }

    public virtual void Awake()
    {
        //Start関数で呼びたい処理はこの関数に
        RendererActive(false);
        _myCollider = GetComponent<Collider>();
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

    
    public float ChargePower(TypeOfPower top,float magnification)
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
        if(magnification < 1)
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


