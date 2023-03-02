using UnityEngine;
using System.Linq;

public class EnemysSpawnContoller : MonoBehaviour,IObjectGenerator
{
    [SerializeField, Header("")]
    EnemyBase[] _enemies;

    public Transform GenerateTrance => throw new System.NotImplementedException();

    public void Start()
    {
        foreach (EnemyBase enemy in _enemies)
        {
            enemy.DisactiveForInstantiate(this);
            enemy.GetComponent<EnemyStatus>().Initialize(enemy);
            enemy.Create();
        }
    }

    public void ReSetting()
    {
        _enemies.Where(x => !x.IsActive).ToList().ForEach(x => x.Create());
    }
}





