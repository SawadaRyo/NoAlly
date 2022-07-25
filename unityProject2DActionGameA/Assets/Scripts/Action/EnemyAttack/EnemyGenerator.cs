using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] int m_size = 5;
    EnemyBase[] m_prefabBases = default;
    ObjectPool<EnemyBase>[] m_enemyiesPool = default;
    // Start is called before the first frame update
    void Start()
    {
        m_prefabBases = Resources.LoadAll<EnemyBase>("Enemy");
        m_enemyiesPool = new ObjectPool<EnemyBase>[m_prefabBases.Length];
        for(int i = 0;i < m_enemyiesPool.Length; i++)
        {
            var enmeyPos = gameObject.transform.GetChild(i);
            m_enemyiesPool[i].SetBaseObj(m_prefabBases[i],enmeyPos);
            m_enemyiesPool[i].SetCapacity(m_size);
        }
    }

    private void FixedUpdate()
    {
        
    }
}
