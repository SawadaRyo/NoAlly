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
        var targetPos = PlayerContoller.Instance.transform.position + new Vector3(0f, 1.8f, 0f);
        transform.LookAt(targetPos);

        if (InSight())
        {
            _distance = (targetPos - transform.position);
            if (_hit)
            {
                _isSpeed = (-_speed * _moveMagnification);
                if (_distance.magnitude > _radius - 2f)
                {
                    _hit = false;
                }
            }
            else
            {
                _isSpeed = _speed;
                PlayerGauge playerGauge = CallPlayerGauge();

                if (playerGauge != null)
                {
                    playerGauge.DamageMethod(0f, 0f, _power, 0f);
                    _hit = true;
                }
            }
        }
        else
        {
            _distance = Vector3.zero;
        }
        _rb.velocity = _distance.normalized * _isSpeed;
        //Debug.Log(_hit);
    }
    //public override void OnTriggerEnter(Collider other)
    //{
    //    base.OnTriggerEnter(other);
    //    if (other.gameObject.TryGetComponent(out PlayerGauge playerGauge))
    //    {
    //        Debug.Log('A');
    //        _hit = true;
    //        playerGauge.DamageMethod(0, 0, _power, 0);
    //    }
    //}

    PlayerGauge CallPlayerGauge()
    {
        var playerCol = Physics.OverlapSphere(_attackPos.position, _attackRadius, _playerLayer);
        foreach (var col in playerCol)
        {
            if (col.TryGetComponent<PlayerGauge>(out PlayerGauge playerGauge))
            {
                return playerGauge;
            }
        }
        return null;
    }
    //public override void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(_attackPos.position, _attackRadius);
    //}
}
