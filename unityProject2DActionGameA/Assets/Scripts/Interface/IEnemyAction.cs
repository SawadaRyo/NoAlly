using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAction 
{
    Transform AttackPos { get; }
    public void EnemyAttack();
}
