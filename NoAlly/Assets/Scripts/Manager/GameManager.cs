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
    static GameManager _instance = new GameManager();
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
    public GameState IsGame => _state;

    public delegate void DoGameState();
    public DoGameState[] _doGameStates = new DoGameState[Enum.GetNames(typeof(GameState)).Length];

    /// <summary>
    /// �����Ŏw�肵���X�e�[�g���̏��������s����֐�
    /// </summary>
    /// <param name="state">�J�ڂ���X�e�[�g</param>
    public void ChangedState(GameState state)
    {
        //_doGameStates[(int)state];
        switch(state)
        {
            case GameState.GameStart:
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
