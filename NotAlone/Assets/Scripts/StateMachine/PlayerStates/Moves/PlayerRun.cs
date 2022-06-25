using UnityEngine;

public class PlayerRun : BasePlayerMove
{
    public override MovementType.SpeedType MoveType => MovementType.SpeedType.Run;
    public PlayerRun(PlayerController playerController, Rigidbody body, Animator playerAnimator,
         StateMachine stateMachine)
       : base(playerController, body, playerAnimator, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _playerAnimator.SetBool("Run", true);
    }

    public override void Exit()
    {
        base.Exit();
        _playerAnimator.SetBool("Run", false);
    }

}
