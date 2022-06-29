using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConbatWeapon : WeaponBase
{
    Collider m_boxCollider;

    public override void Start()
    {
        m_boxCollider = gameObject.GetComponent<Collider>();
    }
}
