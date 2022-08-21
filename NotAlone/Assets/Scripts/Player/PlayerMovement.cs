using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
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
    private Camera _cam;

    private Vector3 _moveDir;
    private Vector3 _dirToMouse;

    #region Properties
  
    public Vector3 MoveDir => _moveDir;
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
    }

    private void OnEnable()
    {
        _playerUI.InitPlayerValues(_health, Stamina);
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

}

