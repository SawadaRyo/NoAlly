using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// “–‚½‚è”»’è‚Æƒ_ƒ[ƒWŒvZ‚Ì—v‘f‚ğ“n‚·ŠÖ”
/// </summary>

[RequireComponent(typeof(Collider))]
public class HitParameter : MonoBehaviour, IHitBehavorOfAttack
{
    [SerializeField]
    HitOwner _owner = HitOwner.NONE;
    [Header("UŒ‚‚³‚ê‚½‚Ì”{—¦B’l‚ª‚‚¢‚Ù‚Çó‚¯‚éƒ_ƒ[ƒW‚ªã‚ª‚é")]
    [Range(0f, 2f), SerializeField, Header("•¨—UŒ‚‚Ì”{—¦")]
    float _rigitDefensePercentage = 1f;
    [Range(0f, 2f), SerializeField, Header("‰ŠUŒ‚‚Ì”{—¦")]
    float _fireDifansePercentage = 1f;
    [Range(0f, 2f), SerializeField, Header("“d‹CUŒ‚‚Ì”{—¦")]
    float _elekeDifansePercentage = 1f;
    [Range(0f, 2f), SerializeField, Header("•XUŒ‚‚Ì”{—¦")]
    float _frozenDifansePercentage = 1f;

    [Tooltip("ƒIƒuƒWƒFƒNƒg‚ÌStatusBase")]
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
