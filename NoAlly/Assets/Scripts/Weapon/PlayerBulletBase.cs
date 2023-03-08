using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerBulletBase : ObjectBase, IObjectPool<WeaponSnip>, IBullet
{
    [SerializeField, Header("eÌ¬x")]
    float _bulletSpeed = 0;
    [SerializeField, Header("eÌc¯Ô")]
    float _intervalTime = 3f;
    [SerializeField, Header("eÌRigitbody")]
    Rigidbody _rb = default;

    [Tooltip("eÌUÍ")]
    float[] bulletPowers = new float[Enum.GetValues(typeof(ElementType)).Length];
    [Tooltip("eÌ®«")]
    ElementType elementType;
    [Tooltip("­Ë³êé¼OÌúÊu")]
    Transform _muzzlePos = null;
    [Tooltip("­Ë³êéûü")]
    Vector3 _muzzleForwardPos;
    [Tooltip("eÌÁ¬x")]
    Vector3 _velo = Vector3.zero;
    [Tooltip("eÌ®«")]

    public WeaponSnip Owner { get; set; }

    public void FixedUpdate()
    {
        if (!_isActive) return;
        _velo.x = _bulletSpeed * _muzzleForwardPos.x;
        _velo.y = _muzzleForwardPos.y;
        _rb.velocity = new Vector3(_velo.x, _rb.velocity.y, 0f);
        Disactive(_intervalTime);
    }
    public void WeaponAttackMovement(Collider target)
    {
        if (target.TryGetComponent(out IHitBehavorOfAttack hitObj))
        {
            hitObj.BehaviorOfHIt(bulletPowers,Owner.ElementType);
            Disactive();
        }
    }

    public void Create()
    {
        _rb.isKinematic = false;
        ActiveObject(true);

        _muzzleForwardPos = _muzzlePos.position;
        this.transform.position = _muzzleForwardPos;
    }

    public void Disactive()
    {
        _rb.isKinematic = true;
        ActiveObject(false);
    }

    public async void Disactive(float interval)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(interval));
        _isActive = false;
        ActiveObject(_isActive);
    }
    public void DisactiveForInstantiate(WeaponSnip owner)
    {
        Owner = owner;
        _rb = GetComponent<Rigidbody>();
        _velo = _rb.velocity;
        _muzzlePos = Owner.GenerateTrance;
    }

    public void HitMovement(Collider target)
    {
        throw new NotImplementedException();
    }

}


