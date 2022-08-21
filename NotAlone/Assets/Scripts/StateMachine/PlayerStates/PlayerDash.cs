using Cysharp.Threading.Tasks;
using Player.States;
using UnityEngine;

public class PlayerDash : IState
{
    private PlayerMovement _playerController;
    private StateMachine _stateMachine;
    private Animator _animator;
    private Rigidbody _body;

    private float _dashForce;
    private float _dashCooldown;
    private float _staminaPerDash;

    private bool _isDashing;

    public PlayerDash(PlayerMovement player, PlayerStateController stateController)
    {
        _playerController = player;
        _animator = player.Animator;
        _body = player.Body;
        _dashForce = player.DashForce;
        _dashCooldown = player.DashCooldown;
        _staminaPerDash = player.DashStamina;
        _stateMachine = stateController.StateMachine;
    }

    public void Enter()
    {
        Dash();
    }

    public void ExecuteFixedUpdate()
    {
        Friciton();
    }
        
    public void ExecuteUpdate() { }

    public void Exit() { }

    private void Dash()
    {
        _playerController.Stamina.ResetRegenTimer();
        Dashing(_dashCooldown, _playerController.MoveDir);
    }

    private async void Dashing(float dashCooldown, Vector3 dir)
    {
        float speed = _animator.speed;
        _animator.speed = 0;
        await UniTask.Yield();
        _animator.speed = speed;
        _isDashing = true;
        _body.AddRelativeForce(dir.normalized * _dashForce, ForceMode.Impulse);
        _playerController.Stamina.CurrentValue -= _staminaPerDash;
        _stateMachine.ChangeState(_stateMachine.PreviousState);
        await UniTask.Delay((int)dashCooldown * 1000);
        _isDashing = false;
    }

    public bool CanDash()
    {
        return _playerController.MoveDir != Vector3.zero && !_isDashing && _playerController.Stamina.CurrentValue > 0;
    }

    private void Friciton()
    {
        if (_body.velocity.magnitude > 1)
        {
            var oppositeDir = -_body.velocity * _playerController.CurrentMoveType.Friction;
            _body.AddForce(oppositeDir * Time.deltaTime);
        }
    }
}
