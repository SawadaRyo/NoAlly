using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour,IObjectGenerator
{
    [SerializeField] Transform _poolPos = null;
    [SerializeField] EnemyBase _prefabBases = null;
    [SerializeField] EnemyType _enemyType;

    int _size = 1;
    Transform[] _enmeyPositions;
    ObjectPool<EnemyBase> _enemyiesPool = new ObjectPool<EnemyBase>();

    public Transform GenerateTrance => _poolPos;

    public void Start()
    {
        _enmeyPositions = gameObject.GetComponentsInChildren<Transform>();
        _enemyiesPool.SetBaseObj(_prefabBases, _poolPos, (int)_enemyType);
        _enemyiesPool.SetCapacity(this,_enmeyPositions.Length);
        EnemyGenerate();
    }
    public void EnemyGenerate()
    {
        foreach (Transform enmeyPosition in _enmeyPositions)
        {
            EnemyBase enmey = _enemyiesPool.Instantiate((int)_enemyType);
            enmey.transform.position = enmeyPosition.position;
        }
    }
}

