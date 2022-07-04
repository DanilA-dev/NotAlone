
public class PlayerStatesFabric
{
    private PlayerController _player;

    public PlayerStatesFabric(PlayerController player)
    {
        _player = player;
    }

    public PlayerIdle Idle()
    {
        return new PlayerIdle(_player, this);
    }

    public PlayerWalk Walk()
    {
        return new PlayerWalk(_player, this);
    }

    public PlayerRun Run()
    {
        return new PlayerRun(_player, this);
    }

    public PlayerSprint Sprint()
    {
        return  new PlayerSprint(_player, this);
    }

    public PlayerDash Dash()
    {
        return new PlayerDash(_player);
    }

    public PlayerObjectInteraction ObjectInteract()
    {
        return new PlayerObjectInteraction(_player);
    }
}
