using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WeaponShield : WeaponBase
{
    AsyncOperationHandle _handle;
    ShieldWall _shieldWallPrefab = null;
    Vector3 _boxRenge = Vector3.zero;

    /// <summary>
    /// このクラスのインスタンスが生成7される際に呼ばれる
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="weaponData"></param>
    public override void Initializer(PlayerBehaviorController owner, WeaponController baseObj, WeaponDataEntity weaponData)
    {
        base.Initializer(owner, baseObj, weaponData);
        _boxRenge = new Vector3(_base.GetAttackPos.localScale.x / 2
                              , _base.GetAttackPos.localScale.y / 2
                              , _base.GetAttackPos.localScale.z / 2);
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
        Collider[] cols = Physics.OverlapBox(_base.GetAttackPos.position
                                           , _boxRenge
                                           , _base.GetAttackPos.localRotation
                                           , _base.HitLayerToBlock);
        if (cols.Length == 0) return;
        foreach (Collider col in cols)
        {
            if (col.TryGetComponent(out ObjectBase damageObj))
            {
                damageObj.ActiveObject(false);
            }
        }
    }

    public override void HitObjectBehaviour(Collider col)
    {
        base.HitObjectBehaviour(col);
        if(col.gameObject.layer == Base.HitLayerToBlock)
        {
            Debug.Log("Hit");
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
