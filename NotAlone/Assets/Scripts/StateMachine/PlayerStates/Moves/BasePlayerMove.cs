using Player.States;
using System.Linq;
using UnityEngine;

public class BasePlayerMove : IState
{
    protected Animator _playerAnimator;
    protected StateMachine _stateMachine;
    protected Rigidbody _body;
    protected MovementType _moveType;
    protected PlayerMovement _playerMovement;
    protected PlayerStatesFabric _statesFabric;
    public virtual MovementType.SpeedType MoveType { get; }

    public BasePlayerMove(PlayerMovement player, PlayerStatesFabric statesFabric, PlayerStateController stateController)
    {
        _statesFabric = statesFabric;
        _playerMovement = player;
        _body = player.Body;
        _playerAnimator = player.Animator;
        _moveType = _playerMovement.PlayerMovements.Where(m => m.Type == MoveType).FirstOrDefault();
        _stateMachine = stateController.StateMachine;
    }

    public virtual void Enter()
    {
        _playerMovement.ChangeCurrentSpeedType(_moveType);
    }

    public virtual void ExecuteFixedUpdate()
    {
        Move();
        Friciton();
    }

    public virtual void ExecuteUpdate()
    {
       if (_playerMovement.MoveDir == Vector3.zero)
            _stateMachine.ChangeState(_statesFabric.Idle());
    }
   
    public virtual void Exit() { }
   
    private void Move()
    {
        float velocityX = Mathf.Clamp01(_body.velocity.x);

        _playerAnimator.SetFloat("InputX", _body.velocity.z, 0.1f, Time.deltaTime);

        _body.AddForce(-_playerMovement.MoveDir.normalized * _moveType.Acceleration * Time.deltaTime);

        if (_playerMovement.MoveDir != Vector3.zero)
            _playerMovement.CurrentMoveType.ReduceStamina(_playerMovement.Stamina);
    }

    private void Friciton()
    {
        if (_body.velocity.magnitude > 0)
        {
            var oppositeDir = -_body.velocity * _playerMovement.CurrentMoveType.Friction;
            _body.AddForce(oppositeDir * Time.deltaTime);
        }
    }
}
    
