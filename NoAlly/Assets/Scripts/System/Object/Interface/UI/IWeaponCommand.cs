using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponCommand : ICommandButton
{
    public WeaponType TypeOfWeapon { get; }
}
