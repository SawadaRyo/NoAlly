using System.Collections;
using UnityEngine;

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
    [Tooltip("�e�̉����x")]
    Vector3 _velo = Vector3.zero;
    [Tooltip("�e�̌���")]
    Vector3 _bulletVec = Vector3.zero;

    public WeaponArrow Owner { get; set; }

    public void FixedUpdate()
    {
        if (_isActive)
        {
            _rb.velocity = _bulletVec * _bulletSpeed;
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
        _elementType = Owner.Base.CurrentElement.Value;
        StartCoroutine(Disactive(_intervalTime));
    }

    public void Disactive()
    {
        _isActive = false;
        _rb.isKinematic = !_isActive;
        ActiveObject(_isActive);
    }

    public IEnumerator Disactive(float interval)
    {
        float time = 0f;
        while (true)
        {
            time += Time.deltaTime;
            if (time > interval)
            {
                _isActive = false;
                _rb.isKinematic = !_isActive;
                ActiveObject(_isActive);
                yield break;
            }
            else if (!_isActive)
            {
                yield break;
            }
            yield return null;
        }
    }
    public void DisactiveForInstantiate(WeaponArrow owner)
    {
        Owner = owner;
        _isActive = false;
        _rb = GetComponent<Rigidbody>();
        _velo = _rb.velocity;
        _muzzlePos = Owner.GenerateTrance;
        ActiveObject(_isActive);
    }

    public void HitMovement(Collider target)
    {
        if (target)
        {
            if (target.CompareTag("Player")) return;
            if (target.CompareTag("Enemy") || target.CompareTag("Gimic"))
            {
                if (target.TryGetComponent(out IHitBehavorOfAttack hitObj))
                {
                    hitObj.BehaviorOfHit(_bulletPowers, _elementType);
                }
                else if(target.TryGetComponent(out IHitBehavorOfGimic hitGimic))
                {
                    hitGimic.BehaviorOfHit(Owner, _elementType);
                }
                Disactive();
            }
        }

    }
    void SetTrans()
    {
        this.transform.position = _muzzlePos.position;
        _bulletVec = new Vector3(Owner.Base.GetAttackPos.position.x - Owner.Owner.transform.position.x
                               , 0f
                               , Owner.Base.GetAttackPos.position.z - Owner.Owner.transform.position.z).normalized;
        if (_bulletVec.x > 0f)
        {
            this.transform.rotation = Quaternion.AngleAxis(0f, Vector3.up);
        }
        else if (_bulletVec.x <= 0f)
        {
            this.transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
        }
    }

    void IObjectPool<WeaponArrow>.Disactive(float interval)
    {
        throw new System.NotImplementedException();
    }
}


