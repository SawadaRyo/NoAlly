using System;
using UnityEngine;

//ToDo 装備中の属性によってBulletの攻撃力を変更できるようにする
public class SnipWeapon : WeaponBase, IObjectGenerator
{
    [SerializeField] string _filePath = "PlayerBullet";
    [SerializeField] string _findMuzzlePath = "PlayerB/Muzzle";
    [SerializeField] Transform _poolPos = null;
    [SerializeField] PersonType _personType = PersonType.Player;

    Transform _muzzleForward = default;
    ObjectPool<PlayerBulletBase, SnipWeapon, BulletType> _bPool = new ObjectPool<PlayerBulletBase, SnipWeapon, BulletType>();
    ObjectPool<PlayerBulletBase, SnipWeapon, BulletType>.ObjectKey[] _keys = new ObjectPool<PlayerBulletBase, SnipWeapon, BulletType>
                                                                                .ObjectKey[Enum.GetNames(typeof(BulletType)).Length];
    PlayerBulletBase[] _bulletPrefab = new PlayerBulletBase[3];
    BulletType _bulletType = BulletType.NORMAL;

    public Transform GenerateTrance => _muzzleForward;
    public ObjectPool<PlayerBulletBase, SnipWeapon, BulletType> BPool => _bPool;
    public ObjectPool<PlayerBulletBase, SnipWeapon, BulletType>.ObjectKey[] BPoolKeys => _keys;


    public override void Initialize(WeaponDataEntity weaponData)
    {
        base.Initialize(weaponData);
        _muzzleForward = GameObject.Find(_findMuzzlePath).transform;
        _bulletPrefab = Resources.LoadAll<PlayerBulletBase>(_filePath);
        for (int i = 0; i < _bulletPrefab.Length; i++)
        {
            _keys[i] = _bPool.SetBaseObj(_bulletPrefab[i], this, _poolPos, (BulletType)i);
            _bPool.SetCapacity(_keys[i], 10);
        }
    }
}

public enum BulletType : int
{
    NORMAL,
    STRENGTHEN,
    CANNON
}