using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public abstract class EnemyBase : MonoBehaviour, IObjectPool
{
    [SerializeField] float m_radius = 5f;
    [SerializeField] protected Animator m_enemyAnimator;
    [SerializeField] Renderer[] m_enemyRenderer = default;
    [SerializeField] LayerMask m_playerLayer = ~0;
    bool m_isActive = false;
    RaycastHit m_hitInfo;

    public bool IsActive { get => m_isActive; set => m_isActive = value; }
    public abstract void EnemyAttack();
    public virtual void Start() { }
    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnTriggerExit(Collider other) { }
    public void FixedUpdate()
    {
        EnemyAttack();
        Debug.Log(InSight());
    }

    public bool InSight()
    {
        Vector3 center = transform.position;
        Ray ray = new Ray(center, Vector3.forward);
        var inSight = Physics.OverlapSphere(center, m_radius, m_playerLayer);
        foreach(var s in inSight)
        {
            var player = s.gameObject.GetComponent<PlayerContoller>();
            if(player)
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

    public void Death()
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
        var center = transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, m_radius);
    }
}
