using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

public class Elevator : MonoBehaviour
{
    [SerializeField] float _timeForPoint = 3f;
    [SerializeField] Transform _movePos1;
    [SerializeField] Transform _movePos2;
    [SerializeField] Text _rideText;

    [Tooltip("エレベーターの起動判定")]
    bool _moving = false;
    [Tooltip("エレベーターのAnimator")]
    Animator _animator;
    [Tooltip("プレイヤーのRIgitbody")]
    Rigidbody _playerRb = default;
    //[Tooltip("Animationの遷移状況")]
    //ObservableStateMachineTrigger _trigger = default;


    // Start is called before the first frame update
    void Start()
    {
        //m_rideText.enabled = false;
        _playerRb = PlayerContoller.Instance.gameObject.GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        //_trigger = _animator.GetBehaviour<ObservableStateMachineTrigger>();
    }


    void OnTriggerEnter(Collider other)
    {
        _rideText.enabled = true;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerContoller>())
        {
            if(Input.GetButtonDown("Decision") && !_moving)
            {
                MovePoint();
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        _rideText.enabled = false;
    }

    void MovePoint()
    {
        DOTween.To(() => _movePos1.position,
            x => transform.position = x,
            _movePos2.position, _timeForPoint)
            .OnStart(() => RideOnPod(true))
            .OnComplete(() => RideOnPod(false));
    }

    void RideOnPod(bool activeElevator)
    {
        if (activeElevator)
        {
            _animator.SetBool("Open", false);
            PlayerContoller.Instance.transform.parent = gameObject.transform;
            _playerRb.constraints = RigidbodyConstraints.None;
        }
        else
        {
            _animator.SetBool("Open", true);
            PlayerContoller.Instance.transform.parent = null;
            _playerRb.constraints = RigidbodyConstraints.FreezePositionZ;
            (_movePos1, _movePos2) = (_movePos2, _movePos1);
        }
    }
}
