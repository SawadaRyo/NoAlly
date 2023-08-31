//日本語コメント可
using UnityEngine;

[CreateAssetMenu(fileName = "ActorParamater", menuName = "ScriptableObjects/ActorParamater/Enemy", order = 1)]
public abstract class EnemyParamaterBase : ScriptableObject
{
    [SerializeField,Tooltip("")]
    WeaponPower[] enemyPowers;
    [SerializeField,Tooltip("索敵範囲の中心")]
    Vector3 searchCentar = Vector3.zero;
    [SerializeField, Tooltip("索敵範囲の有効範囲")]
    float searchRenge = 0f;
    [SerializeField, Header("索敵用のレイヤー")]
    LayerMask playerLayer = ~0;
}
