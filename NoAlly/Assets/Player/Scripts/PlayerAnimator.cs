//日本語コメント可
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    Animator _animator = null;

    [Tooltip("Animationの遷移状況")]
    ObservableStateMachineTrigger _trigger = default;

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
