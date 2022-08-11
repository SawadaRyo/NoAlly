using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BowAction : WeaponAction
{
    [SerializeField] float _bulletSpeed = 5;
    [SerializeField] string _filePath = "";
    [SerializeField] Transform _muzzleForward = default;
    [SerializeField] PersonType _personType = PersonType.Player;

    int m_bulletType = 0;
    ObjectPool<Bullet>[] _bulletPool = new ObjectPool<Bullet>[3];
    Bullet[] _bulletPrefab = new Bullet[3];

    enum PersonType { Player, Enemy };

    public void Start()
    {
        _bulletPrefab = Resources.LoadAll<Bullet>(_filePath);
        for(int i = 0;i < _bulletPool.Length;i++)
        {
            _bulletPool[i].SetBaseObj(_bulletPrefab[i], _muzzleForward);
            _bulletPool[i].SetCapacity(10);
        }
    }

    public override void WeaponChargeAttackMethod(float chrageCount)
    {
        if (_personType == PersonType.Player)
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

            var bullletObj = _bulletPool[m_bulletType].Instantiate();
            bullletObj.GetComponent<Rigidbody>().velocity = _muzzleForward.forward * _bulletSpeed;
            m_bulletType = 0; //ë≈ÇøèIÇÌÇ¡ÇΩå„íeÇÃprefabÇí èÌíeÇ…ñﬂÇ∑
        }
    }
}
