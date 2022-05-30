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

    private IInteractable _lastInteractableObject;
    private MovementType _currentMovement;
    private PlayerInput _input;
    private Camera _cam;

    private bool _isDashing;
    private bool _isSprint;
    private bool _isRun;

    private Vector3 _moveDir;
    private Vector3 _dirToMouse;

    #region Properties

    public GameObject GameObject => this.gameObject;

    #endregion

    private void Awake()
    {
        _cam = Camera.main;
        _input = new PlayerInput();
        ToogleInput(true);
    }

    private void OnEnable()
    {
        _playerUI.InitPlayerValues(_health, _stamina);
        ChangeCurrentSpeedType(MovementType.SpeedType.Walk);
        SetInputs();
    }

    private void OnDisable()
    {
        ToogleInput(false);
    }

    private void Update()
    {
        RotateTowardsMouse();
    }

    private void FixedUpdate()
    {
        Move(_moveDir, false);
        Friciton();
    }

    private void ChangeCurrentSpeedType(MovementType.SpeedType type)
    {
        _currentMovement = _playerMovements.Where(s => s.Type == type).FirstOrDefault();
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
        _isRun = !_isRun;
        _playerAnimator.SetBool("Run", _isRun);

        if (_isRun)
            ChangeCurrentSpeedType(MovementType.SpeedType.Run);
        else
            ChangeCurrentSpeedType(MovementType.SpeedType.Walk);
    }


    private void EnableSprint(InputAction.CallbackContext obj)
    {
        if(!_isSprint && _stamina.CurrentValue > 0)
        {
            _isSprint = true;
            ChangeCurrentSpeedType(MovementType.SpeedType.Sprint);
            _playerAnimator.SetBool("Sprint", _isSprint);
            _playerAnimator.SetBool("Run", !_isSprint);
        }
    }

    private void DisableSprint()
    {
        if(_isSprint)
        {
            _isSprint = false;
            ChangeCurrentSpeedType(MovementType.SpeedType.Run);
            _playerAnimator.SetBool("Sprint", _isSprint);
            _playerAnimator.SetBool("Run", !_isSprint);
        }
    }

    private void Friciton()
    {
        if (_body.velocity.magnitude > 0)
        {
            var oppositeDir = -_body.velocity * _currentMovement.Friction;
            _body.AddForce(oppositeDir * Time.deltaTime);
        }
    }

    private void Dash()
    {
        _stamina.RestRegenTimer();

        if(_moveDir != Vector3.zero && !_isDashing && _stamina.CurrentValue > 0)
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
        _stamina.CurrentValue -= _staminaPerDash;
        yield return new WaitForSeconds(dashCooldown);
        _isDashing = false;
    }

    private void Move(Vector3 moveDir, bool useGlobalCoordinates)
    {
        Vector3 target = _body.transform.InverseTransformDirection(_body.velocity);
        float velocityZ = target.z;
        float velocityX = target.x;

        _playerAnimator.SetFloat("InputY", velocityZ, 0.1f, Time.deltaTime);
        _playerAnimator.SetFloat("InputX", velocityX, 0.1f, Time.deltaTime);

        if(useGlobalCoordinates)
            _body.AddForce(moveDir.normalized * _currentMovement.Acceleration * Time.deltaTime);
        else
            _body.AddRelativeForce(moveDir.normalized * _currentMovement.Acceleration * Time.deltaTime);

        if (_moveDir != Vector3.zero)
            _currentMovement.ReduceStamina(_stamina);

        if (_isSprint)
            _stamina.RestRegenTimer();

        if (_isSprint && _moveDir.z < 0 || _moveDir.x != 0 || _stamina.CurrentValue <= 0)
            DisableSprint();
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
                Move(dirToItem, true);
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

