using Player.States;

public class PlayerStatesFabric
{
    private PlayerMovement _player;
    private PlayerStateController _stateController;

    private PlayerIdle _idle;
    private PlayerWalk _walk;
    private PlayerSprint _sprint;
    private PlayerRun _run;
    private PlayerObjectInteraction _interaction;
    private PlayerDash _dash;

    public PlayerStatesFabric(PlayerMovement player, PlayerStateController stateController)
    {
        _player = player;
        _stateController = stateController;
    }

    public PlayerIdle Idle()
    {
        return _idle = _idle ?? new PlayerIdle(_player, this, _stateController);
    }

    public PlayerWalk Walk()
    {
        return _walk = _walk ?? new PlayerWalk(_player, this, _stateController);
    }

    public PlayerRun Run()
    {
        return _run = _run ?? new PlayerRun(_player, this, _stateController);
    }

    public PlayerSprint Sprint()
    {
        return _sprint = _sprint ?? new PlayerSprint(_player, this, _stateController);
    }

    public PlayerDash Dash()
    {
        return _dash = _dash ?? new PlayerDash(_player, _stateController);
    }

    public PlayerObjectInteraction ObjectInteract()
    {
        return _interaction = _interaction ?? new PlayerObjectInteraction(_player);
    }
}
