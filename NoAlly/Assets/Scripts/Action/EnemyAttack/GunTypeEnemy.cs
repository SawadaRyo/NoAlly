using System;
using System.Collections;
using UnityEngine;
public class GunTypeEnemy : EnemyBase, IObjectGenerator
{
    [SerializeField,Header("��������e")] 
    EnemyBullet _bulletPrefab;
    [SerializeField,Header("��������e�̐�")]
    int _bulletSize = 10;
    [SerializeField,Header("�e�̘A�ˑ��x")] 
    float _interval = 1f;
    [SerializeField,Header("�e�̃}�Y��")] 
    Transform _muzzleTrans;
    [SerializeField,Header("�e�̒��~�n�_")] 
    Transform _poolTrans;

    const float _turnSpeed = 10f;
    ObjectPool<EnemyBullet> _bulletPool = new ObjectPool<EnemyBullet>();
    Vector3 _distance = Vector3.zero;

    public float Interval => _interval;
    public Transform GenerateTrance => _muzzleTrans;
    public Vector3 Distance => _distance;

    public override void Start()
    {
        base.Start();
        _bulletPool.SetBaseObj(_bulletPrefab, _poolTrans, (int)HitOwner.Enemy);
        _bulletPool.SetCapacity(this, _bulletSize);
    }
    /// <summary>
    /// �I�u�W�F�N�g�̉�]����
    /// </summary>
    /// <param name="player">��������</param>
    public void EnemeyRotate(Transform player)
    {
        Vector3 _distance = (player.position - this.transform.position).normalized;
        if (_distance.x == 1)
        {
            Quaternion rotationRight = Quaternion.LookRotation(Vector3.right);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationRight, Time.deltaTime * _turnSpeed);
        }
        else if (_distance.x == -1)
        {
            Quaternion rotationLeft = Quaternion.LookRotation(Vector3.left);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotationLeft, Time.deltaTime * _turnSpeed);
        }
    }

    public override void DisactiveForInstantiate<T>(T Owner)
    {
        base.DisactiveForInstantiate(Owner);
        //�v���C���[�����������v���C���[���U��
        _stateMachine.AddTransition<Search, GunAttack>((int)StateOfEnemy.Attack);
        //�v���C���[�����������Ƃ��U���𒆎~
        _stateMachine.AddTransition<GunAttack, Search>((int)StateOfEnemy.Saerching);
    }
    public void InsBullet()
    {
        var bullet = _bulletPool.Instantiate((int)HitOwner.Enemy);
        bullet.transform.position = _muzzleTrans.position;
    }

    public override void EnemyAttack()
    {
        throw new NotImplementedException();
    }
}

class GunAttack : Attack
{
    protected override void OnEnter(StateMachine<EnemyBase>.State prevState)
    {
        base.OnEnter(prevState);
        Owner.EnemyAnimator.SetBool("Aiming", true);
        Owner.StartCoroutine(RapidFire((GunTypeEnemy)Owner));
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (!Owner.Player)
        {
            Owner.EnemyStateMachine.Dispatch((int)StateOfEnemy.Saerching);
        }
    }
    protected override void OnExit(StateMachine<EnemyBase>.State nextState)
    {
        base.OnExit(nextState);
        Owner.EnemyAnimator.SetBool("Aiming", false);
    }
    public IEnumerator RapidFire<T>(T enemy) where T : GunTypeEnemy
    {
        var wait = new WaitForSeconds(enemy.Interval);
        while (Owner.Player)
        {
            enemy.EnemeyRotate(Owner.Player.transform);
            Owner.EnemyAnimator.SetTrigger("Fire");
            yield return wait;
        }
    }
}


//class GunAttack : State
//{
//    PlayerContoller player = null;

//    void EnemeyRotate(PlayerContoller player)
//    {
//        Vector3 _distance = (player.transform.position - Owner.transform.position).normalized;
//        if (_distance.x == 1)
//        {
//            Quaternion rotationRight = Quaternion.LookRotation(Vector3.right);
//            Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation, rotationRight, Time.deltaTime * _turnSpeed);
//        }
//        else if (_distance.x == -1)
//        {
//            Quaternion rotationLeft = Quaternion.LookRotation(Vector3.left);
//            Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation, rotationLeft, Time.deltaTime * _turnSpeed);
//        }
//    }
//    public override void EnemyAttack()
//    {

//        PlayerContoller player = InSight();
//        if (player)
//        {
//            EnemeyRotate(player);
//        }
//        _enemyAnimator.SetBool("Aiming", InSight());
//        StartCoroutine(RapidFire(InSight()));
//    }

//}