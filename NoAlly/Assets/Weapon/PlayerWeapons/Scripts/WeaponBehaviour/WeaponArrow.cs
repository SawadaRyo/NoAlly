using System;
using UnityEngine;

//ToDo 装備中の属性によってBulletの攻撃力を変更できるようにする
public class WeaponArrow : WeaponBase, IObjectGenerator,IArrowWeapon
{
    [Tooltip("弾のプレハブのパス")]
    string _filePath = "PlayerBullet";
    [Tooltip("弾の生成タイミング")]
    float _shootTiming = 0.275f;
    [Tooltip("弾の生成座標")]
    Transform _muzzlePos = default;
    [Tooltip("")]
    ObjectPool<PlayerBulletBase, WeaponArrow, BulletType> _bPool = new ObjectPool<PlayerBulletBase, WeaponArrow, BulletType>();
    [Tooltip("")]
    ObjectPool<PlayerBulletBase, WeaponArrow, BulletType>.ObjectKey[] _keys = new ObjectPool<PlayerBulletBase, WeaponArrow, BulletType>
                                                                                .ObjectKey[Enum.GetNames(typeof(BulletType)).Length];
    PlayerBulletBase[] _bulletPrefab = new PlayerBulletBase[3];

    public Transform GenerateTrance => _muzzlePos;
    public ObjectPool<PlayerBulletBase, WeaponArrow, BulletType> BPool => _bPool;
    public ObjectPool<PlayerBulletBase, WeaponArrow, BulletType>.ObjectKey[] BPoolKeys => _keys;

    public override void Initializer(PlayerBehaviorController owner,WeaponController baseObj, WeaponDataEntity weaponData)
    {
        base.Initializer(owner, baseObj, weaponData);
        _muzzlePos = _base.GetAttackPos;
        //_contoller = player;
        _bulletPrefab = Resources.LoadAll<PlayerBulletBase>(_filePath);
        for (int i = 0; i < _bulletPrefab.Length; i++)
        {
            _keys[i] = _bPool.SetBaseObj(_bulletPrefab[i], this, _base.GetPoolPos, (BulletType)i);
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