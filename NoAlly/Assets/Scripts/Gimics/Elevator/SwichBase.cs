using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwichBase<T> : IHitBehavorOfGimic where T : IGimic
{
    protected T _swichOwner;
    ElementType _typeOfActive = ElementType.RIGIT;

    public void BehaviorOfHit(ElementType type)
    {
        
    }
}
