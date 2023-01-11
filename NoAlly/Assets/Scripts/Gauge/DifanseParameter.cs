using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DifanseParameter : MonoBehaviour, IHitBehavorOfAttack
{
    [Range(0f,2f),SerializeField, Tooltip("•¨—–hŒä—Í")]
    float _rigitDefensePercentage = 0f;
    [Range(0f, 2f), SerializeField, Tooltip("‰Š–hŒä—Í")]
    float _fireDifansePercentage = 0f;
    [Range(0f, 2f), SerializeField, Tooltip("“d‹C–hŒä—Í")]
    float _elekeDifansePercentage = 0f;
    [Range(0f, 2f), SerializeField, Tooltip("•XŒ‹–hŒä—Í")]
    float _frozenDifansePercentage = 0f;
    [Tooltip("ƒIƒuƒWƒFƒNƒg‚ÌStatusBase")]
    StatusBase _status = null;

    public float RigitDefensePercentage => _rigitDefensePercentage;
    public float FireDifansePercentage => _fireDifansePercentage;
    public float ElekeDifansePercentage => _elekeDifansePercentage;
    public float FrozenDifansePercentage => _frozenDifansePercentage;

    void Start()
    {
        _status = GetComponentInParent<StatusBase>();
    }
    public void BehaviorOfHit(WeaponBase weaponStatus, ElementType type, WeaponOwner owner)
    {
        _status.DamageCalculation(weaponStatus, this, type, owner);
    }
}
