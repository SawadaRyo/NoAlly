using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConbatWeapon : WeaponBase
{
    [SerializeField] WeaponAction m_weaponChanger;
    BoxCollider m_boxCollider;

    public override void IsStart()
    {
        m_boxCollider = gameObject.GetComponent<BoxCollider>();
    }
    public override void IsUpdate()
    {
        if (m_weaponChanger.Attacked)
        {
            m_boxCollider.enabled = true;
        }
        else
        {
            m_boxCollider.enabled = false; 
        }
    }
    
}
