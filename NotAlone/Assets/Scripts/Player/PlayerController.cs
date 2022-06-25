using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour, IInteractor
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
    private IState _idleState;
    private IState _walkState;
    private IState _runState;
    private IState _sprintState;

    private IInteractable _lastInteractableObject;
    private MovementType _currentMovement;
    private PlayerInput _input;
    private Camera _cam;

    private bool _isDashing;

    private Vector3 _moveDir;
    private Vector3 _dirToMouse;

    #region Properties

    public IState IdleState => _idleState;
    public IState WalkState => _walkState;
    public IState RunState => _runState;
    public IState SprintState => _sprintState;

    public Vector3 MoveDir => _moveDir;
    public PlayerInput Input => _input;
    public List<MovementType> PlayerMovements => _playerMovements;
    public MovementType CurrentMoveType => _currentMovement;
    public GameObject GameObject => this.gameObject;
    public StaminaSystem Stamina => _stamina;

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
        _stateMachine = new StateMachine();
        _idleState = new PlayerIdle(this, _stateMachine, _playerAnimator, _body);
        _walkState = new PlayerWalk(this, _body, _playerAnimator,_stateMachine);
        _runState = new PlayerRun(this, _body, _playerAnimator, _stateMachine);
        _sprintState = new PlayerSprint(this, _body, _playerAnimator, _stateMachine);
        _stateMachine.SetStartState(_idleState);
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
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _dirToMouse = hit.point - transform.position;
                _dirToMouse.y = 0;
                _dirToMouse.Normalize();
                RotateTowards(_dirToMouse);
            }
        }
    }

    private void RotateTowards(Vector3 dir)
    {
        Quaternion r = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, r, _rotationSpeed * Time.deltaTime);
    }

    private void HandleRun(InputAction.CallbackContext obj)
    {
        _stateMachine.ChangeState(RunState);
    }


    private void EnableSprint(InputAction.CallbackContext obj)
    {
    }

    private void DisableSprint()
    {
    }


    private void Dash()
    {
        Stamina.ResetRegenTimer();

        if(_moveDir != Vector3.zero && !_isDashing && Stamina.CurrentValue > 0)
            StartCoroutine(Dashing(_dashCooldown, _moveDir));
    }

    private IEnumerator Dashing(float dashCooldown, Vector3 dir)
    {
        float speed = _playerAnimator.speed;
        _playerAnimator.speed = 0;
        yield return new WaitForEndOfFrame();
        _playerAnimator.speed = speed;
        _isDashing = true;
        _body.AddRelativeForce(dir.normalized * _dashForce, ForceMode.Impulse);
        Stamina.CurrentValue -= _staminaPerDash;
        yield return new WaitForSeconds(dashCooldown);
        _isDashing = false;
    }


    private void GetMoveVector(InputAction.CallbackContext value)
    {
        var input = value.ReadValue<Vector2>();
        _moveDir = new Vector3(input.x, 0, input.y);
    }

    private void SetInputs()
    {
        _input.Player.Move.performed += GetMoveVector;
        _input.Player.Run.performed += HandleRun;
        _input.Player.Sprint.performed += EnableSprint;
        _input.Player.Sprint.canceled += _ => DisableSprint();
        _input.Player.Dash.performed += _ => Dash();
        _input.Player.Interact.performed += _ => _lastInteractableObject?.Interact(this);
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

    public async void FocusToInteractable(Transform objectToFocus)
    {
        var pickAbleItem = objectToFocus.GetComponent<PickableItem>();
        if(pickAbleItem)
        {
            ToogleInput(false);
            Vector3 dirToItem = (objectToFocus.position - transform.position).normalized;
            while (Vector3.Distance(transform.position, pickAbleItem.transform.position) > pickAbleItem.PickUpDistance)
            {
                Vector3 rotateDir = new Vector3(dirToItem.x, transform.position.y, dirToItem.z);
                RotateTowards(rotateDir);
              //  Move(dirToItem, true);
                await Task.Yield();
            }
            _playerAnimator.SetTrigger("Take");
            pickAbleItem.DisableItemOnScene();
        }
    }

    private bool ToogleInput(bool value)
    {
        bool i = value;

        if (i == true)
            _input.Enable();
        else
            _input.Disable();

        return i;
    }    

    public void ResetInteractableFocus() => ToogleInput(true);
}

