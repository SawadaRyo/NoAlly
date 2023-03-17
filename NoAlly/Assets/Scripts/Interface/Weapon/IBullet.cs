using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet<T> : IObjectPool<T> where T : IObjectGenerator
{
    public void HitMovement(Collider target);
}
