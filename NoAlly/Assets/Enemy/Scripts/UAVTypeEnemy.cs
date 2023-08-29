using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UAVTypeEnemy : EnemyBase
{
    [SerializeField, Tooltip("UŒ‚—Í")]
    float[] _power = new float[4] { 0, 0, 3, 0 };
    [SerializeField, Tooltip("‘¬“x")]
    float _speed = 2;
    [SerializeField, Tooltip("‰Á‘¬“x")]
    float _moveMagnification = 2f;
    [SerializeField, Tooltip("UŒ‚”ÍˆÍ")]
    float _attackRadius = 1.6f;
    [SerializeField, Tooltip("UŒ‚”ÍˆÍ‚Ì’†S")]
    Transform _attackPos = default;
    [SerializeField, Tooltip("")]
    LayerMask _fieldLayer = ~0;

    [Tooltip("")]
    Vector3 _distance = Vector2.zero;
    [Tooltip("")]
    bool _hit = false;
    [Tooltip("")]
    float _currentSpeed = 0f;
    float _time = 0f;

    public override void EnemyAttack()
    {
        if (InSight())
        {
            var targetPos = Player.Value.transform.position + new Vector3(0f, 1.8f, 0f);
            transform.LookAt(targetPos);
            _distance = (targetPos - transform.position);
            if (_hit)
            {
                _currentSpeed = (-_speed * _moveMagnification);
                _time += Time.deltaTime;
                if (_time > 1f || !InSight())
                {
                    _hit = false;
                    _time = 0f;
                }
            }
            else
            {
                _currentSpeed = _speed;
                IHitBehavorOfAttack playerStatus = CallPlayerGauge();
                if (playerStatus != null)
                {
                    playerStatus.BehaviorOfHit(_power, ElementType.ELEKE);
                    _hit = true;
                }
            }
        }
        _rb.velocity = _distance.normalized * _currentSpeed;
    }

    public override void ExitAttackState()
    {
        base.ExitAttackState();
        _hit = false;
        _currentSpeed = 0f;
        _time = 0f;
        _rb.velocity = _distance.normalized * _currentSpeed;
    }

    IHitBehavorOfAttack CallPlayerGauge()
    {
        Collider[] playerCol = Physics.OverlapSphere(_attackPos.position, _attackRadius, _playerLayer);
        foreach (Collider col in playerCol)
        {
            if (col.TryGetComponent(out IHitBehavorOfAttack playerGauge))
            {
                return playerGauge;
            }
        }
        return null;
    }

    public override void Disactive()
    {
        _rb.velocity = Vector3.zero;
        _rb.isKinematic = true;
    }
    //public override void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(_attackPos.position, _attackRadius);
    //}
}
