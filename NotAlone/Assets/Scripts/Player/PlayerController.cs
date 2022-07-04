using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Systems")]
    [SerializeField] private HealthSystem _health;
    [SerializeField] private StaminaSystem _stamina;
    [SerializeField] private PlayerUI _playerUI;
    [Space]
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Rigidbody _body;
    [Space]
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _dashForce;
    [SerializeField] private float _staminaPerDash;
    [SerializeField] private float _dashCooldown;
    [Space]
    [SerializeField] private List<MovementType> _playerMovements = new List<MovementType>();

    private StateMachine _stateMachine;
    private PlayerStatesFabric _statesFabic;
    private PlayerDash _dash;

    private IInteractable _lastInteractableObject;
    private MovementType _currentMovement;
    private PlayerInput _input;
    private Camera _cam;

    private Vector3 _moveDir;
    private Vector3 _dirToMouse;

    #region Properties
  
    public Vector3 MoveDir => _moveDir;
    public PlayerInput Input => _input;
    public StateMachine StateMachine => _stateMachine;
    public Rigidbody Body => _body;
    public Animator Animator => _playerAnimator;
    public List<MovementType> PlayerMovements => _playerMovements;
    public MovementType CurrentMoveType => _currentMovement;
    public GameObject Interactor => this.gameObject;
    public StaminaSystem Stamina => _stamina;
    public float DashForce => _dashForce;
    public float DashCooldown => _dashCooldown;
    public float DashStamina => _staminaPerDash;


    #endregion

    private void Awake()
    {
        _cam = Camera.main;
        _input = new PlayerInput();
        ToogleInput(true);
    }

    private void OnEnable()
    {
        _playerUI.InitPlayerValues(_health, Stamina);
        SetInputs();
        EnableStateMachine();
    }

    private void OnDisable()
    {
        ToogleInput(false);
    }

    private void SetInputs()
    {
        _input.Player.Move.performed += GetMoveVector;
        _input.Player.Run.performed += HandleRun;
        _input.Player.Sprint.performed += HandleSprint;
        _input.Player.Sprint.canceled += _ => DisableSprint();
        _input.Player.Dash.performed += HandleDash;
        _input.Player.Interact.performed += HandleObjectInteraction;
    }

    private void Update()
    {
        RotateTowardsMouse();
        if(_stateMachine.CurrentState != null)
            _stateMachine.Tick();
    }

    private void FixedUpdate()
    {
        if (_stateMachine.CurrentState != null)
            _stateMachine.FixedTick();
    }

    private void EnableStateMachine()
    {
        _statesFabic = new PlayerStatesFabric(this);
        _stateMachine = new StateMachine();
        _dash = _statesFabic.Dash();
        _stateMachine.OnStateChange += () => _playerUI.ChangeStateDebug(_stateMachine.CurrentState.ToString());
        _stateMachine.SetStartState(_statesFabic.Idle());
    }

    public void ChangeCurrentSpeedType(MovementType type)
    {
        _currentMovement = type;
    }

    private void RotateTowardsMouse()
    {
        Vector3 lookDelta = _input.Player.LookDelta.ReadValue<Vector2>();
        Ray ray = _cam.ScreenPointToRay(_input.Player.Look.ReadValue<Vector2>());
        if(lookDelta != Vector3.zero)
        {
            if (Physics.SphereCast(ray, 0.5f, out RaycastHit hit))
            {
                _dirToMouse = hit.point - transform.position;
                _dirToMouse.y = 0;

                if (_dirToMouse.magnitude < 1)
                    return;

                _dirToMouse.Normalize();
                Quaternion r = Quaternion.LookRotation(_dirToMouse, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, r, _rotationSpeed * Time.deltaTime);
            }
        }
    }

    #region States

    private void HandleRun(InputAction.CallbackContext obj)
    {
        if (_stateMachine.CurrentState != _statesFabic.Run())
            _stateMachine.ChangeState(_statesFabic.Run());
        else
            _stateMachine.ChangeState(_statesFabic.Walk());
    }


    private void HandleSprint(InputAction.CallbackContext obj)
    {
        if (_stateMachine.CurrentState != _statesFabic.Sprint() && _stamina.CurrentValue > 0)
            _stateMachine.ChangeState(_statesFabic.Sprint());
    }

    private void DisableSprint()
    {
        if (_stateMachine.CurrentState == _statesFabic.Sprint())
            _stateMachine.ChangeState(_stateMachine.PreviousState);
    }

    private void HandleDash(InputAction.CallbackContext obj)
    {
        if (_dash.CanDash())
            _stateMachine.ChangeState(_dash);
    }
    private void HandleObjectInteraction(InputAction.CallbackContext obj)
    {
        _lastInteractableObject?.Interact(_statesFabic.ObjectInteract());
        _stateMachine.ChangeState(_statesFabic.ObjectInteract());
    }

    public void DisableInteractionState()
    {
        _stateMachine.ChangeState(_statesFabic.Idle());
    }

    #endregion
    private void GetMoveVector(InputAction.CallbackContext value)
    {
        var input = value.ReadValue<Vector2>();
        _moveDir = new Vector3(input.x, 0, input.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable i))
            _lastInteractableObject = i;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable i))
            _lastInteractableObject = null;
    }


    public bool ToogleInput(bool value)
    {
        bool i = value;

        if (i == true)
            _input.Enable();
        else
            _input.Disable();

        return i;
    }    
}

