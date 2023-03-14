//GameManagerで行う処理
//1:IsGameの判定
//2:プレイヤー及び敵キャラクターの管理
//3:マネージャー管理
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    [Tooltip("GameManagerのインスタンス")]
    static GameManager _instance = new();
    [Tooltip("SoundManagerのインスタンス")]
    static SoundManager _instanceSound = new();

    [Tooltip("ゲーム中かどうか判定する変数")]
    GameState _state = GameState.GameStart;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError($"GameManagerのインスタンスがありません");
            }
            return _instance;
        }
    }
    public static SoundManager InstanceSM
    {
        get
        {
            if (_instanceSound == null)
            {
                Debug.LogError($"SoundManagerのインスタンスがありません");
            }
            return _instanceSound;
        }
    }
    public GameState IsGame => _state;

    public delegate void DoGameState();
    public DoGameState[] _doGameStates = new DoGameState[Enum.GetNames(typeof(GameState)).Length];

    public GameManager()
    {
        Initialize();
    }
    void Initialize()
    {
        ChangedState(GameState.GameStart);
    }
    /// <summary>
    /// 引数で指定したステート時の処理を実行する関数
    /// </summary>
    /// <param name="state">遷移するステート</param>
    public void ChangedState(GameState state)
    {

        //_doGameStates[(int)state]();
        switch (state)
        {
            case GameState.GameStart:
                ChangedState(GameState.InGame);
                state = GameState.InGame;
                break;
            case GameState.InGame:
                break;
            case GameState.WaitGame:
                break;
            case GameState.GameClear:
                break;
        }
        _state = state;
    }

}

public enum GameState
{
    GameStart = 0,
    InGame = 1,
    WaitGame = 2,
    GameClear = 3,
}
