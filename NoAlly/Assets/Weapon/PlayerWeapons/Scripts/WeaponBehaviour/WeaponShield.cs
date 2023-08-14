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
    public override void Initializer(WeaponController owner, WeaponDataEntity weaponData)
    {
        base.Initializer(owner, weaponData);
        SetPrefab();
    }

    async void SetPrefab()
    {
        _handle = Addressables.LoadAssetAsync<ShieldWall>("ShieldWall");
        await _handle.Task;
        _shieldWallPrefab = GameObject.Instantiate((ShieldWall)_handle.Result, _owner.GetAttackPos);
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
        Collider[] targets = Physics.OverlapBox(_owner.GetAttackPos.position
                                              , _owner.GetAttackPos.localScale
                                              , _owner.GetAttackPos.rotation
                                              , _owner.HitLayer);
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
