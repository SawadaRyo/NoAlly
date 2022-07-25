using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAVTypeEnemy : EnemyBase
{
    [SerializeField] float m_power = 3;
    [SerializeField] float m_speed = 2;
    [SerializeField] LayerMask m_fieldLayer = ~0;
    Rigidbody m_rb = default;
    Vector3 m_distance = Vector2.zero;
    bool m_hit = false;

    public override void Start()
    {
        base.Start();
        m_rb = GetComponent<Rigidbody>();
    }
    public override void EnemyAttack()
    {
        var targetPos = PlayerContoller.Instance.transform.position + new Vector3(0f, 1.8f, 0f);
        transform.LookAt(targetPos);

        if (InSight())
        {
            m_distance = (targetPos - transform.position);
            if (m_hit)
            {
                m_rb.velocity = m_distance.normalized * (-m_speed * 2);
                if (m_distance.magnitude > m_radius - 2f) m_hit = false;
            }
            else
            {
                m_rb.velocity = m_distance.normalized * m_speed;
            }
        }
        else
        {
            m_distance = Vector3.zero;
            Debug.Log("o");
        }
        Debug.Log(m_hit);
        Debug.Log(m_distance);
    }
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.TryGetComponent(out PlayerGauge playerGauge))
        {
            Debug.Log('A');
            m_hit = true;
            playerGauge.DamageMethod(m_power, 0, 0, 0);
        }
    }

    bool OnField()
    {
        bool hit = Physics.Raycast(m_center, m_distance, m_fieldLayer);
        return hit;
    }
}
