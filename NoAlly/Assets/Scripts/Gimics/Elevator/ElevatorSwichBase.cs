using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSwichBase : IHit
{
    [SerializeField] Elevator _targetElevator = null;

    ElementType _type = ElementType.RIGIT;

    public void BehaviorOfHit(ElementType type)
    {
        switch(type)
        {
            case ElementType.ELEKE:
                break;
            default:
                break;
        }
    }
}
