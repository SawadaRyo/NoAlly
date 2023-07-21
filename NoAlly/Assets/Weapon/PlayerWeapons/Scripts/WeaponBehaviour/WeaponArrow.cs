using System;
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

    public override void Initializer(WeaponController owner, WeaponDataEntity weaponData)
    {
        base.Initializer(owner, weaponData);
        _muzzleForward = _owner.GetAttackPos;
        //_contoller = player;
        _bulletPrefab = Resources.LoadAll<PlayerBulletBase>(_filePath);
        for (int i = 0; i < _bulletPrefab.Length; i++)
        {
            _keys[i] = _bPool.SetBaseObj(_bulletPrefab[i], this, _owner.GetPoolPos, (BulletType)i);
            _bPool.SetCapacity(_keys[i], 10);
        }
    }

    public void InsBullet()
    {
        //BulletType type = WeaponChargeAttackMethod(Owner.ChargeCount, _weaponData.ChargeLevels, _elementType);
        //_bPool.Instantiate(_keys[(int)type]);
        //weaponAction.ChargeCount = 0f;
    }

    public BulletType WeaponChargeAttackMethod(float chrageCount, float[] chargeLevels, ElementType elementType)
    {
        throw new NotImplementedException();
    }
}

public enum BulletType : int
{
    NORMAL,
    STRENGTHEN,
    CANNON
}