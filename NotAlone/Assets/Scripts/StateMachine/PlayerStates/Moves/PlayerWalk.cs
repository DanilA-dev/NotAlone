using UnityEngine;

public class PlayerWalk : BasePlayerMove
{
    public PlayerWalk(PlayerController playerController, Rigidbody body, Animator playerAnimator,
         StateMachine stateMachine)
       : base(playerController, body, playerAnimator, stateMachine)
    {
    }

    public override MovementType.SpeedType MoveType => MovementType.SpeedType.Walk;
}
