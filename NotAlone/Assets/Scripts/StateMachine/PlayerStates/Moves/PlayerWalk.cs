using Player.States;
using UnityEngine;

public class PlayerWalk : BasePlayerMove
{
    public PlayerWalk(PlayerMovement playerController, PlayerStatesFabric fabric,
        PlayerStateController stateController) : base(playerController, fabric, stateController) { }


    public override MovementType.SpeedType MoveType => MovementType.SpeedType.Walk;
}
