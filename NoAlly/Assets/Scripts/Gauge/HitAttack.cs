using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitAttack : MonoBehaviour, IHitBehavorOfAttack
{
    [SerializeField, Tooltip("オブジェクトのStatusBase")]
    StatusBase status = null;
    [SerializeField, Tooltip("物理防御力")]
    float _rigitDefensePercentage = 0f;
    [SerializeField, Tooltip("炎防御力")]
    float _fireDifansePercentage = 0f;
    [SerializeField, Tooltip("電気防御力")]
    float _elekeDifansePercentage = 0f;
    [SerializeField, Tooltip("氷結防御力")]
    float _frozenDifansePercentage = 0f;

    
    public float BehaviorOfHit(WeaponBase weaponStatus, ElementType type)
    {
        float currentHP = status.HP.Value;
        float baseDamage = weaponStatus.RigitPower * _rigitDefensePercentage;
        float elemantDamage = 0;
        switch (type)
        {
            case ElementType.FIRE:
                elemantDamage = weaponStatus.FirePower * _fireDifansePercentage;
                break;
            case ElementType.ELEKE:
                elemantDamage = weaponStatus.ElekePower * _elekeDifansePercentage;
                break;
            case ElementType.FROZEN:
                elemantDamage = weaponStatus.FrozenPower * _frozenDifansePercentage;
                break;
            default:
                break;
        }
        currentHP -= (baseDamage + elemantDamage);
        return currentHP;
    }
}
