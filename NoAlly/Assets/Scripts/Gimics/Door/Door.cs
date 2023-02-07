using System;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [SerializeField] DoorSwitchBase[] _locks = null;

    bool _locked = false;
    int _lockCount = 0;
    Animator _animator = null;

    void Start()
    {
        Array.ForEach(_locks, x => x.Initalizer());
        _lockCount = _locks.Length;
        _animator = GetComponent<Animator>();
        DoorState();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerContoller>(out _))
        {
            DoorState(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerContoller>(out _))
        {
            DoorState(false);
        }
    }

    void DoorState(bool doorLock)
    {
        {
            _animator.SetBool("DoorLock", doorLock);
        }
    }

    void DoorState()
    {
        for(int i = 0;i < _lockCount;i++)
        {
            _locks[i].IsLock
                .Subscribe(x =>
                {
                    _lockCount--;
                    if(_lockCount <= 0)
                    {
                        _locked = true;
                    }
                });
        }
    }
}
