using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Rigidbody _body;
    [Space]
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _dashForce;
    [SerializeField] private float _staminaPerDash;
    [SerializeField] private float _dashCooldown;
    [Space]
    [SerializeField] private List<MovementType> _playerMovements = new List<MovementType>();


    private StaminaSystem _stamina;
    private MovementType _currentMovement;
    private Camera _cam;

    private Vector3 _moveDir;
    private Vector3 _dirToMouse;

    #region Properties
  
    public Vector3 MoveDir => _moveDir;
    public Rigidbody Body => _body;
    public Animator Animator => _playerAnimator;
    public List<MovementType> PlayerMovements => _playerMovements;
    public StaminaSystem Stamina => _stamina;
    public MovementType CurrentMoveType => _currentMovement;
    public GameObject Interactor => this.gameObject;
    public float DashForce => _dashForce;
    public float DashCooldown => _dashCooldown;
    public float DashStamina => _staminaPerDash;


    #endregion

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        if(_moveDir != Vector3.zero)
        {
            Quaternion r = Quaternion.LookRotation(-_moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, r, _rotationSpeed * Time.deltaTime);
        }
    }

    public void Init(StaminaSystem stamina)
    {
        _stamina = stamina;
    }

    public void UpdateMoveVector(Vector2 vector)
    {
        _moveDir = new Vector3(vector.x, 0, vector.y);
    }

    public void ChangeCurrentSpeedType(MovementType type)
    {
        _currentMovement = type;
    }

    private void RotateTowardsMouse(Vector2 delta)
    {
        Ray ray = _cam.ScreenPointToRay(delta);
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

