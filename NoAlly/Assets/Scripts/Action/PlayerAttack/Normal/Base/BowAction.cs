using System;
using UnityEngine;

[RequireComponent(typeof(WeaponBase))]
public class BowAction : WeaponAction, IObjectGenerator
{
    [SerializeField] string _filePath = "PlayerBullet";
    [SerializeField] string _findMuzzlePath = "PlayerB/Muzzle";
    [SerializeField] Transform _poolPos = null;
    [SerializeField] PersonType _personType = PersonType.Player;

    Transform _muzzleForward = default;
    ObjectPool<PlayerBulletBase, BowAction, BulletType> _bPool = new ObjectPool<PlayerBulletBase, BowAction, BulletType>();
    ObjectPool<PlayerBulletBase, BowAction, BulletType>.ObjectKey[] _keys = null;
    PlayerBulletBase[] _bulletPrefab = new PlayerBulletBase[3];
    BulletType _bulletType = BulletType.NORMAL;

    public Transform GenerateTrance => _muzzleForward;


    public override void Initialize(PlayerContoller player, WeaponBase weaponBase)
    {
        base.Initialize(player, weaponBase);
        _keys = new ObjectPool<PlayerBulletBase, BowAction, BulletType>.ObjectKey[Enum.GetNames(typeof(BulletType)).Length];
        _muzzleForward = GameObject.Find(_findMuzzlePath).transform;
        _bulletPrefab = Resources.LoadAll<PlayerBulletBase>(_filePath);
        for (int i = 0; i < _bulletPrefab.Length; i++)
        {
            _keys[i] = _bPool.SetBaseObj(_bulletPrefab[i], this, _poolPos, (BulletType)i);
            //Debug.Log(_keys[i]);
            _bPool.SetCapacity(_keys[i], 10);
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
                _bulletType = BulletType.NORMAL;
            }
            //‹­‰»’e
            else if (_chrageCount > _chargeLevel1 && _chrageCount <= _chargeLevel2)
            {
                _bulletType = BulletType.STRENGTHEN;
            }
            //‘å–C
            else if (_chrageCount > _chargeLevel2)
            {
                _bulletType = BulletType.CANNON;
            }

            var bullletObj = _bPool.Instantiate(_keys[(int)_bulletType]);

        }
    }

    public override void ResetValue()
    {
        base.ResetValue();
        _bulletType = 0;
    }
}
public enum BulletType : int
{
    NORMAL,
    STRENGTHEN,
    CANNON
}
enum PersonType
{
    Player,
    Enemy
}
