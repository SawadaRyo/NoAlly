using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ToDo ‘•”õ’†‚Ì‘®«‚É‚æ‚Á‚ÄBullet‚ÌUŒ‚—Í‚ğ•ÏX‚Å‚«‚é‚æ‚¤‚É‚·‚é
public class SnipWeapon : WeaponBase
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
}
