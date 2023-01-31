using System;
using System.Collections;
using UnityEngine;
public class GunTypeEnemy : EnemyBase, IObjectGenerator
{
    [SerializeField,Header("生成する弾")] 
    EnemyBullet _bulletPrefab;
    [SerializeField,Header("生成する弾の数")]
    int _bulletSize = 10;
    [SerializeField,Header("弾の連射速度")] 
    float _interval = 1f;
    [SerializeField,Header("弾のマズル")] 
    Transform _muzzleTrans;
    [SerializeField,Header("弾の貯蓄地点")] 
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
    /// オブジェクトの回転制御
    /// </summary>
    /// <param name="player">向く方向</param>
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
        //プレイヤーを見つけた時プレイヤーを攻撃
        _stateMachine.AddTransition<Search, GunAttack>((int)StateOfEnemy.Attack);
        //プレイヤーを見失ったとき攻撃を中止
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