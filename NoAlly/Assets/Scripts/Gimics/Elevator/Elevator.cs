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

    
    [Tooltip("エレベーターの起動判定")]
    bool _moving = false;
    [Tooltip("エレベーターのAnimator")]
    Animator _animator;
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
            _playerRb = player.GetComponent<Rigidbody>();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<PlayerContoller>(out PlayerContoller equipment))
        {
            if (Input.GetButtonDown("Decision") && equipment)
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
            _playerRb = null;
        }
    }

    void PodMotion()
    {
        IDisposable exitState = _trigger
        .OnStateExitAsObservable()
        .Subscribe(onStateInfo =>
        {
            AnimatorStateInfo info = onStateInfo.StateInfo;
            if (info.IsName("ElevatorClose"))
            {
                if (_playerRb != null)
                {
                    _moving = true;
                    _playerRb.transform.parent = this.gameObject.transform;
                    _playerRb.constraints = RigidbodyConstraints.None;
                    if (_rotObj)
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
                _moving = false;
                _playerRb.transform.parent = null;
                _playerRb.constraints = RigidbodyConstraints.FreezePositionZ
                                      | RigidbodyConstraints.FreezeRotation;
                (_movePos1, _movePos2) = (_movePos2, _movePos1);
            }
        }).AddTo(this);
    }
    public void SetPosition(Transform pos1, Transform pos2)
    {
        _movePos1 = pos1;
        _movePos2 = pos2;
    }
}
public enum ElevatorState
{
    Normal = 0,
    Locked = 1,
    Othor = 2,
}
