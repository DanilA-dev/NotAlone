using Player.States;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerIdle : IState
{
    private PlayerMovement _playerController;
    private Rigidbody _body;
    private Animator _animator;
    private StateMachine _stateMachine;
    private PlayerStatesFabric _statesFarbic;

    public PlayerIdle(PlayerMovement player, PlayerStatesFabric statesFarbic, PlayerStateController stateController)
    {
        _playerController = player;
        _body = player.Body;
        _animator = player.Animator;
        _statesFarbic = statesFarbic;
        _stateMachine = stateController.StateMachine;
    }

    public void Enter() { }

    public void ExecuteFixedUpdate()
    {
        Friciton();
    }

    public void ExecuteUpdate()
    {
        if (_animator.GetFloat("InputY") != 0 && _animator.GetFloat("InputX") != 0)
        {
            _animator.SetFloat("InputY", 0, 0.1f, Time.deltaTime);
            _animator.SetFloat("InputX", 0, 0.1f, Time.deltaTime);
        }

        if (_playerController.MoveDir != Vector3.zero)
            _stateMachine?.ChangeState(_statesFarbic.Walk());

    }

    public void Exit() { }

    private void Friciton()
    {
        if (_body.velocity.magnitude > 1)
        {
            var oppositeDir = -_body.velocity * _playerController.CurrentMoveType.Friction;
            _body.AddForce(oppositeDir * Time.deltaTime);
        }
    }

}
