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

    bool _isActive = true;
    Collider _collider = default;
    RaycastHit _hitInfo;

    


    public bool IsActive { get => _isActive; set => _isActive = value; }
    public abstract void EnemyAttack();
    public virtual void Start()
    {
        _collider = GetComponent<Collider>();
    }
    public void FixedUpdate()
    {
        if (_isActive)
        {
            EnemyAttack();
        }
    }
    public virtual void OnTriggerEnter(Collider other) { }
    public virtual void OnTriggerExit(Collider other) { }

    public PlayerContoller InSight()
    {
        //_center = transform.position;
        Collider[] inSight = Physics.OverlapSphere(_center.position, _radius, _playerLayer);
        foreach (var s in inSight)
        {
            if (s.gameObject.TryGetComponent<PlayerContoller>(out PlayerContoller player))
            {
                return player;
            }
        }
        return null;

    }

    //ê∂ê¨Ç∆îjâÛéûÇÃèàóù
    public virtual void Create()
    {
        _isActive = true;
        _collider.enabled = true;
        RendererActive(true);
        _enemyAnimator.SetBool("Death", false);
        _enemyAnimator.Play("RifleIdle");
    }
    public virtual void Disactive()
    {
        _isActive = false;
        _collider.enabled = false;
        RendererActive(false);
        
    }
    public virtual void DisactiveForInstantiate()
    {
        _isActive = false;
        _collider = gameObject.GetComponent<CapsuleCollider>();
        _collider.enabled = false;
        RendererActive(false);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_center.position, _radius);
    }
    public void RendererActive(bool stats)
    {
        foreach (var r in _enemyRenderer)
        {
            r.enabled = stats;
        }
    }
}
public enum EnemyType
{
    GUN = 0,
    UAV = 1,
}
