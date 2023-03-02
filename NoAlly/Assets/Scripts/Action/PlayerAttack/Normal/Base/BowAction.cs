using System;
using UnityEngine;

[RequireComponent(typeof(SnipWeapon))]
public class BowAction : WeaponAction
{
    [SerializeField] 
    PersonType _personType = PersonType.Player;

    SnipWeapon _snipWeapon = null;
    BulletType _bulletType = BulletType.NORMAL;

    public override void Initialize(PlayerContoller player, WeaponBase weaponBase)
    {
        base.Initialize(player, weaponBase);
        _snipWeapon = weaponBase as SnipWeapon;
    }

    public override void WeaponChargeAttackMethod()
    {
        if (_personType == PersonType.Player)
        {
            Debug.Log(_chrageCount);
            //í èÌíe
            if (_chrageCount <= _chargeLevel1)
            {
                _bulletType = BulletType.NORMAL;
            }
            //ã≠âªíe
            else if (_chrageCount > _chargeLevel1 && _chrageCount <= _chargeLevel2)
            {
                _bulletType = BulletType.STRENGTHEN;
            }
            //ëÂñC
            else if (_chrageCount > _chargeLevel2)
            {
                _bulletType = BulletType.CANNON;
            }

            _snipWeapon.BPool.Instantiate(_snipWeapon.BPoolKeys[(int)_bulletType]);
        }
    }

    public override void ResetValue()
    {
        base.ResetValue();
        _bulletType = 0;
    }
}

enum PersonType
{
    Player,
    Enemy
}
