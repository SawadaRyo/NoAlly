using UnityEngine;

public class BrastarWeapon : CombatWeapon, IWeapon
{
    float _normalRigit = 0f;
    float _normalEleke = 0f;
    float _powerUpRigit = 3.5f;
    float _powerUpEleke = 5f;

    public override void Initialize(WeaponDataEntity weapon)
    {
        base.Initialize(weapon);
        _normalRigit = _rigitPower;
        _normalEleke = _elekePower;
        _normalHarfExtents = new Vector3(0.25f, 1.2f, 0.1f);
        _pawerUpHarfExtents = new Vector3(0.4f, 1.7f, 0.1f);
        _harfExtents = _normalHarfExtents;
    }
    public override void WeaponMode(ElementType type)
    {
        switch (type)
        {
            case ElementType.ELEKE:
                _isDeformated = WeaponDeformation.Deformation;
                _harfExtents = _pawerUpHarfExtents;
                _rigitPower = _powerUpRigit;
                _elekePower = _powerUpEleke;
                _weaponAnimator.SetBool("IsOpen", true);
                break;
            default:
                _isDeformated = WeaponDeformation.None;
                _harfExtents = _normalHarfExtents;
                _rigitPower = _normalRigit;
                _elekePower = _normalEleke;
                break;
        }
        base.WeaponMode(type);
    }
}
