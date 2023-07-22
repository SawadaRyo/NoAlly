//日本語コメント可
using UniRx;
using UnityEngine;

public class WeaponStatePresenter : MonoBehaviour
{
    [SerializeField]
    WeaponController _weaponController;
    [SerializeField]
    WeaponAnimator _weaponAnimator;
    [SerializeField]
    InputToPlayerMove _inputToPlayer;

    void Start()
    {
        _weaponController.Initializer(WeaponType.SWORD, WeaponType.LANCE);
        _weaponAnimator.Initializer(_weaponController.WeaponPrefab);
        AnimationStateChacker();
        InputStateChacker();
    }

    void AnimationStateChacker()
    {
        _weaponController.EquipementWeapon
            .Subscribe(weapon =>
            {
                _weaponAnimator.DeformationWeapon(weapon.WeaponData.TypeOfWeapon, _weaponController.CurrentElement.Value);
            });
        _weaponController.CurrentElement
            .Subscribe(element =>
            {
                _weaponAnimator.DeformationWeapon(_weaponController.EquipementWeapon.Value.WeaponData.TypeOfWeapon, element);
            });
    }
    void InputStateChacker()
    {
        _inputToPlayer.IsSwichWeapon
            .Subscribe(switchWeapon =>
            {
                _weaponController.SwichEquipmentWeapon(switchWeapon);
            });
        _inputToPlayer.InputAttackDown
            .Where(_ => _inputToPlayer.InputAttackDown.Value)
            .Subscribe(inputDown =>
            {

            });
        _inputToPlayer.InputAttackCharge
            .Where(_ => _inputToPlayer.InputAttackCharge.Value)
            .Subscribe(inputCharge =>
            {
                
            });
        _inputToPlayer.InputAttackUp
            .Where(_ => _inputToPlayer.InputAttackUp.Value)
            .Subscribe(inputUp =>
            {
                _weaponController.EquipementWeapon.Value.Charge(false);
            });
    }
}
