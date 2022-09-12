using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPlus : ItemBase
{
    [SerializeField] int m_HPPlusParameter = 4;
    PlayerGauge m_playerGagueManager;
    public override void Activate(Collider other)
    {
        m_playerGagueManager = other.gameObject.GetComponent<PlayerGauge>();
        m_playerGagueManager.HPPuls(m_HPPlusParameter);
    }
}
