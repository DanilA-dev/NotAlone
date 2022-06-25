using UnityEngine;

public class PlayerSprint : BasePlayerMove
{
    public override MovementType.SpeedType MoveType => MovementType.SpeedType.Sprint;

    public PlayerSprint(PlayerController playerController, Rigidbody body, Animator playerAnimator,
         StateMachine stateMachine)
       : base(playerController, body, playerAnimator, stateMachine)
    {
    }

}
