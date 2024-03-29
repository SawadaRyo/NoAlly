using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerBulletBase : ObjectBase, IBullet<WeaponArrow>
{
    [SerializeField, Header("eÌ¬x")]
    float _bulletSpeed = 0;
    [SerializeField, Header("eÌc¯Ô")]
    float _intervalTime = 3f;
    [SerializeField, Header("eÌRigitbody")]
    Rigidbody _rb = default;

    [Tooltip("eÌUÍ")]
    float[] _bulletPowers = new float[Enum.GetValues(typeof(ElementType)).Length];
    float _speed = 0f;
    [Tooltip("eÌ®«")]
    ElementType _elementType;
    [Tooltip("­Ë³êé¼OÌúÊu")]
    Transform _muzzlePos = null;
    [Tooltip("­Ë³êéûü")]
    Vector3 _muzzleForwardPos;
    [Tooltip("eÌÁ¬x")]
    Vector3 _velo = Vector3.zero;

    public WeaponArrow Owner { get; set; }

    public void FixedUpdate()
    {
        if (_isActive)
        {
            _velo.x = _bulletSpeed * _speed;
            _velo.y = _muzzleForwardPos.y;
            _rb.velocity = new Vector3(_velo.x, _rb.velocity.y, 0f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        HitMovement(other);
    }

    public void Create()
    {
        _isActive = true;
        _rb.isKinematic = !_isActive;
        SetTrans();
        ActiveObject(true);
        _bulletPowers = Owner.WeaponPower;
        _elementType = Owner.ElementType;
        Disactive(_intervalTime);
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
    public void DisactiveForInstantiate(WeaponArrow owner)
    {
        Owner = owner;
        _isActive = false;
        _rb = GetComponent<Rigidbody>();
        _velo = _rb.velocity;
        _muzzlePos = Owner.GenerateTrance;
        _bulletPowers = Owner.WeaponPower;
        ActiveObject(_isActive);
    }

    public void HitMovement(Collider target)
    {
        if (target.TryGetComponent(out IHitBehavorOfAttack hitObj) && hitObj.Owner != ObjectOwner.PLAYER)
        {
            hitObj.BehaviorOfHit(_bulletPowers, _elementType);
            Disactive();
        }
        else if (target.gameObject.tag == "TargetObject")
        {
            Disactive();
        }
    }
    void SetTrans()
    {
        _muzzleForwardPos = _muzzlePos.position;
        this.transform.position = _muzzleForwardPos;
        if (Owner.playerVec == ActorVec.Right)
        {
            _speed = 1;
        }
        else if (Owner.playerVec == ActorVec.Left)
        {
            _speed = -1;
        }
    }
}


