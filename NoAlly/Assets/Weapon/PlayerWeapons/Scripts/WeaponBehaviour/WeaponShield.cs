using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WeaponShield : WeaponBase
{
    AsyncOperationHandle _handle;
    ShieldWall _shieldWallPrefab = null;

    /// <summary>
    /// このクラスのインスタンスが生成7される際に呼ばれる
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="weaponData"></param>
    public override void Initializer(PlayerBehaviorController owner, WeaponController baseObj, WeaponDataEntity weaponData)
    {
        base.Initializer(owner, baseObj, weaponData);
        //SetPrefab();
    }

    async void SetPrefab()
    {
        _handle = Addressables.LoadAssetAsync<ShieldWall>("ShieldWall");
        await _handle.Task;
        _shieldWallPrefab = GameObject.Instantiate((ShieldWall)_handle.Result, _base.GetAttackPos);
        _shieldWallPrefab.WallActive(FadeType.FadeIn);
    }

    //public void IsBrocking()
    //{
    //GameObject.Instantiate _owner.GetAttackPos
    //}

    /// <summary>
    /// 入力時の挙動
    /// </summary>
    public override void AttackBehaviour()
    {
        Collider[] targets = Physics.OverlapBox(_base.GetAttackPos.position
                                              , _base.GetAttackPos.localScale
                                              , _base.GetAttackPos.rotation
                                              , _base.HitLayer);
        foreach (Collider targetCollider in targets)
        {
            //if(targetCollider.TryGetComponent(out IBullet<GunTypeEnemy> bullet))
            {
                //bullet.Disactive();
            }
        }
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~WeaponShield()
    {
        Addressables.ReleaseInstance(_handle);
        _shieldWallPrefab = null;
    }
}
