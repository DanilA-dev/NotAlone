using UnityEngine;

public class PlayerWalk : BasePlayerMove
{
    public PlayerWalk(PlayerController playerController, PlayerStatesFabric fabric) : base(playerController, fabric) { }


    public override MovementType.SpeedType MoveType => MovementType.SpeedType.Walk;
}
