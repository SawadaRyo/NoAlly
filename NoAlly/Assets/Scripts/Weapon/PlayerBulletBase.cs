using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerBulletBase : ObjectBase, IObjectPool<WeaponArrow>, IBullet
{
    [SerializeField, Header("’e‚Ì‘¬“x")]
    float _bulletSpeed = 0;
    [SerializeField, Header("’e‚Ìc—¯ŠÔ")]
    float _intervalTime = 3f;
    [SerializeField, Header("’e‚ÌRigitbody")]
    Rigidbody _rb = default;

    [Tooltip("’e‚ÌUŒ‚—Í")]
    float[] _bulletPowers = new float[Enum.GetValues(typeof(ElementType)).Length];
    [Tooltip("’e‚Ì‘®«")]
    ElementType _elementType;
    [Tooltip("”­Ë‚³‚ê‚é’¼‘O‚Ì‰ŠúˆÊ’u")]
    Transform _muzzlePos = null;
    [Tooltip("”­Ë‚³‚ê‚é•ûŒü")]
    Vector3 _muzzleForwardPos;
    [Tooltip("’e‚Ì‰Á‘¬“x")]
    Vector3 _velo = Vector3.zero;

    public WeaponArrow Owner { get; set; }

    public void FixedUpdate()
    {
        if (!_isActive) return;
        _velo.x = _bulletSpeed * _muzzleForwardPos.x;
        _velo.y = _muzzleForwardPos.y;
        _rb.velocity = new Vector3(_velo.x, _rb.velocity.y, 0f);
        Disactive(_intervalTime);
    }

    public void Create()
    {
        _rb.isKinematic = false;
        ActiveObject(true);
        _velo = _rb.velocity;
        _muzzleForwardPos = _muzzlePos.position;
        this.transform.position = _muzzleForwardPos;
        _bulletPowers = Owner.WeaponPower;
        _elementType = Owner.ElementType;
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
        _muzzlePos = Owner.GenerateTrance;
        _bulletPowers = Owner.WeaponPower;
        ActiveObject(_isActive);
    }

    public void HitMovement(Collider target)
    {
        if (target.TryGetComponent(out IHitBehavorOfAttack hitObj))
        {
            hitObj.BehaviorOfHIt(_bulletPowers, _elementType);
            Disactive();
        }
    }

}


