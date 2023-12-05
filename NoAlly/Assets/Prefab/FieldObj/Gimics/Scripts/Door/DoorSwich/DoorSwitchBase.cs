using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class DoorSwitchBase : MonoBehaviour, IHitBehavorOfGimic
{
    [SerializeField]
    ObjectOwner _owner;
    [SerializeField]
    Color[] _colors;


    protected BoolReactiveProperty _isLock = null;

    public IReadOnlyReactiveProperty<bool> IsLock => _isLock;

    public virtual void ObjectAction() { }

    public void BehaviorOfHit<T>(T weaponBase, ElementType type) where T : WeaponBase
    {
        DoorLock();
    }
    public void Initalize()
    {
        _isLock = new BoolReactiveProperty(false);
    }
    protected virtual void DoorLock()
    {
        _isLock.Value = true;
    }
}
