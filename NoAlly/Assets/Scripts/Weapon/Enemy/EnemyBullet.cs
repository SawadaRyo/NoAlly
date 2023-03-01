using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : ObjectBase, IObjectPool<GunTypeEnemy>,IBullet
{
    public GunTypeEnemy Owner { get; set; }

    public void Create()
    {
        throw new System.NotImplementedException();
    }

    public void Disactive()
    {
        throw new System.NotImplementedException();
    }

    public void Disactive(float interval)
    {
        throw new System.NotImplementedException();
    }

    public void DisactiveForInstantiate(GunTypeEnemy owner)
    {
        throw new System.NotImplementedException();
    }

    public void HitMovement(Collider target)
    {
        throw new System.NotImplementedException();
    }
}
