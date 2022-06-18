using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected int m_weaponPower = 5;
    [SerializeField] protected int m_elekePower = 0;
    [SerializeField] protected int m_firePower = 0;
    [SerializeField] protected int m_frozenPower = 0;
    [SerializeField] protected LayerMask enemyLayer = ~0;
    public int WeaponPower { get => m_weaponPower; set => m_weaponPower = value; }

    public virtual void Start()
    {
        //StartŠÖ”‚ÅŒÄ‚Ñ‚½‚¢ˆ—‚Í‚±‚ÌŠÖ”‚É
    }
    public virtual void Update()
    {
        //UpdateŠÖ”‚ÅŒÄ‚Ñ‚½‚¢ˆ—‚Í‚±‚ÌŠÖ”‚É
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out GaugeManager targetHp))
        {
            targetHp.DamageMethod(m_weaponPower, m_firePower, m_elekePower, m_frozenPower);
        }
    }
}
