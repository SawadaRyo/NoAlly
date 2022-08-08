using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BowAction : WeaponAction
{
    [SerializeField] float m_bulletSpeed = 5;
    [SerializeField] string m_filePath = "";
    [SerializeField] Transform m_muzzleForward = default;
    [SerializeField] PersonType m_personType = PersonType.Player;

    int m_bulletType = 0;
    Bullet[] m_bulletPrefab = new Bullet[3];

    enum PersonType { Player, Enemy };

    public override void Enable()
    {
        base.Enable();
        m_bulletPrefab = Resources.LoadAll<Bullet>(m_filePath);
    }

    public override void WeaponChargeAttackMethod(float chrageCount)
    {
        if (m_personType == PersonType.Player)
        {
            Debug.Log(chrageCount);
            //í èÌíe
            if (chrageCount <= m_chargeLevel1)
            {
                m_bulletType = 0;
            }
            //ã≠óÕíe
            else if (chrageCount > m_chargeLevel1 && chrageCount <= m_chargeLevel2)
            {
                m_bulletType = 1;
            }
            //ëÂñC
            else if (chrageCount > m_chargeLevel2)
            {
                m_bulletType = 2;
            }

            var bullletObj = Instantiate(m_bulletPrefab[m_bulletType], m_muzzleForward.position, Quaternion.Euler(0f, 0f, 90f));
            bullletObj.GetComponent<Rigidbody>().velocity = m_muzzleForward.forward * m_bulletSpeed;
            m_bulletType = 0; //ë≈ÇøèIÇÌÇ¡ÇΩå„íeÇÃprefabÇí èÌíeÇ…ñﬂÇ∑
        }
    }
}
