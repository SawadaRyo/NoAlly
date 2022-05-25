//GameManagerで行う処理
//1:IsGameの判定
//2:プレイヤー及び敵キャラクターの管理
//3:カメラの資格ないのみにオブジェクト生成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    bool m_isGame = false; //ゲーム中かどうか判定する変数

    public bool IsGame { get => m_isGame; set => m_isGame = value; }
void Start()
    {
        IsGame = true;
    }

    void Update()
    {
        
    }
}
