//日本語コメント可
using UniRx;
using State = StateMachine<EnemyBase>.State;

public class EnemyBattlePosture : State
{
    protected override void OnEnter(State prevState)
    {
        base.OnEnter(prevState);
        Owner.ObjectAnimator.SetBool("InSight", true);
    }

    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
        Owner.ObjectAnimator.SetBool("InSight", false);
    }
    protected override void OnTranstion()
    {
        base.OnTranstion();
        Owner.Player
            .Where(player => player == null && IsActive)
            .Subscribe(player =>
            {
                Owner.EnemyStateMachine.Dispatch((int)StateOfEnemy.Saerching);
            });
    }
}
