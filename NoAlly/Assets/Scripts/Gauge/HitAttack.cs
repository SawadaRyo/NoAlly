using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitAttack : MonoBehaviour, IHitBehavorOfAttack
{
    [SerializeField, Tooltip("ÉIÉuÉWÉFÉNÉgÇÃStatusBase")]
    StatusBase status = null;
    [SerializeField, Tooltip("ï®óùñhå‰óÕ")]
    float _rigitDefensePercentage = 0f;
    [SerializeField, Tooltip("âäñhå‰óÕ")]
    float _fireDifansePercentage = 0f;
    [SerializeField, Tooltip("ìdãCñhå‰óÕ")]
    float _elekeDifansePercentage = 0f;
    [SerializeField, Tooltip("ïXåãñhå‰óÕ")]
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
