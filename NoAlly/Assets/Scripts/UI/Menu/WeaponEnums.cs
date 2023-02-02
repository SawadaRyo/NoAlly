using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CommandType
{
    MAIN = 0,
    SUB = 1,
    ElEMENT = 2
}
public enum WeaponType
{
    NONE = -1,
    SWORD = 0,
    LANCE = 1,
    BOW = 2,
    BRASTER = 3,
}
public enum ElementType
{
    RIGIT = 0,
    FIRE = 1,
    ELEKE = 2,
    FROZEN = 3
}
public enum HitOwner
{
    NONE = -1,
    Player = 0,
    Enemy = 1,
    Item = 2
}

public enum WeaponDeformation
{
    NONE = 0,
    Deformation = 1
}

public enum PlayerClimbWall
{
    NONE,
    RIGHT,
    LEFT,
}


