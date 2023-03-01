using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 当たり判定とダメージ計算の要素を渡す関数
/// </summary>

[RequireComponent(typeof(Collider))]
public class HitParameter : MonoBehaviour, IHitBehavorOfAttack
{
    [SerializeField, Tooltip("このクラスのオーナー")]
    ObjectOwner _owner;
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

    /// <summary>
    /// 物理攻撃の倍率のプロパティ(読み取り専用)
    /// </summary>
    public float RigitDefensePercentage => _rigitDefensePercentage;
    /// <summary>
    /// 炎攻撃の倍率のプロパティ(読み取り専用)
    /// </summary>
    public float FireDifansePercentage => _fireDifansePercentage;
    /// <summary>
    /// 電気攻撃の倍率のプロパティ(読み取り専用)
    /// </summary>
    public float ElekeDifansePercentage => _elekeDifansePercentage;
    /// <summary>
    /// 氷攻撃の倍率のプロパティ(読み取り専用)
    /// </summary>
    public float FrozenDifansePercentage => _frozenDifansePercentage;
    /// <summary>
    /// オブジェクトのStatusBaseのプロパティ(読み取り専用)
    /// </summary>
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
    public ObjectOwner Owner => _owner;

    public void BehaviorOfHIt(float[] damageValue, ElementType type)
    {
        Status.Damage(damageValue, this, type);
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
