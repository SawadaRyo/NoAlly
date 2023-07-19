//日本語コメント可
using UnityEngine;
using UniRx;
//using UniRx.Triggers;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    Animator _animator = null;
    [SerializeField]
    CapsuleCollider _actorCollider = null;

    //[Tooltip("Animationの遷移状況")]
    //ObservableStateMachineTrigger _trigger = default;

    [Tooltip("")]
    CapsuleColliderValue _colliderValue;
    public void Initialize(InputToPlayerMove moveInput)
    {
        _colliderValue = new CapsuleColliderValue(_actorCollider.center, _actorCollider.height);
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                var height = _animator.GetFloat("ColliderHeight");
                var center = new Vector3(_animator.GetFloat("ColliderCenterX")
                                       , _animator.GetFloat("ColliderCenterY")
                                       , _animator.GetFloat("ColliderCenterZ"));
                Debug.Log(height);
                Debug.Log(center);
                if (height != 0)
                {
                    _actorCollider.height = height;
                }
                else
                {
                    _actorCollider.height = _colliderValue.height;
                }

                if(center != Vector3.zero)
                {
                    _actorCollider.center = center;
                }
                else
                {
                    _actorCollider.center = _colliderValue.center;
                }
                
            }).AddTo(moveInput);
        MoveAnimation(moveInput);
    }

    public void MoveAnimation(InputToPlayerMove moveInput)
    {
        moveInput.IsDash
            .Where(_ => moveInput.AbleDash == true
                     && moveInput.CurrentMoveVector.Value.x != 0f
                     && moveInput.CurrentLocation.Value == StateOfPlayer.OnGround)
            .Subscribe(isDash =>
            {
                _animator.SetTrigger("Dudge");
            }).AddTo(moveInput);
        moveInput.CurrentLocation
            .Subscribe(currentLocation =>
            {
                _animator.SetBool("InAir", currentLocation == StateOfPlayer.InAir);
                _animator.SetBool("WallGrip", currentLocation == StateOfPlayer.GripingWall);
            }).AddTo(moveInput);
        moveInput.CurrentMoveVector
            .Subscribe(currentMoveVec =>
            {
                _animator.SetFloat("MoveSpeed", Mathf.Abs(currentMoveVec.x * moveInput.PlayerParamater.speed));
            }).AddTo(moveInput);
        moveInput.WallBehaviour.Climbing
            .Subscribe(climbing =>
            {
                _animator.SetBool("Climbing", climbing);
            });
    }


}

public struct CapsuleColliderValue
{
    public Vector3 center;
    public float height;

    public CapsuleColliderValue(Vector3 _center,float _higth)
    {
        center = _center;
        height = _higth;
    }
}

