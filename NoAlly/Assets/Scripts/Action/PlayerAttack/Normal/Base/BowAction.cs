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

    int _bulletType = 0;
    ObjectPool<BulletBase> _bPool = new ObjectPool<BulletBase>();
    BulletBase[] _bulletPrefab = new BulletBase[3];
    
    public Transform MuzzlePos => _muzzleForward;

    public void Start()
    {
        _bulletPrefab = Resources.LoadAll<BulletBase>(_filePath);
        for (int i = 0; i < _bulletPrefab.Length; i++)
        {
            _bPool.SetBaseObj(_bulletPrefab[i], _poolPos, i);
            _bPool.SetCapacity(10);
        }
    }

    public override void WeaponChargeAttackMethod()
    {
        if (_personType == PersonType.Player)
        {
            Debug.Log(_chrageCount);
            //’Êí’e
            if (_chrageCount <= _chargeLevel1)
            {
                _bulletType = 0;
            }
            //‹­‰»’e
            else if (_chrageCount > _chargeLevel1 && _chrageCount <= _chargeLevel2)
            {
                _bulletType = 1;
            }
            //‘å–C
            else if (_chrageCount > _chargeLevel2)
            {
                _bulletType = 2;
            }

            var bullletObj = _bPool.Instantiate(_bulletType);
            
        }
    }

    public override void ResetValue()
    {
        base.ResetValue();
        _bulletType = 0;
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
