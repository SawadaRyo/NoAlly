using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IObjectPool
{
    [SerializeField] protected float m_rigitPower = 5;
    [SerializeField] protected float m_elekePower = 0;
    [SerializeField] protected float m_firePower = 0;
    [SerializeField] protected float m_frozenPower = 0;
    [SerializeField] protected LayerMask enemyLayer = ~0;
    [SerializeField] Renderer[] m_weaponRenderer = default;

    bool m_isActive = false;
    Collider m_collider = default;

    public bool IsActive { get => m_isActive; set => m_isActive = value; }
    public float RigitPower { get => m_rigitPower; set => m_rigitPower = value; }
    public float ElekePower { get => m_elekePower; set => m_elekePower = value; }
    public float FirePower { get => m_firePower; set => m_firePower = value; }
    public float FrozenPower { get => m_frozenPower; set => m_frozenPower = value; }

    public virtual void Start()
    {
        //StartŠÖ”‚ÅŒÄ‚Ñ‚½‚¢ˆ—‚Í‚±‚ÌŠÖ”‚É
        m_collider = GetComponent<Collider>();
    }
    public virtual void Update()
    {
        //UpdateŠÖ”‚ÅŒÄ‚Ñ‚½‚¢ˆ—‚Í‚±‚ÌŠÖ”‚É
    }
    public virtual void MovementOfWeapon() { }
    public virtual void MovementOfWeapon(Collider target) { }
    public void RendererActive(bool stats)
    {
        foreach (var weaponRend in m_weaponRenderer)
        {
            weaponRend.enabled = stats;
        }
    }
    public void Create()
    {
        foreach (var weaponRend in m_weaponRenderer)
        {
            weaponRend.enabled = true;
        }
        m_collider.enabled = true;
    }

    public void Disactive()
    {
        foreach (var weaponRend in m_weaponRenderer)
        {
            weaponRend.enabled = false;
        }
        m_collider.enabled = false;
    }

    public void DisactiveForInstantiate()
    {
        
    }
}


