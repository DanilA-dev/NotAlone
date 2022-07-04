using System.Linq;
using UnityEngine;

public class BasePlayerMove : IState
{
    protected Animator _playerAnimator;
    protected StateMachine _stateMachine;
    protected Rigidbody _body;
    protected MovementType _moveType;
    protected PlayerController _playerController;
    protected PlayerStatesFabric _statesFabric;
    public virtual MovementType.SpeedType MoveType { get; }

    public BasePlayerMove(PlayerController player, PlayerStatesFabric statesFabric)
    {
        _statesFabric = statesFabric;
        _playerController = player;
        _body = player.Body;
        _playerAnimator = player.Animator;
        _moveType = _playerController.PlayerMovements.Where(m => m.Type == MoveType).FirstOrDefault();
        _stateMachine = player.StateMachine;
    }

    public virtual void Enter()
    {
        _playerController.ChangeCurrentSpeedType(_moveType);
    }

    public virtual void ExecuteFixedUpdate()
    {
        Move();
        Friciton();
    }

    public virtual   void ExecuteUpdate()
    {
       if (_playerController.MoveDir == Vector3.zero)
            _stateMachine.ChangeState(_statesFabric.Idle());
    }
   
    public virtual void Exit() { }
   
    private void Move()
    {
        Vector3 target = _body.transform.InverseTransformDirection(_body.velocity);
        float velocityZ = target.z;
        float velocityX = target.x;

        _playerAnimator.SetFloat("InputY", velocityZ, 0.1f, Time.deltaTime);
        _playerAnimator.SetFloat("InputX", velocityX, 0.1f, Time.deltaTime);

        _body.AddRelativeForce(_playerController.MoveDir.normalized * _moveType.Acceleration * Time.deltaTime);

        if (_playerController.MoveDir != Vector3.zero)
            _playerController.CurrentMoveType.ReduceStamina(_playerController.Stamina);
    }

    private void Friciton()
    {
        if (_body.velocity.magnitude > 0)
        {
            var oppositeDir = -_body.velocity * _playerController.CurrentMoveType.Friction;
            _body.AddForce(oppositeDir * Time.deltaTime);
        }
    }
}
    
