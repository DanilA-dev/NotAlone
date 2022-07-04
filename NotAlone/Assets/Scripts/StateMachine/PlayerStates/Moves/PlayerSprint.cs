using UnityEngine;

public class PlayerSprint : BasePlayerMove
{
    public override MovementType.SpeedType MoveType => MovementType.SpeedType.Sprint;

    public PlayerSprint(PlayerController playerController, PlayerStatesFabric fabric) : base(playerController, fabric) { }


    public override void Enter()
    {
        base.Enter();
        _playerAnimator.SetBool("Sprint", true);
    }

    public override void ExecuteFixedUpdate()
    {
        base.ExecuteFixedUpdate();
        if (_playerController.Stamina.CurrentValue <= 0)
        {
            if (_stateMachine.PreviousState == _statesFabric.Dash())
                _stateMachine?.ChangeState(_statesFabric.Run());
            else
                _stateMachine?.ChangeState(_stateMachine.PreviousState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _playerAnimator.SetBool("Sprint", false);
    }
}
