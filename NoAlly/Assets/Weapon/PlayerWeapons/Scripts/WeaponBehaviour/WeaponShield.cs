using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WeaponShield : WeaponBase
{
    AsyncOperationHandle _handle;
    ShieldWall _shieldWallPrefab = null;

    /// <summary>
    /// ���̃N���X�̃C���X�^���X������7�����ۂɌĂ΂��
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
    /// ���͎��̋���
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
    /// �f�X�g���N�^
    /// </summary>
    ~WeaponShield()
    {
        Addressables.ReleaseInstance(_handle);
        _shieldWallPrefab = null;
    }
}
