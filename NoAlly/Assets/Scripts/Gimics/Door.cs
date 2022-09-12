using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [SerializeField] DoorSwitchBase[] _locks = default;
    Material _doorLight = default;
    Animator _animator = default;
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
        _doorLight = transform.GetChild(0).GetComponent<Renderer>().materials[1];
        _remainLock = _locks.Length;
        UnLock();
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
        if (!_isLock) return;
        _animator.SetBool("DoorLock", doorLock);
    }

    public void UnLock()
    {
        _remainLock--;
        if (_remainLock <= 0)
        {
            _isLock = true;
            _doorLight.SetColor("Emission", Color.green);
        }
    }
}
