//日本語コメント可
using UnityEngine;
using UniRx;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    Animator _animator = null;

    public void MoveAnimation(PlayerMoveInput moveInput)
    {
        moveInput.IsDash
            .Where(_ => moveInput.IsDash.Value == true 
                     && moveInput.AbleDash == true 
                     && moveInput.CurrentMoveVector.Value.x != 0f
                     && moveInput.CurrentLocation.Value == StateOfPlayer.OnGround)
            .Subscribe(isDash =>
            {
                _animator.SetTrigger("Dudge");
            }).AddTo(moveInput);
        moveInput.CurrentLocation
            .Subscribe(currentLocation =>
            {
                _animator.SetBool("Jump", currentLocation == StateOfPlayer.InAir);
                _animator.SetBool("WallGrip", currentLocation == StateOfPlayer.GripingWall);
            }).AddTo(moveInput);
        moveInput.CurrentMoveVector
            .Subscribe(currentMoveVec =>
            {
                _animator.SetFloat("MoveSpeed", Mathf.Abs(currentMoveVec.x));
            }).AddTo(moveInput);
    }
    public void ActionAnimation(InputToWeapon weaponInput)
    {

    }
}
