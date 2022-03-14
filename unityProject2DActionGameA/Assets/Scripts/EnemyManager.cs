using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject m_enwmyPrefab = default;
    [SerializeField] int m_enemyHP = 10;
    Transform m_instancePositon;
    bool m_isEnemyExist = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBecameVisible()
    {
        if(m_isEnemyExist)
        {
            gameObject.SetActive(true);
        }
        else
        {
            GameObject.Instantiate(m_enwmyPrefab, m_instancePositon.position, Quaternion.identity);
        }
        
    }
    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
