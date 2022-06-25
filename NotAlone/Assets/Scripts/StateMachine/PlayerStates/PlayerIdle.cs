using System.Threading.Tasks;
using UnityEngine;

public class PlayerIdle : IState
{
    private PlayerController _playerController;
    private Rigidbody _body;
    private Animator _animator;
    private StateMachine _stateMachine;
    private IState _walkState;


    public PlayerIdle(PlayerController player, StateMachine stateMachine, Animator animator, Rigidbody body)
    {
        _playerController = player;
        _stateMachine = stateMachine;
        _animator = animator;
        _body = body;
        GetTransitionState();
    }

    private async void GetTransitionState()
    {
        await Task.Delay(300);
        _walkState = _playerController.WalkState;
    }

    public void Enter()
    {
    }

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
            _stateMachine?.ChangeState(_walkState);

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
