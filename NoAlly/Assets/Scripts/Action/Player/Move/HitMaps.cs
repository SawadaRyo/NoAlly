//日本語コメント可
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static public class HitMaps
{

    static Dictionary<PlayerPartValues, StateOfPlayer> _hitObjMapToWall = new()
    {   
        //壁に掴まっている
        {new PlayerPartValues ( true,true,true ),StateOfPlayer.GripingWall},//全ての部位が当たっている
        {new PlayerPartValues ( true,true,false ),StateOfPlayer.GripingWall},//頭、胴が当たっている
        {new PlayerPartValues ( false,true,false ),StateOfPlayer.GripingWall},//胴のみが当たっている

        //よじ登っている
        {new PlayerPartValues ( false,true,true ),StateOfPlayer.GripingWallEdge},//胴、足が当たっている

        //足をかけて登っている
        {new PlayerPartValues( false,false,true ),StateOfPlayer.HangingWallEgde}//足のみ当たっている
    };


    static public StateOfPlayer HitObjMapToWall(bool[] isPlayerPart)
    {
        Debug.Log($"{isPlayerPart[0]},{isPlayerPart[1]},{isPlayerPart[2]}");
        if (_hitObjMapToWall.TryGetValue((PlayerPartValues)isPlayerPart, out StateOfPlayer state))
        {
            return state;
        }
        return StateOfPlayer.None;
    }
}

public struct PlayerPartValues
{
    public bool head;
    public bool body;
    public bool foot;

    public PlayerPartValues(bool head, bool body, bool foot)
    {
        this.head = head;
        this.body = body;
        this.foot = foot;
    }

    public static implicit operator PlayerPartValues(bool[] val)
    {
        return new PlayerPartValues(val[0], val[1], val[2]);
    }
}



