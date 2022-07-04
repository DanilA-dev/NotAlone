using UnityEngine;

public class PlayerRun : BasePlayerMove
{
    public override MovementType.SpeedType MoveType => MovementType.SpeedType.Run;
    public PlayerRun(PlayerController playerController, PlayerStatesFabric fabric) : base(playerController, fabric) { }

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
