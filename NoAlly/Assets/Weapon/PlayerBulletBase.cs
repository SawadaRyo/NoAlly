using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerBulletBase : ObjectBase, IBullet<WeaponArrow>
{
    [SerializeField, Header("�e�̑��x")]
    float _bulletSpeed = 0;
    [SerializeField, Header("�e�̎c������")]
    float _intervalTime = 3f;
    [SerializeField, Header("�e��Rigitbody")]
    Rigidbody _rb = default;

    [Tooltip("�e�̍U����")]
    WeaponPower _bulletPowers = WeaponPower.zero;
    [Tooltip("�e�̑���")]
    ElementType _elementType;
    [Tooltip("���˂���钼�O�̏����ʒu")]
    Transform _muzzlePos = null;
    [Tooltip("���˂�������")]
    Vector3 _muzzleForwardPos;
    [Tooltip("�e�̉����x")]
    Vector3 _velo = Vector3.zero;

    public WeaponArrow Owner { get; set; }

    public void FixedUpdate()
    {
        if (_isActive)
        {
            //_velo.x = _bulletSpeed * _speed;
            _velo.x = _bulletSpeed;
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
        _bulletPowers = Owner.GetWeaponPower;
        _elementType = Owner.WeaponOwner.CurrentElement.Value;
        Disactive(_intervalTime);
    }

    public void Disactive()
    {
        _isActive = false;
        _rb.isKinematic = !_isActive;
        ActiveObject(_isActive);
    }

    public async void Disactive(float interval)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(interval));
        _isActive = false;
        _rb.isKinematic = !_isActive;
        ActiveObject(_isActive);
    }
    public void DisactiveForInstantiate(WeaponArrow owner)
    {
        Owner = owner;
        _isActive = false;
        _rb = GetComponent<Rigidbody>();
        _velo = _rb.velocity;
        _muzzlePos = Owner.GenerateTrance;
        _bulletPowers = Owner.GetWeaponPower;
        ActiveObject(_isActive);
    }

    public void HitMovement(Collider target)
    {
        if (target)
        {
            if (target.CompareTag("Player")) return;
            if (target.TryGetComponent(out IHitBehavorOfAttack hitObj) && hitObj.Owner != ObjectOwner.PLAYER)
            {
                hitObj.BehaviorOfHit(_bulletPowers, _elementType);
            }
            Disactive();
        }

    }
    void SetTrans()
    {
        _muzzleForwardPos = _muzzlePos.position;
        this.transform.position = _muzzleForwardPos;
        if (Owner.playerVec == ActorVec.Right)
        {
            //_speed = 1;
        }
        else if (Owner.playerVec == ActorVec.Left)
        {
            //_speed = -1;
        }
    }
}


