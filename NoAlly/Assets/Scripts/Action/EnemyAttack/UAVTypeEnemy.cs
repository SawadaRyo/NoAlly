using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UAVTypeEnemy : EnemyBase
{
    [SerializeField] float _power = 3;
    [SerializeField] float _speed = 2;
    [SerializeField] float _moveMagnification = 2f;
    [SerializeField] float _attackRadius = 1.6f;
    [SerializeField] Transform _attackPos = default;
    [SerializeField] LayerMask _fieldLayer = ~0;
    Rigidbody _rb = default;
    Vector3 _distance = Vector2.zero;
    bool _hit = false;
    float _isSpeed = 0f;

    public override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
    }
    public override void EnemyAttack()
    {
        PlayerContoller player = InSight();
        if (player)
        {
            var targetPos = player.transform.position + new Vector3(0f, 1.8f, 0f);
            transform.LookAt(targetPos);
            _distance = (targetPos - transform.position);
            if (_hit)
            {
                _isSpeed = (-_speed * _moveMagnification);
                float time = Time.deltaTime;
                if (time > 1f)
                {
                    _hit = false;
                }
            }
            else
            {
                _isSpeed = _speed;
                PlayerStatus playerGauge = CallPlayerGauge();

                if (playerGauge != null)
                {
                    playerGauge.DamageMethod(0f, 0f, _power, 0f);
                    _hit = true;
                }
            }
            _enemyAnimator.SetBool("InSight",InSight());
        }
        else
        {
            _distance = Vector3.zero;
        }
        _rb.velocity = _distance.normalized * _isSpeed;
    }

    PlayerStatus CallPlayerGauge()
    {
        var playerCol = Physics.OverlapSphere(_attackPos.position, _attackRadius, _playerLayer);
        foreach (var col in playerCol)
        {
            if (col.TryGetComponent<PlayerStatus>(out PlayerStatus playerGauge))
            {
                return playerGauge;
            }
        }
        return null;
    }

    public override void Disactive()
    {
        base.Disactive();
        RendererActive(false);
        _rb.velocity = Vector3.zero;
    }
    //public override void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(_attackPos.position, _attackRadius);
    //}
}
