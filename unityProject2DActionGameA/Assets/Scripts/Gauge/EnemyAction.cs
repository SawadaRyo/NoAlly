using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    [SerializeField] float m_distance = 5f;
    [SerializeField] LayerMask m_playerMask = ~0;
    [SerializeField] Animator m_enemyAnimator;

    Transform m_muzulPos;
    RaycastHit hit;
    bool m_sightIn = false;

 
    void Start()
    {
        hit = new RaycastHit();
        m_muzulPos = gameObject.transform;
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
    public void EnemyFire()
    {
        Ray ray = new Ray(m_muzulPos.position, Vector3.forward);
        bool hitFlg = Physics.Raycast(ray, out hit, m_distance, m_playerMask);
        if (hitFlg)
        {
            Debug.Log(hit.collider.gameObject.tag);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, m_distance);
        }
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
