using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract@class EnemyBase : MonoBehaviour,IObjectPool
{
    [SerializeField] protected Animator m_enemyAnimator;
    [SerializeField] Renderer m_enemyRenderer = default;
    protected bool m_sightIn = false;
    bool m_isActive = false;

    public bool IsActive { get => m_isActive; set => m_isActive = value; }
    public abstract void EnemyAttack();
    public virtual void Start() { }
    public virtual void Update() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerContoller player))
        {
            m_sightIn = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerContoller player))
        {
            m_sightIn = false;
        }
    }
    //AnimationEvent‚ÅŒÄ‚Ô
    

    //¶¬‚Æ”j‰ó‚Ìˆ—
    public void Create()
    {
        m_enemyRenderer.enabled = true;
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
        m_enemyRenderer.enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }
}
