using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
    [SerializeField] private List<PlayerSpeed> _playerSpeeds = new List<PlayerSpeed>();

    private PlayerSpeed _currentSpeed;
    private PlayerInput _input;
    private Camera _cam;

    private bool _isDashing;
    private bool _isSprint;
    private bool _isRun;

    private Vector3 _moveDir;
    private Vector3 _dirToMouse;
    

    private void Awake()
    {
        _cam = Camera.main;
        _input = new PlayerInput();
        _input.Enable();
    }

    private void OnEnable()
    {
        _playerUI.InitPlayerValues(_health, _stamina);
        SetInputs();
    }
   

    private void Start()
    {
        ChangeCurrentSpeedType(PlayerSpeed.SpeedType.Walk);
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Update()
    {
        RotateTowards();
    }

    private void FixedUpdate()
    {
        Move();
        Friciton();
    }

    private void ChangeCurrentSpeedType(PlayerSpeed.SpeedType type)
    {
        _currentSpeed = _playerSpeeds.Where(s => s.Type == type).FirstOrDefault();
    }

    private void RotateTowards()
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
                Quaternion r = Quaternion.LookRotation(_dirToMouse, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, r, _rotationSpeed * Time.deltaTime);
            }
        }
       
    }

    private void HandleRun(InputAction.CallbackContext obj)
    {
        _isRun = !_isRun;
        _playerAnimator.SetBool("Run", _isRun);

        if (_isRun)
            ChangeCurrentSpeedType(PlayerSpeed.SpeedType.Run);
        else
            ChangeCurrentSpeedType(PlayerSpeed.SpeedType.Walk);
    }


    private void EnableSprint(InputAction.CallbackContext obj)
    {
        if(!_isSprint && _stamina.CurrentValue > 0)
        {
            _isSprint = true;
            ChangeCurrentSpeedType(PlayerSpeed.SpeedType.Sprint);
            _playerAnimator.SetBool("Sprint", _isSprint);
            _playerAnimator.SetBool("Run", !_isSprint);
        }
    }

    private void DisableSprint()
    {
        if(_isSprint)
        {
            _isSprint = false;
            ChangeCurrentSpeedType(PlayerSpeed.SpeedType.Run);
            _playerAnimator.SetBool("Sprint", _isSprint);
            _playerAnimator.SetBool("Run", !_isSprint);
        }
    }

    private void Friciton()
    {
        if (_body.velocity.magnitude > 0)
        {
            var oppositeDir = -_body.velocity * _currentSpeed.Friction;
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

    private void Move()
    {
        Vector3 target = _body.transform.InverseTransformDirection(_body.velocity);
        float velocityZ = target.z;
        float velocityX = target.x;

        _playerAnimator.SetFloat("InputY", velocityZ, 0.1f, Time.deltaTime);
        _playerAnimator.SetFloat("InputX", velocityX, 0.1f, Time.deltaTime);
        _body.AddRelativeForce(_moveDir.normalized * _currentSpeed.Acceleration * Time.deltaTime);

        if(_moveDir != Vector3.zero)
            _currentSpeed.ReduceStamina(_stamina);

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
        // _input.Player.Run.canceled += HandleRun; // for gamepad
        _input.Player.Sprint.performed += EnableSprint;
        _input.Player.Sprint.canceled += _ => DisableSprint();
        _input.Player.Dash.performed += _ => Dash();
    }
}

[Serializable]
public class PlayerSpeed
{
    public enum SpeedType
    {
        Walk,
        Run,
        Sprint
    }

    [field: SerializeField] public SpeedType Type { get; private set; }
    [field : SerializeField, Range(0,10000)] public float Acceleration { get; private set; }
    [field: SerializeField, Range(0, 100)] public float StaminaPerSec { get; private set; }
    public float Friction { get; private set; } = 500;

    public void ReduceStamina(StaminaSystem stamina)
    {
        stamina.CurrentValue -= StaminaPerSec * Time.deltaTime;
    }

}
