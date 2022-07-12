using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IObjectPool
{
    [SerializeField] protected int m_rigitPower = 5;
    [SerializeField] protected int m_elekePower = 0;
    [SerializeField] protected int m_firePower = 0;
    [SerializeField] protected int m_frozenPower = 0;
    [SerializeField] protected LayerMask enemyLayer = ~0;
    [SerializeField] Renderer[] m_weaponRenderer = default;

    bool m_isActive = false;

    public Renderer[] WeaponRenderer { get => m_weaponRenderer; set => m_weaponRenderer = value; }
    public bool IsActive { get => m_isActive; set => m_isActive = value; }

    public virtual void Start()
    {
        //StartŠÖ”‚ÅŒÄ‚Ñ‚½‚¢ˆ—‚Í‚±‚ÌŠÖ”‚É
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

    }

    public void Death()
    {

    }

    public void DisactiveForInstantiate()
    {

    }

}
