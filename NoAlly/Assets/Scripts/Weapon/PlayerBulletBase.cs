using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerBulletBase : ObjectBase, IBullet<WeaponArrow>
{
    [SerializeField, Header("íeÇÃë¨ìx")]
    float _bulletSpeed = 0;
    [SerializeField, Header("íeÇÃécóØéûä‘")]
    float _intervalTime = 3f;
    [SerializeField, Header("íeÇÃRigitbody")]
    Rigidbody _rb = default;

    [Tooltip("íeÇÃçUåÇóÕ")]
    float[] _bulletPowers = new float[Enum.GetValues(typeof(ElementType)).Length];
    float _speed = 0f;
    [Tooltip("íeÇÃëÆê´")]
    ElementType _elementType;
    [Tooltip("î≠éÀÇ≥ÇÍÇÈíºëOÇÃèâä˙à íu")]
    Transform _muzzlePos = null;
    [Tooltip("î≠éÀÇ≥ÇÍÇÈï˚å¸")]
    Vector3 _muzzleForwardPos;
    [Tooltip("íeÇÃâ¡ë¨ìx")]
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
        if (Owner.playerVec == PlayerVec.RIGHT)
        {
            _speed = 1;
        }
        else if (Owner.playerVec == PlayerVec.LEFT)
        {
            _speed = -1;
        }
    }
}


