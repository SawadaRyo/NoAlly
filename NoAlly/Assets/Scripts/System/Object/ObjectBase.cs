using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

/// <summary>
/// オブジェクトの表示を切り替えるクラス
/// </summary>
public class ObjectBase : MonoBehaviour
{
    [SerializeField, Header("オブジェクトのCollider")]
    protected Collider[] _objectCollider = null;
    [SerializeField, Header("オブジェクトのRenderer")]
    protected Renderer[] _objectRenderer = null;
    [SerializeField, Header("オブジェクトのAnimator")]
    protected Animator _objectAnimator = null;

    protected bool _isActive = false;

    public Animator ObjectAnimator => _objectAnimator;

    public bool IsActive => _isActive;

    public virtual void ActiveObject(bool stats)
    {
        if (_objectAnimator != null)
        {
            _objectAnimator.enabled = stats;
        }
        Array.ForEach(_objectRenderer, x => x.enabled = stats);
        Array.ForEach(_objectCollider, x => x.enabled = stats);
    }
    public virtual void ActiveCollider(bool stats)
    {
        Array.ForEach(_objectCollider, x => x.enabled = stats);
    }
}
