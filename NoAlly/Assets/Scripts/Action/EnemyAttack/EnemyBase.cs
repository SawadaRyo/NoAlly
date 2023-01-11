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
    [SerializeField] ObjectVisual _thisObject = default;

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
        _thisObject.ActiveWeapon(_isActive);
        _enemyAnimator.SetBool("Death", !_isActive);
    }
    public virtual void Disactive()
    {
        _isActive = false;
        _thisObject.ActiveWeapon(_isActive);
        _enemyAnimator.SetBool("Death", !_isActive);

    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_center.position, _radius);
    }
    

    public void DisactiveForInstantiate<T>(T Owner) where T : IObjectGenerator
    {
        _isActive = false;
        _thisObject.ActiveWeapon(_isActive);
    }
}
