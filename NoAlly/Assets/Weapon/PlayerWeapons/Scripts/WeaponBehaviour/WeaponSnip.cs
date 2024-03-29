﻿using System;
using UnityEngine;

//ToDo 装備中の属性によってBulletの攻撃力を変更できるようにする
public class WeaponArrow : WeaponBase, IObjectGenerator,IArrowWeapon
{
    string _filePath = "PlayerBullet";
    string _findMuzzlePath = "PlayerB/Muzzle";

    Transform _muzzleForward = default;
    PlayerContoller _contoller = null;
    ObjectPool<PlayerBulletBase, WeaponArrow, BulletType> _bPool = new ObjectPool<PlayerBulletBase, WeaponArrow, BulletType>();
    ObjectPool<PlayerBulletBase, WeaponArrow, BulletType>.ObjectKey[] _keys = new ObjectPool<PlayerBulletBase, WeaponArrow, BulletType>
                                                                                .ObjectKey[Enum.GetNames(typeof(BulletType)).Length];
    PlayerBulletBase[] _bulletPrefab = new PlayerBulletBase[3];

    public Transform GenerateTrance => _muzzleForward;
    public ActorVec playerVec => _contoller.Vec;
    public ObjectPool<PlayerBulletBase, WeaponArrow, BulletType> BPool => _bPool;
    public ObjectPool<PlayerBulletBase, WeaponArrow, BulletType>.ObjectKey[] BPoolKeys => _keys;


    public WeaponArrow(WeaponDataEntity weaponData,PlayerContoller player,Transform attackPos,Transform poolPos) : base(weaponData,attackPos)
    {
        _muzzleForward = attackPos;
        _contoller = player;
        _bulletPrefab = Resources.LoadAll<PlayerBulletBase>(_filePath);
        for (int i = 0; i < _bulletPrefab.Length; i++)
        {
            _keys[i] = _bPool.SetBaseObj(_bulletPrefab[i], this, poolPos, (BulletType)i);
            _bPool.SetCapacity(_keys[i], 10);
        }
    }

    public void InsBullet(IWeaponAction weaponAction)
    {
        if(weaponAction is IArrowAction arrow)
        {
            BulletType type = arrow.WeaponChargeAttackMethod(weaponAction.ChargeCount, _chargeLevels, _elementType);
            _bPool.Instantiate(_keys[(int)type]);
            weaponAction.ChargeCount = 0f;
        }
    }
}

public enum BulletType : int
{
    NORMAL,
    STRENGTHEN,
    CANNON
}