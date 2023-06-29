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
            .Where(_ =>moveInput.IsDash.Value == true)
            .Subscribe(isDash =>
            {
                _animator.SetTrigger("Dudge");
            }).AddTo(moveInput);
        moveInput.CurrentLocation
            .Subscribe(currentLocation =>
            {
                _animator.SetBool("Jump", !currentLocation.Item1);
                _animator.SetBool("WallGrip", currentLocation == (false,StateOfPlayer.GripingWall));
            }).AddTo(moveInput);
        moveInput.CurrentMove
            .Subscribe(currentMoveVec =>
            {
                _animator.SetFloat("MoveSpeed",Mathf.Abs(currentMoveVec));
            }).AddTo(moveInput);
    }
    public void ActionAnimation(InputToWeapon weaponInput)
    {

    }
}
