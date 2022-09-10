using UnityEngine;
using Input;
using Player.States;
using System;

namespace Player
{
    public class PlayerInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerUI _playerUI;

        private InputRouter _inputRouter;
        private HealthSystem _healthSystem;
        private StaminaSystem _staminaSystem;
        private PlayerMovement _movement;
        private PlayerStateController _stateController;
        

        private void OnEnable()
        {
            _movement = GetComponent<PlayerMovement>();
            _stateController = GetComponent<PlayerStateController>();
            _healthSystem = GetComponent<HealthSystem>();
            _staminaSystem = GetComponent<StaminaSystem>();

            _inputRouter = _inputRouter ?? gameObject.AddComponent<InputRouter>();
            InitComponents();
            SubscribeToActions();
        }

        private void InitComponents()
        {
            _playerUI.InitPlayerValues(_healthSystem, _staminaSystem);
            _movement.Init(_staminaSystem);
            _stateController.Init(_movement, _staminaSystem);
        }

        private void SubscribeToActions()
        {
            _inputRouter.MoveVector += _movement.UpdateMoveVector;
        }
    }

}
