using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 当たり判定とダメージ計算の要素を渡す関数
/// </summary>

[RequireComponent(typeof(Collider))]
public class HitParameter : MonoBehaviour, IHitBehavorOfAttack
{
    [SerializeField]
    HitOwner _owner = HitOwner.NONE;
    [Header("攻撃された時の倍率。値が高いほど受けるダメージが上がる")]
    [Range(0f, 2f), SerializeField, Header("物理攻撃の倍率")]
    float _rigitDefensePercentage = 1f;
    [Range(0f, 2f), SerializeField, Header("炎攻撃の倍率")]
    float _fireDifansePercentage = 1f;
    [Range(0f, 2f), SerializeField, Header("電気攻撃の倍率")]
    float _elekeDifansePercentage = 1f;
    [Range(0f, 2f), SerializeField, Header("氷攻撃の倍率")]
    float _frozenDifansePercentage = 1f;

    [Tooltip("オブジェクトのStatusBase")]
    StatusBase _status = null;

    public float RigitDefensePercentage => _rigitDefensePercentage;
    public float FireDifansePercentage => _fireDifansePercentage;
    public float ElekeDifansePercentage => _elekeDifansePercentage;
    public float FrozenDifansePercentage => _frozenDifansePercentage;

    public HitOwner StatusOwner => _owner;
    StatusBase Status
    {
        get
        {
            if (_status == null)
            {
                _status = this.GetComponentInParent<StatusBase>();
            }
            return _status;
        }
    }
    public void BehaviorOfHit(WeaponBase weaponStatus, ElementType type, HitOwner owner)
    {
        if(_owner == owner) return;
        Status.Damage(weaponStatus, this, type);
    }
    public void BehaviorOfHit<TPlus>(TPlus pulsItem) where TPlus : ItemBase
    {

        if (Status is PlayerStatus)
        {
            PlayerStatus playerStatus = (PlayerStatus)Status;
            if (pulsItem is HPPlus)
            {
                playerStatus.HPPuls(pulsItem.PlusParameter);
            }
            else if (pulsItem is SAPPlus)
            {
                playerStatus.SAPPuls(pulsItem.PlusParameter);
            }
        }
    }
}
