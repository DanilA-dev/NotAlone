using System;
using UnityEngine;

namespace Player.States
{
    public class PlayerStateController : MonoBehaviour
    {
        private StateMachine _stateMachine;
        private StaminaSystem _stamina;
        private PlayerStatesFabric _states;
        private IInteractable _lastInteractableObject;

        public StateMachine StateMachine => _stateMachine;

        public void Init(PlayerMovement playerMovement, StaminaSystem stamina)
        {
            _stateMachine = new StateMachine();
            _states = new PlayerStatesFabric(playerMovement, this);
            _stamina = stamina;

            _stateMachine.SetStartState(_states.Idle());
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedTick();
        }

        public void HandleDash()
        {
            if (_states.Dash().CanDash())
                _stateMachine.ChangeState(_states.Dash());
        }

        public void HandleRun()
        {
            if (_stateMachine.CurrentState != _states.Run())
                _stateMachine.ChangeState(_states.Run());
            else
                _stateMachine.ChangeState(_states.Walk());
        }

        public void HandleSprint()
        {
            if (_stateMachine.CurrentState != _states.Sprint() && _stamina.CurrentValue > 0)
                _stateMachine.ChangeState(_states.Sprint());
        }

        public void HandleCancelSprint()
        {
            if (_stateMachine.CurrentState == _states.Sprint())
                _stateMachine.ChangeState(_stateMachine.PreviousState);
        }

        public void HandleInteraction()
        {
            if(_lastInteractableObject != null)
            {
                _lastInteractableObject?.Interact(_states.ObjectInteract());
                _stateMachine.ChangeState(_states.ObjectInteract());
            }
        }

        public void HandleInteractionCancel()
        {
            _stateMachine.ChangeState(_states.Idle());
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

}

