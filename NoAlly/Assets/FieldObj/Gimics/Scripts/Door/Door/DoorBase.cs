using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public abstract class DoorBase : MonoBehaviour
{
    [SerializeField, Header("ƒhƒA‚ÌŒ®")]
    protected DoorSwitchBase _locks = null;

    protected BoolReactiveProperty _locked = null;
    protected bool _inGame = false;
    protected int _lockCount = 1;
    Animator _animator = null;

    void Start()
    {
        _locked = new BoolReactiveProperty(false);
        Initialize();
        _inGame = true;
    }

    protected virtual void Initialize()
    {
        if (_locks)
        {
            _locks.Initalize();
            DoorLock();
            //Array.ForEach(_locks, x => x.Initalizer());
            //_lockCount = _locks.Length;
        }
        _animator = GetComponent<Animator>();
    }

    protected void DoorState(bool doorLock)
    {
        {
            _animator.SetBool("DoorLock", doorLock);
        }
    }

    protected void DoorLock()
    {
        _locks.IsLock
            .Subscribe(x =>
            {
                if (!_inGame) return;
                _lockCount--;
                if (_lockCount <= 0)
                {
                    _locked.Value = true;
                }
            });
    }
}
