using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAPPlus : ItemBase
{
    [SerializeField] int m_SAPPlusParameter = 4;
    bool hitFlg = false;
    GaugeManager m_playerGagueManager;
    public override void Activate(Collider other)
    {
        m_playerGagueManager = other.gameObject.GetComponent<GaugeManager>();
        m_playerGagueManager.SAP += m_SAPPlusParameter;
    }
}
