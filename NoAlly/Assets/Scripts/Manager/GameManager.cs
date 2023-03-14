//GameManager�ōs������
//1:IsGame�̔���
//2:�v���C���[�y�ѓG�L�����N�^�[�̊Ǘ�
//3:�}�l�[�W���[�Ǘ�
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    [Tooltip("GameManager�̃C���X�^���X")]
    static GameManager _instance = new();
    [Tooltip("SoundManager�̃C���X�^���X")]
    static SoundManager _instanceSound = new();

    [Tooltip("�Q�[�������ǂ������肷��ϐ�")]
    GameState _state = GameState.GameStart;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError($"GameManager�̃C���X�^���X������܂���");
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
                Debug.LogError($"SoundManager�̃C���X�^���X������܂���");
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
    /// �����Ŏw�肵���X�e�[�g���̏��������s����֐�
    /// </summary>
    /// <param name="state">�J�ڂ���X�e�[�g</param>
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
