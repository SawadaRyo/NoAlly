using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UAVTypeEnemy : EnemyBase
{
    [SerializeField,Tooltip("")] 
    float _power = 3;
    [SerializeField, Tooltip("")] 
    float _speed = 2;
    [SerializeField, Tooltip("")] 
    float _moveMagnification = 2f;
    [SerializeField, Tooltip("")] 
    float _attackRadius = 1.6f;
    [SerializeField, Tooltip("")] 
    Transform _attackPos = default;
    [SerializeField, Tooltip("")] 
    LayerMask _fieldLayer = ~0;

    [Tooltip("")]
    Rigidbody _rb = default;
    [Tooltip("")]
    Vector3 _distance = Vector2.zero;
    [Tooltip("")]
    bool _hit = false;
    [Tooltip("")]
    float _currentSpeed = 0f;

    public override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
    }
    public override void EnemyAttack()
    {
        if (InSight())
        {
            PlayerStatus player = InSight();
            var targetPos = player.transform.position + new Vector3(0f, 1.8f, 0f);
            transform.LookAt(targetPos);
            _distance = (targetPos - transform.position);
            if (_hit)
            {
                _currentSpeed = (-_speed * _moveMagnification);
                float time = Time.deltaTime;
                if (time > 1f)
                {
                    _hit = false;
                }
            }
            else
            {
                _currentSpeed = _speed;
                IHitBehavorOfAttack playerStatus = CallPlayerGauge();
                if (playerStatus != null)
                {
                    //playerStatus.BehaviorOfHit(,MainMenu.Instance.Type);
                    _hit = true;
                }
            }
            _enemyAnimator.SetBool("InSight",InSight());
        }
        else
        {
            _distance = Vector3.zero;
        }
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
        base.Disactive();
        _rb.velocity = Vector3.zero;
    }
    //public override void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(_attackPos.position, _attackRadius);
    //}
}
