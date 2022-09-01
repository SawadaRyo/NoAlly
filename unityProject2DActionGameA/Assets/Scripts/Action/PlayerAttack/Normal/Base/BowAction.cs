using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BowAction : WeaponAction
{
    [SerializeField] float _bulletSpeed = 5;
    [SerializeField] string _filePath = "";
    [SerializeField] Transform _muzzleForward = default;
    [SerializeField] Transform _poolPos = default;
    [SerializeField] PersonType _personType = PersonType.Player;

    int m_bulletType = 0;
    ObjectPool<Bullet> _bPool = new ObjectPool<Bullet>();
    Bullet[] _bulletPrefab = new Bullet[3];
    
    public Transform MuzzlePos => _muzzleForward;

    public void Start()
    {
        _bulletPrefab = Resources.LoadAll<Bullet>(_filePath);
        for (int i = 0; i < _bulletPrefab.Length; i++)
        {
            _bPool.SetBaseObj(_bulletPrefab[i], _poolPos, i);
            _bPool.SetCapacity(10);
        }
    }

    public override void WeaponChargeAttackMethod(float chrageCount)
    {
        if (_personType == PersonType.Player)
        {
            Debug.Log(chrageCount);
            //í èÌíe
            if (chrageCount <= _chargeLevel1)
            {
                m_bulletType = 0;
            }
            //ã≠âªíe
            else if (chrageCount > _chargeLevel1 && chrageCount <= _chargeLevel2)
            {
                m_bulletType = 1;
            }
            //ëÂñC
            else if (chrageCount > _chargeLevel2)
            {
                m_bulletType = 2;
            }

            var bullletObj = _bPool.Instantiate(m_bulletType);
            //bullletObj.GetComponent<Rigidbody>().velocity = _muzzleForward.forward * _bulletSpeed;
            m_bulletType = 0; //ë≈ÇøèIÇÌÇ¡ÇΩå„íeÇÃprefabÇí èÌíeÇ…ñﬂÇ∑
        }
    }


}
public enum BulletType
{
    NORMAL = 0,
    STRENGTHEN = 1,
    CANNON = 2
}
enum PersonType
{
    Player, 
    Enemy
}
