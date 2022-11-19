using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAttack : MonoBehaviour, IHitBehavorOfAttack
{
    [SerializeField, Tooltip("�I�u�W�F�N�g��StatusBase")]
    StatusBase status = null;
    [SerializeField, Tooltip("�����h���")]
    float _rigitDefensePercentage = 0f;
    [SerializeField, Tooltip("���h���")]
    float _fireDifansePercentage = 0f;
    [SerializeField, Tooltip("�d�C�h���")]
    float _elekeDifansePercentage = 0f;
    [SerializeField, Tooltip("�X���h���")]
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
