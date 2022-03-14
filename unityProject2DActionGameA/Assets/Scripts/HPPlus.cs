using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPlus : ItemBase
{
    [SerializeField] int m_HPPlusParameter = 4;
    bool hitFlg = false;
    GaugeManager m_playerGagueManager;
    public override void Activate(Collider other)
    {
        m_playerGagueManager = other.gameObject.GetComponent<GaugeManager>();
        m_playerGagueManager.HP += m_HPPlusParameter;
    }
}
