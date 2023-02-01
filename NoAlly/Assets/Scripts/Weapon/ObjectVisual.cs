using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g�̕\����؂�ւ���N���X
/// </summary>
public class ObjectVisual: MonoBehaviour,IObjectPool
{
    [SerializeField, Header("�I�u�W�F�N�g��Collider")]
    protected Collider[] _objectCollider = null;
    [SerializeField, Header("�I�u�W�F�N�g��Renderer")]
    protected Renderer[] _objectRenderer = null;
    [SerializeField, Header("�I�u�W�F�N�g��Animator")]
    protected Animator _objectAnimator = null;

    protected bool _isActive = false;

    public Animator ObjectAnimator => _objectAnimator;

    public bool IsActive => _isActive;

    public void ActiveObject(bool stats)
    {
        if (_objectAnimator != null)
        {
            _objectAnimator.enabled = stats;
        }
        Array.ForEach(_objectRenderer, x => x.enabled = stats);
        Array.ForEach(_objectCollider, x => x.enabled = stats);
    }

    public virtual void Create()
    {
        _isActive = true;
        ActiveObject(_isActive);
    }

    public virtual void Disactive()
    {
        _isActive = false;
        ActiveObject(_isActive);
    }

    public virtual void DisactiveForInstantiate<TOwner>(TOwner Owner) where TOwner : IObjectGenerator
    {
        _isActive = false;
        ActiveObject(_isActive);
    }
}
