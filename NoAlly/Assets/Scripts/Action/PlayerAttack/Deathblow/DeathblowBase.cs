using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeathblowBase : MonoBehaviour
{
    [SerializeField] protected PlayerGauge m_playerHP = default;
    [SerializeField] protected int m_power = 20;
    [SerializeField] protected LayerMask m_targetLayer = ~0;
    [SerializeField] int needSAP = 20;
    protected Animator m_playerAnimator = default;
    public abstract void DeathblowAbility();

    void Start()
    {
        m_playerAnimator = gameObject.GetComponentInParent<Animator>();
    }

    void Update()
    {
        if(Input.GetButtonDown("SpecialAction"))
        {
            Debug.Log("a");
            if(m_playerHP.SAP >= needSAP)
            {
                m_playerAnimator.SetTrigger("DeathBlow");
                m_playerHP.SAP -= needSAP;
            }
        }
    }
}
