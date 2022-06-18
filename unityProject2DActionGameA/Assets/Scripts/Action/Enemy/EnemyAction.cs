using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour,IEnemyAction
{
    [SerializeField] float m_distance = 3f;
    [SerializeField] GameObject m_bulletPrefab;
    [SerializeField] LayerMask m_playerMask = ~0;
    [SerializeField] Animator m_enemyAnimator;
    [SerializeField] Transform m_attackPos;
    RaycastHit hit;
    bool m_sightIn = false;

    public Transform AttackPos => m_attackPos;
 
    void Start()
    {
        hit = new RaycastHit();
    }
    void Update()
    {
        m_enemyAnimator.SetBool("Aiming", m_sightIn);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_sightIn = true;
            StartCoroutine(RapidFire());
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_sightIn = false;
        }
    }
    //AnimationEvent‚ÅŒÄ‚Ô
    public void EnemyAttack()
    {
        var bullletObj = Instantiate(m_bulletPrefab, m_attackPos.position, Quaternion.Euler(0f, 0f, 90f)) as GameObject;
    }
    IEnumerator RapidFire()
    {
        while (m_sightIn)
        {
            m_enemyAnimator.SetTrigger("Fire");
            yield return new WaitForSeconds(2f);
        }
        if (!m_sightIn) yield break;
    }
}
