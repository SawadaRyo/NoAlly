using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

public class Elevator : MonoBehaviour
{
    [SerializeField] bool _rotObj = false;
    [SerializeField] float _timeForPoint = 3f;
    [SerializeField] Transform _movePos1;
    [SerializeField] Transform _movePos2;
    [SerializeField] Text _rideText;

    [Tooltip("プレイヤーの搭乗判定")]
    bool _riding = false;
    [Tooltip("エレベーターの起動判定")]
    bool _moving = false;
    [Tooltip("エレベーターのAnimator")]
    Animator _animator;
    [Tooltip("プレイヤー")]
    PlayerContoller _playerContoller = default;
    [Tooltip("プレイヤー")]
    Rigidbody _playerRb = default;
    [Tooltip("Animationの遷移状況")]
    ObservableStateMachineTrigger _trigger = default;


    // Start is called before the first frame update
    void Start()
    {
        _rideText.enabled = false;
        _animator = GetComponent<Animator>();
        _animator.SetBool("Open", true);
        _trigger = _animator.GetBehaviour<ObservableStateMachineTrigger>();
        PodMotion();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerContoller>(out PlayerContoller player))
        {
            _rideText.enabled = true;
            _playerContoller = player;
            _playerRb = player.Rb;
        }
        WeaponEquipment.Instance.AvailableWeapon(false);
    }
    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<PlayerContoller>(out PlayerContoller player))
        {
            if (Input.GetButtonDown("Decision") && player)
            {
                _animator.SetBool("Open", false);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerContoller>(out PlayerContoller player))
        {
            _rideText.enabled = false;
            _playerContoller = null;
            _playerRb = null;
        }
        WeaponEquipment.Instance.AvailableWeapon(true);
    }


    //void RideOnPod(bool activeElevator)
    //{
    //    if (activeElevator)
    //    {
    //        _animator.SetBool("Open", false);
    //        PlayerContoller.Instance.transform.parent = gameObject.transform;
    //        _playerRb.constraints = RigidbodyConstraints.None;
    //    }
    //    else
    //    {
    //        _animator.SetBool("Open", true);
    //        PlayerContoller.Instance.transform.parent = null;
    //        _playerRb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    //        (_movePos1, _movePos2) = (_movePos2, _movePos1);
    //    }
    //}
    void PodMotion()
    {
        IDisposable exitState = _trigger
        .OnStateExitAsObservable()
        .Subscribe(onStateInfo =>
        {
            AnimatorStateInfo info = onStateInfo.StateInfo;
            if (info.IsName("ElevatorClose"))
            {
                if (_playerContoller)
                {
                    _moving = true;
                    _playerContoller.transform.parent = this.gameObject.transform;
                    _playerRb.constraints = RigidbodyConstraints.None;
                    if(_rotObj)
                    {
                        this.transform.DORotate(new Vector3(0f, 180f, 0f), _timeForPoint, RotateMode.LocalAxisAdd);
                    }
                    DOTween.To(() => _movePos1.position,
                        x => transform.position = x,
                        _movePos2.position, _timeForPoint)
                        .OnComplete(() => _animator.SetBool("Open", true));
                }
            }
            else if (info.IsName("ElevatorOpen"))
            {
                if (_playerContoller)
                {
                    _moving = false;
                    _playerContoller.transform.parent = null;
                    _playerRb.constraints = RigidbodyConstraints.FreezePositionZ
                                          | RigidbodyConstraints.FreezeRotation;
                    (_movePos1, _movePos2) = (_movePos2, _movePos1);
                }
            }
        }).AddTo(this);
    }

}
