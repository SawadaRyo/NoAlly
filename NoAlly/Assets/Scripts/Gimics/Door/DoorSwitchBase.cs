using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class DoorSwitchBase : MonoBehaviour
{

    protected BoolReactiveProperty _isLock = null;

    public IReadOnlyReactiveProperty<bool> IsLock => _isLock;

    public void Initalizer()
    {
        _isLock = new BoolReactiveProperty(false);
    }
    public virtual void ObjectAction() { }
    protected virtual void DoorLock()
    {
        _isLock.Value = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WeaponBase playerWeapon))
        {
            if (playerWeapon.Owner == HitOwner.Player)
            {
                DoorLock();
            }
        }
    }
}
