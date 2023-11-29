//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamaterController : MonoBehaviour
{
    [SerializeField, Header("プレイヤーの挙動に関するパラメーター")]
    ActorParamater _playerParamater = null;

    [Tooltip("")]
    ActorParamater _beforeParamater = null;

    public ActorParamater GetParamater => _playerParamater;

    public void Initializer()
    {
        _beforeParamater = new ActorParamater(_playerParamater);
    }

    public void ChangeMoveSpeed(float magnification = 1f)
    {
        _playerParamater.speed = _beforeParamater.speed * magnification;
    }
}
