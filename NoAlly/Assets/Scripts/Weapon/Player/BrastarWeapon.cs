using UnityEngine;

public class BrastarWeapon : CombatWeapon
{
    public override void WeaponMode(ElementType type)
    {
        switch (type)
        {
            case ElementType.ELEKE:
                _isDeformated = WeaponDeformation.Deformation;
                _weaponAnimator.SetBool("IsOpen", true);
                break;
            default:
                _isDeformated = WeaponDeformation.NONE;
                _weaponAnimator.SetBool("IsOpen", false);
                break;
        }
        base.WeaponMode(type);
    }
}
