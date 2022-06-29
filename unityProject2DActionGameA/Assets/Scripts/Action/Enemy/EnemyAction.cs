using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour, IEnemyAction
{
    [SerializeField] GameObject m_bulletPrefab;
    [SerializeField] Animator m_enemyAnimator;
    [SerializeField] Transform m_attackPos;
    bool m_sightIn = false;
    float m_interval = 2f;

    public Transform AttackPos => m_attackPos;


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
    //AnimationEventÇ≈åƒÇ‘
    public void EnemyAttack()
    {
        var bullletObj = Instantiate(m_bulletPrefab, m_attackPos.position, Quaternion.Euler(0f, 0f, 90f)) as GameObject;
    }
    IEnumerator RapidFire()
    {
        var wait = new WaitForSeconds(m_interval);
        while (m_sightIn)
        {
            m_enemyAnimator.SetTrigger("Fire");
            yield return wait;
        }
    }

    //ê∂ê¨Ç∆îjâÛéûÇÃèàóù
    public void Create()
    {

    }

    public void Death()
    {
        throw new System.NotImplementedException();
    }
}
