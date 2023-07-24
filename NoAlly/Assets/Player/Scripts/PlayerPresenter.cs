//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    [SerializeField, Header("")]
    PlayerBehaviorController _inputToPlayer = null;
    [SerializeField, Header("")]
    PlayerBehaviorController _playerBehaviorController = null;

    void Start()
    {
        
    }
}
