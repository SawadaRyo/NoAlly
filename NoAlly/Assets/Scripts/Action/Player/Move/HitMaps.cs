//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class HitMaps
{
    static public StateOfPlayer HitObjMapToWall(bool[] isPlayerPart) => _hitObjMapToWall[isPlayerPart];

    static Dictionary<bool[], StateOfPlayer> _hitObjMapToWall = new()
    {   
        //壁に掴まっている
        {new bool[] { true,true,true },StateOfPlayer.GripingWall},//全ての部位が当たっている
        {new bool[] { true,true,false },StateOfPlayer.GripingWall},//頭、胴が当たっている
        {new bool[] { false,true,false },StateOfPlayer.GripingWall},//胴のみが当たっている

        //よじ登っている
        {new bool[] { false,true,true },StateOfPlayer.GripingWallEdge},//胴、足が当たっている

        //足をかけて登っている
        {new bool[]{ false,false,true },StateOfPlayer.HangingWallEgde}//足のみ当たっている
    };
}
