using System;
using UnityEngine;

namespace Input
{
    public class InputRouter : MonoBehaviour
    {
        private PlayerInput _input;

        public event Action Mouse0Click;
        public event Action Mouse1Click;
        public event Action Mouse0Canceled;
        public event Action Mouse1Canceled;
        public event Action Mouse0Hold;
        public event Action Mouse1Hold;

        public event Action Interact;
        public event Action Dash;

        public event Action<Vector2> MoveVector;
        public event Action<Vector2> MouseDelta;

        private Vector2 _moveDir;
        private Vector2 _mouseDelta;

        private void Awake()
        {
            _input = new PlayerInput();
        }

        private void OnEnable()
        {
            TurnInputOn();
        }

        private void OnDisable()
        {
            TurnInputOff();
        }

        private void Start()
        {
            KeyBoundsActions();
        }

        private void Update()
        {
            UpdateMoveVector();
            UpdateMouseDelta();
            UpdateMouseHold();
        }

        private void UpdateMouseHold()
        {
            if (_input.Player.Fire0.IsPressed())
                Mouse0Hold?.Invoke();

            if (_input.Player.Fire1.IsPressed())
                Mouse1Hold?.Invoke();
        }

        private void UpdateMouseDelta()
        {
            _mouseDelta = _input.Player.Look.ReadValue<Vector2>();
            MouseDelta?.Invoke(_mouseDelta);
        }

        private void UpdateMoveVector()
        {
            _moveDir = _input.Player.Move.ReadValue<Vector2>();
            MoveVector?.Invoke(_moveDir);
        }

        private void KeyBoundsActions()
        {
            _input.Player.Fire0.performed += _ => Mouse0Click?.Invoke();
            _input.Player.Fire0.canceled += _ => Mouse0Canceled?.Invoke();
            _input.Player.Fire1.performed += _ => Mouse1Click?.Invoke();
            _input.Player.Fire1.canceled += _ => Mouse1Canceled?.Invoke();

            _input.Player.Interact.performed += _ => Interact?.Invoke();
            _input.Player.Dash.performed += _ => Dash?.Invoke();
        }

        public void TurnInputOn()
        {
            _input.Enable();
        }

        public void TurnInputOff()
        {
            _input.Disable();
        }
    }

}
