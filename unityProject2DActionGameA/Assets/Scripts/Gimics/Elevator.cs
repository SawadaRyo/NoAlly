using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

public class Elevator : MonoBehaviour
{
    [SerializeField] float m_timeForPoint = 3f;
    [SerializeField] Transform _movePos1;
    [SerializeField] Transform _movePos2;
    [SerializeField] Text m_rideText;
    Animator _animator;
    [Tooltip("プレイヤーのRIgitbody")]
    Rigidbody _playerRb = default;
    [Tooltip("プレイヤーの位置調整")]
    Transform _putPlayerPos;
    [Tooltip("Animationの遷移状況")]
    ObservableStateMachineTrigger _trigger = default;


    // Start is called before the first frame update
    void Start()
    {
        m_rideText.enabled = false;
        _putPlayerPos = transform.GetChild(0);
        _playerRb = PlayerContoller.Instance.gameObject.GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _trigger = _animator.GetBehaviour<ObservableStateMachineTrigger>();
    }


    void OnTriggerEnter(Collider other)
    {
        m_rideText.enabled = true;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerContoller>(out PlayerContoller player))
        {
            MovePoint();
        }
    }
    void OnTriggerExit(Collider other)
    {
        m_rideText.enabled = false;
        //m_player = null;
        //if (m_podType == PodType.ElvaterType)
        //{
        //    MovePoint();
        //}
    }

    void MovePoint()
    {
        DOTween.To(() => _movePos1.position,
            x => transform.position = x,
            _movePos2.position, m_timeForPoint)
            .OnStart(() => RideOnPod(false))
            .OnComplete(() => RideOnPod(true));
    }

    void RideOnPod(bool activeUnityChan)
    {
        if (!activeUnityChan)
        {
            _animator.SetBool("Lock", true);
            PlayerContoller.Instance.transform.position = _putPlayerPos.position;
            _playerRb.constraints =
                RigidbodyConstraints.FreezePositionX |
                RigidbodyConstraints.FreezePositionY;
            PlayerContoller.Instance.transform.parent = gameObject.transform;
        }
        else
        {
            _animator.SetBool("Lock", false);
            _playerRb.constraints = RigidbodyConstraints.None;
            _playerRb.constraints = RigidbodyConstraints.FreezePositionZ |
                RigidbodyConstraints.FreezeRotation;
            (_movePos1, _movePos2) = (_movePos2, _movePos1);
        }
    }

    enum PodType
    {
        LiftType, ElvaterType
    }
}
