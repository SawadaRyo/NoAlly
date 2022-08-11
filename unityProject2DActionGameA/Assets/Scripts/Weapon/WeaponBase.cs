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


    public float RigitPower { get => _rigitPower; set => _rigitPower = value; }
    public float ElekePower { get => _elekePower; set => _elekePower = value; }
    public float FirePower { get => _firePower; set => _firePower = value; }
    public float FrozenPower { get => _frozenPower; set => _frozenPower = value; }

    public virtual void Awake()
    {
        //StartŠÖ”‚ÅŒÄ‚Ñ‚½‚¢ˆ—‚Í‚±‚ÌŠÖ”‚É
        RendererActive(false);
    }
    public virtual void Update()
    {
        //UpdateŠÖ”‚ÅŒÄ‚Ñ‚½‚¢ˆ—‚Í‚±‚ÌŠÖ”‚É
    }
    public virtual void OnApplicationQuit() { }
    public virtual void MovementOfWeapon() { }
    public virtual void MovementOfWeapon(Collider target) { }
    public virtual void WeaponMode(ElementType type) { }
    public virtual void RendererActive(bool stats)
    {
        foreach (var weaponRend in _weaponRenderer)
        {
            weaponRend.enabled = stats;
        }
    }
}


