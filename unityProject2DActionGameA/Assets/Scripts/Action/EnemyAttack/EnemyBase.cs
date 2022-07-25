using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public abstract class EnemyBase : MonoBehaviour, IObjectPool
{
    [SerializeField] protected float m_radius = 5f;
    [SerializeField] protected Animator m_enemyAnimator;
    [SerializeField] Renderer[] m_enemyRenderer = default;
    [SerializeField] LayerMask m_playerLayer = ~0;
    bool m_isActive = false;
    protected Vector3 m_center = Vector3.zero;
    RaycastHit m_hitInfo;

    public bool IsActive { get => m_isActive; set => m_isActive = value; }
    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnTriggerExit(Collider other) { }
    public abstract void EnemyAttack();
    public virtual void Start() { }
    public void FixedUpdate()
    {
        m_center = transform.position;
        EnemyAttack();
    }

    public bool InSight()
    {
        Ray ray = new Ray(m_center, Vector3.forward);
        var inSight = Physics.OverlapSphere(m_center, m_radius, m_playerLayer);
        foreach (var s in inSight)
        {
            var player = s.gameObject.GetComponent<PlayerContoller>();
            if (player)
            {
                return true;
            }
        }
        return false;

    }

    //ê∂ê¨Ç∆îjâÛéûÇÃèàóù
    public void Create()
    {
        foreach (var r in m_enemyRenderer)
        {
            r.enabled = true;
        }
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        m_enemyAnimator.SetBool("Death", false);
        m_enemyAnimator.Play("RifleIdle");
    }

    public void Disactive()
    {
        m_enemyAnimator.SetBool("Death", true);
    }

    public void DisactiveForInstantiate()
    {
        foreach (var r in m_enemyRenderer)
        {
            r.enabled = false;
        }
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_center, m_radius);
    }
}
