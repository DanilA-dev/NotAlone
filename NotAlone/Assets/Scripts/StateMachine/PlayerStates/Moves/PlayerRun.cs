using Player.States;
using UnityEngine;

public class PlayerRun : BasePlayerMove
{
    public PlayerRun(PlayerMovement player, PlayerStatesFabric statesFabric,
        PlayerStateController stateController) : base(player, statesFabric, stateController)
    {
    }

    public override MovementType.SpeedType MoveType => MovementType.SpeedType.Run;
    

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
