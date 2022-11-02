using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [SerializeField] DoorSwitchBase[] _locks = null;
    Animator _animator = null;
    int _remainLock = 0;
    bool _isLock = false;
    public bool IsLock => _isLock;

    void Start()
    {
        foreach(var dsb in _locks)
        {
            dsb.SetUp(this);
        }
        _animator = GetComponent<Animator>();
        _remainLock = _locks.Length;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerContoller p))
        {
            DoorState(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerContoller p))
        {
            DoorState(false);
        }
    }

    void DoorState(bool doorLock)
    {
        _animator.SetBool("DoorLock", doorLock);
    }
}
