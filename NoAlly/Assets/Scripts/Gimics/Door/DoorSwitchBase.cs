using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoorSwitchBase : MonoBehaviour
{
    Door _parentDoor = null;

    public abstract void ObjectAction();

    public virtual void SetUp(Door door)
    {
        _parentDoor = door;
    }
    void FixedUpdate()
    {
        ObjectAction();
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
