using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DifanseParameter : MonoBehaviour, IHitBehavorOfAttack
{
    [Tooltip("�I�u�W�F�N�g��StatusBase")]
    StatusBase _status = null;
    [Range(0f,2f),SerializeField, Tooltip("�����h���")]
    float _rigitDefensePercentage = 0f;
    [Range(0f, 2f), SerializeField, Tooltip("���h���")]
    float _fireDifansePercentage = 0f;
    [Range(0f, 2f), SerializeField, Tooltip("�d�C�h���")]
    float _elekeDifansePercentage = 0f;
    [Range(0f, 2f), SerializeField, Tooltip("�X���h���")]
    float _frozenDifansePercentage = 0f;

    public float RigitDefensePercentage => _rigitDefensePercentage;
    public float FireDifansePercentage => _fireDifansePercentage;
    public float ElekeDifansePercentage => _elekeDifansePercentage;
    public float FrozenDifansePercentage => _frozenDifansePercentage;

    void Start()
    {
        _status = GetComponentInParent<StatusBase>();
    }

    public void BehaviorOfHit(WeaponBase weaponStatus, ElementType type)
    {
        _status.DamageCalculation(weaponStatus, this, type);
    }
}
