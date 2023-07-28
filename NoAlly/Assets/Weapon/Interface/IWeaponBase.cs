using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IWeaponBase<T> where T : class
{
    public T WeaponOwner { get; }
    public WeaponDeformation Deformated { get; }
    public void Initializer(T owner, WeaponDataEntity weaponData);
    public void WeaponModeToElement(ElementType elementType);
    public void AttackBehaviour();
    public float InputCharging(float chargeTime);
    public WeaponPower CurrentPower(float magnification = 1f);
    public void OnEquipment();
    public void OnLift();
}
