using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public abstract class EnemyBase : MonoBehaviour, IObjectPool
{
    [SerializeField] protected float _radius = 5f;
    [SerializeField] protected Animator _enemyAnimator;
    [SerializeField] protected LayerMask _playerLayer = ~0;
    [SerializeField] protected Transform _center = default;
    [SerializeField] Renderer[] _enemyRenderer = default;
    bool _isActive = false;
    RaycastHit _hitInfo;

    public bool IsActive { get => _isActive; set => _isActive = value; }
    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnTriggerExit(Collider other) { }
    public abstract void EnemyAttack();
    public virtual void Start() 
    {
        
    }
    public void FixedUpdate()
    {
        EnemyAttack();
    }

    public bool InSight()
    {
        //_center = transform.position;
        var inSight = Physics.OverlapSphere(_center.position, _radius, _playerLayer);
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
        foreach (var r in _enemyRenderer)
        {
            r.enabled = true;
        }
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        _enemyAnimator.SetBool("Death", false);
        _enemyAnimator.Play("RifleIdle");
    }
    public void Disactive()
    {
        _enemyAnimator.SetBool("Death", true);
    }
    public void DisactiveForInstantiate()
    {
        foreach (var r in _enemyRenderer)
        {
            r.enabled = false;
        }
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_center.position, _radius);
    }
    public void RendererActive(bool stats)
    {
        foreach(var r in _enemyRenderer)
        {
            r.enabled = stats;
        }
    }
}
