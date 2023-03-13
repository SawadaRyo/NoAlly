using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyBullet : ObjectBase, IObjectPool<GunTypeEnemy>,IBullet
{
    [SerializeField, Header("�e�̑��x")]
    float _bulletSpeed = 0;
    [SerializeField, Header("�e�̎c������")]
    float _intervalTime = 3f;
    [SerializeField, Header("�e��Rigitbody")]
    Rigidbody _rb = default;

    [Tooltip("�e�̍U����")]
    float[] bulletPowers = new float[Enum.GetValues(typeof(ElementType)).Length];
    [Tooltip("�e�̑���")]
    ElementType elementType;
    [Tooltip("���˂���钼�O�̏����ʒu")]
    Transform _muzzlePos = null;
    [Tooltip("���˂�������")]
    Vector3 _muzzleForwardPos;
    [Tooltip("�e�̉����x")]
    Vector3 _velo = Vector3.zero;
    public GunTypeEnemy Owner { get; set; }

    void OnTriggerEnter(Collider other)
    {
        HitMovement(other);
        Disactive();
    }

    public void Create()
    {
        _rb.isKinematic = false;
        ActiveObject(true);
        _velo = _rb.velocity;
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
    public void DisactiveForInstantiate(GunTypeEnemy owner)
    {
        Owner = owner;
        _isActive = false;
        ActiveObject(_isActive);
        _rb = GetComponent<Rigidbody>();
        _muzzlePos = Owner.GenerateTrance;
    }

    public void HitMovement(Collider target)
    {
        if (target.TryGetComponent(out IHitBehavorOfAttack hitObj))
        {
            hitObj.BehaviorOfHit(bulletPowers, ElementType.ELEKE);
            Disactive();
        }
    }
}
