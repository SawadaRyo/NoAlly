using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManeuverDateEntity
{
    public WeaponType SelectType;
    public int TargetId;
    public string Name;
    public int Level;
    public float AttackMagnification;
}

public enum WeaponType
{
    Sword = 0,
    Lance = 1
}

