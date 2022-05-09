using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAPPlus : ItemBase
{
    [SerializeField] int m_SAPPlusParameter = 4;
    PlayerHP m_playerGagueManager;
    public override void Activate(Collider other)
    {
        m_playerGagueManager = other.gameObject.GetComponent<PlayerHP>();
        m_playerGagueManager.SAPPuls(m_SAPPlusParameter);
    }
}
