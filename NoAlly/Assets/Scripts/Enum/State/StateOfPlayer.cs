//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateOfPlayer
{
    None,

    Idle,
    Attack,
    Move,
    Dash,
    Death,

    OnGround,
    GripingWall,
    GripingWallEdge,
    HangingWallEgde,
    InAir,

    Right,
    Left,
    Up,
    Down,
}

public enum ActorVec
{
    None,
    Horizontal,
    Right,
    Left,
    Up,
    Down,
}

public enum PlayerWallState
{
    None = -1,
    Griping = 0,
    GripingEdge = 1,
    HangingEgde = 2
}

public enum PlayerPart:int
{
    Head,
    Chast,
    Foot
}
