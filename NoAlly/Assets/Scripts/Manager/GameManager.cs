//GameManager�ōs������
//1:IsGame�̔���
//2:�v���C���[�y�ѓG�L�����N�^�[�̊Ǘ�
//3:�}�l�[�W���[�Ǘ�
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    bool _isGame = false; //�Q�[�������ǂ������肷��ϐ�

    public bool IsGame => _isGame;

    void Start()
    {
        _isGame = true;
    }
}
