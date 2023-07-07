using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class DoorTypeContact : DoorBase
{
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
}
