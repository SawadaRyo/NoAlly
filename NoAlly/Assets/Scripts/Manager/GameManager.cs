//GameManagerで行う処理
//1:IsGameの判定
//2:プレイヤー及び敵キャラクターの管理
//3:マネージャー管理
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    bool _isGame = false; //ゲーム中かどうか判定する変数

    public bool IsGame => _isGame;

    void Start()
    {
        _isGame = true;
    }
}
