//日本語コメント可
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAVBattlePosture : EnemyBattlePosture
{
    protected override void OnUpdate()
    {
        base.OnUpdate();
        //if (Owner.Player.Value)
        //{
        //    var targetPos = Owner.Player.Value.transform.position + new Vector3(0f, 1.8f, 0f);
        //    Owner.transform.LookAt(targetPos);
        //    _distance = (targetPos - transform.position);
        //    if (_hit)
        //    {
        //        _currentSpeed = (-_speed * _moveMagnification);
        //        _time += Time.deltaTime;
        //        if (_time > 1f || !InSight())
        //        {
        //            _hit = false;
        //            _time = 0f;
        //        }
        //    }
        //    else
        //    {
        //        _currentSpeed = _speed;
        //        IHitBehavorOfAttack playerStatus = CallPlayerGauge();
        //        if (playerStatus != null)
        //        {
        //            playerStatus.BehaviorOfHit(_power, ElementType.ELEKE);
        //            _hit = true;
        //        }
        //    }
        //}
        //_rb.velocity = _distance.normalized * _currentSpeed;
    }
}
