//GameManager�ōs������
//1:IsGame�̔���
//2:�v���C���[�y�ѓG�L�����N�^�[�̊Ǘ�
//3:�J�����̎��i�Ȃ��݂̂ɃI�u�W�F�N�g����
//4:�V�[���J��
//5:�}�l�[�W���[�Ǘ�
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    bool m_isGame = false; //�Q�[�������ǂ������肷��ϐ�

    public bool IsGame { get => m_isGame; set => m_isGame = value; }
void Start()
    {
        IsGame = true;
    }

    void Update()
    {
        
    }
}
