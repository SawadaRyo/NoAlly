using UnityEngine;

public class BrastarWeapon : CombatWeapon, IWeapon
{
    float _normalRigit = 0f;
    float _normalEleke = 0f;
    float _powerUpRigit = 3.5f;
    float _powerUpEleke = 5f;

    public override void SetData(WeaponDataEntity weapon)
    {
        base.SetData(weapon);
        _normalRigit = _rigitPower;
        _normalEleke = _elekePower;
    }
    public override void WeaponMode(ElementType type)
    {
        switch (type)
        {
            case ElementType.ELEKE:
                _isDeformated = WeaponDeformation.Deformation;
                _rigitPower = _powerUpRigit;
                _elekePower = _powerUpEleke;
                _weaponAnimator.SetBool("IsOpen", true);
                break;
            default:
                _isDeformated = WeaponDeformation.NONE;
                _rigitPower = _normalRigit;
                _elekePower = _normalEleke;
                break;
        }
        base.WeaponMode(type);
    }
}
