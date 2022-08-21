using UnityEngine;

namespace Player.States
{
    public class PlayerStateController : MonoBehaviour
    {
        private StateMachine _stateMachine;
        private StaminaSystem _stamina;
        private PlayerStatesFabric _states;

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
            //to change state need to check(overlap sphere for example) interactables around player
            //if nothing in zone return

            //_lastInteractableObject?.Interact(_statesFabic.ObjectInteract());
            //_stateMachine.ChangeState(_statesFabic.ObjectInteract());
        }

        public void HandleInteractionCancel()
        {
            _stateMachine.ChangeState(_states.Idle());
        }

    }

}

