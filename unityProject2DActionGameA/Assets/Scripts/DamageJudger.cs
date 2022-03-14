using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageJudger : MonoBehaviour　//攻撃判定のスクリプト
{
    Collider m_hitRenge = default;
    // Start is called before the first frame update
    void Start()
    {
        m_hitRenge = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {
            var weaponPower = other.gameObject.GetComponent<WeaponBase>();
            var playerHP = gameObject.GetComponent<GaugeManager>();
            playerHP.HP -= weaponPower.WeaponPower;
        }
    }
}
