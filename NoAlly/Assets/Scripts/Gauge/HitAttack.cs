using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float BehaviorOfHit(float weaponPower, float firePower, float elekePower, float frozenPower, ElementType type)
    {
        float currentHP = status.HP.Value;
        float baseDamage = weaponPower * _rigitDefensePercentage;
        float elemantDamage = 0;
        switch (type)
        {
            case ElementType.FIRE:
                elemantDamage = firePower * _fireDifansePercentage;
                break;
            case ElementType.ELEKE:
                elemantDamage = elekePower * _elekeDifansePercentage;
                break;
            case ElementType.FROZEN:
                elemantDamage = frozenPower * _frozenDifansePercentage;
                break;
        }
        currentHP -= (baseDamage + elemantDamage);
        return currentHP;
    }
}
