using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletContoller : WeaponAction
{
    [SerializeField] Transform m_muzzleForward = default;
    [SerializeField] GameObject[] m_bulletPrefab = new GameObject[3];
    [SerializeField] PersonType m_personType = PersonType.Player;
    Bullet[] m_bullet = new Bullet[3];
    //ParticleSystem particle;
    int m_chargeLevel1 = 600;
    int m_chargeLevel2 = 1200;
    int m_bulletType = 0;

    enum PersonType { Player,Enemy };

    public override void IsStart()
    {
        for(int i = 0;i < m_bulletPrefab.Length;i++)
        {
            m_bullet[i] = m_bulletPrefab[i].GetComponent<Bullet>();
        }
        //particle = gameObject.GetComponent<ParticleSystem>();
    }
    
    public override void WeaponChargeAttackMethod()
    {
        if (m_personType == PersonType.Player)
        {
            //í èÌíe
            if (m_chrageAttackCount <= m_chargeLevel1)
            {
                m_bulletType = 0;
            }
            //ã≠óÕíe
            else if (m_chrageAttackCount > m_chargeLevel1 && m_chrageAttackCount <= m_chargeLevel2)
            {
                m_bulletType = 1;
            }
            //ëÂñC
            else if (m_chrageAttackCount > m_chargeLevel2)
            {
                m_bulletType = 2;
            }
        }
    }
    void BulletInsAndResat()
    {
        var bullletObj = Instantiate(m_bulletPrefab[m_bulletType], m_muzzleForward.position, Quaternion.Euler(0f, 0f, 90f)) as GameObject;
        m_bulletType = 0; //ë≈ÇøèIÇÌÇ¡ÇΩå„íeÇÃprefabÇí èÌíeÇ…ñﬂÇ∑
    }
}
