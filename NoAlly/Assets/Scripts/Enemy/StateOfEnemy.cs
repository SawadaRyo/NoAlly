using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateOfEnemy : int
{
    Idle, //
    Move, //移動
    Saerching, //プレイヤーを捜索中
    BattlePosture, //戦闘態勢
    Attack, //攻撃
    Death //死亡
}
