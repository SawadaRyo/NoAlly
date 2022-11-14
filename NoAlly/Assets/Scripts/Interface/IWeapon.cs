using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface IWeapon
{
    public bool Operated { get; }
    public bool Deformated { get; }
    public void WeaponMovement();
    public void WeaponMovement(Collider target);
    public void WeaponMode(ElementType type);
}
