//日本語コメント可
using System.Collections;
using UniRx;
using UnityEngine;
using DataOfWeapon;

public class ActionStatePresenter : MonoBehaviour
{
    [Header("オブジェクトの動作関係")]
    [SerializeField, Header("プレイヤー")]
    PlayerBehaviorController _inputToPlayer;
    [SerializeField, Header("武器")]
    WeaponController _weaponController;
    [Space(15)]
    [Header("オブジェクトのアニメーションの挙動")]
    [SerializeField, Header("プレイヤー")]
    PlayerAnimatorController _playerAnimator;
    [SerializeField, Header("武器")]
    WeaponAnimator _weaponAnimator;
    [Space(15)]
    [SerializeField, Header("初期装備")]
    WeaponType[] mainAndSub = new WeaponType[2];


    public void DoAttack()
    {
        _weaponController.EquipementWeapon.Value.AttackBehaviour();
    }
    public void AttackMove(float moveSpeed)
    {
        StartCoroutine(MoveAnimEvent(moveSpeed));
    }
    IEnumerator MoveAnimEvent(float moveSpeed)
    {
        Vector3 moveVec = new Vector3(_weaponController.GetAttackPos.position.x - _inputToPlayer.transform.position.x, 0f, 0f).normalized;
        float time = 0f;
        float interval = 0.2f;
        while (true)
        {
            _inputToPlayer.Rb.velocity = _inputToPlayer.MoveBehaviour.ActorMoveMethod(moveVec.x, moveSpeed, _inputToPlayer.HitInfo);
            time += Time.deltaTime;
            if(time > interval)
            {
                _inputToPlayer.Rb.velocity = Vector3.zero;
                yield break;
            }
            yield return null;
        }
    }

    void Start()
    {
        _weaponController.Initializer(mainAndSub[0], mainAndSub[1]);
        _weaponAnimator.Initializer();
        _playerAnimator.Initializer(_inputToPlayer);
        _playerAnimator.StateChacker();
        _playerAnimator.WeaponActionAnimation(_inputToPlayer, _weaponController);
        WeaponStateChacker();
        InputStateChacker();
        AttackStateChacker();
        InputUpdate();
    }
    void WeaponStateChacker()
    {
        _weaponController.EquipementWeapon
            .Subscribe(weapon =>
            {
                _weaponAnimator.DeformationWeapon(weapon.WeaponData.TypeOfWeapon, _weaponController.CurrentElement.Value);
            }).AddTo(this);
        _weaponController.CurrentElement
            .Subscribe(element =>
            {
                _weaponAnimator.DeformationWeapon(_weaponController.EquipementWeapon.Value.WeaponData.TypeOfWeapon, element);
            }).AddTo(this);
    }
    void AttackStateChacker()
    {
        _playerAnimator.IsAttack
            .Subscribe(isParticle =>
            {
                Debug.Log("Do");
                if (_weaponController.EquipementWeapon.Value is WeaponCombat combat)
                {
                    combat.DoParticle(isParticle);
                }
                _weaponAnimator.AttackJudge(isParticle);
            }).AddTo(this);
    }
    void InputStateChacker()
    {
        _inputToPlayer.IsSwichWeapon
            .Where(_ => _playerAnimator.IsAttack.Value == BoolAttack.NONE)
            .Subscribe(switchWeapon =>
            {
                _weaponController.SwichEquipmentWeapon(switchWeapon);
            }).AddTo(this);


        _inputToPlayer.InputAttackCharge
            .Where(_ => _inputToPlayer.InputAttackCharge.Value && _playerAnimator.AbleAttack)
            .Subscribe(inputCharge =>
            {
                float speedInCharge = _weaponController.EquipementWeapon.Value.WeaponData.SpeedInCharge;
                _inputToPlayer.ParamaterCon.ChangeMoveSpeed(speedInCharge);
                _weaponController.EquipementWeapon.Value.Charging(inputCharge);
            }).AddTo(this);
        _inputToPlayer.InputAttackUp
            .Where(_ => _inputToPlayer.InputAttackUp.Value && _playerAnimator.AbleAttack)
            .Subscribe(inputUp =>
            {
                _inputToPlayer.ParamaterCon.ChangeMoveSpeed();
                _weaponController.EquipementWeapon.Value.Charging(false);
            }).AddTo(this);
    }
    void InputUpdate()
    {
        _playerAnimator.AbleDash
            .Subscribe(ableDash =>
            {
                //Debug.Log(ableDash);
                _inputToPlayer.AbleDash = ableDash;
            }).AddTo(this);
        _playerAnimator.AbleMove
            .Subscribe(ableMove =>
            {
                _inputToPlayer.AbleMove = ableMove;
            }).AddTo(this);
        _playerAnimator.AbleJump
            .Subscribe(ableJump =>
            {
                _inputToPlayer.AbleJump = ableJump;
            }).AddTo(this);
        _playerAnimator.AttackMoveSpeed
            .Subscribe(speed =>
            {
                if (speed != 0f)
                {
                    Vector2 vec = _inputToPlayer.MoveBehaviour.ActorMoveMethod(_inputToPlayer.CurrentVec.x, speed, _inputToPlayer.HitInfo);
                    _inputToPlayer.Rb.velocity = vec;
                }
            }).AddTo(this);
    }

}
