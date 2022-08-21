using UnityEngine;
using Input;
using Player.States;
using System;

namespace Player
{
    public class PlayerInitializer : MonoBehaviour
    {
        private InputRouter _inputRouter;
        private PlayerMovement _movement;
        private PlayerStateController _stateController;


        private void OnEnable()
        {
            _inputRouter = _inputRouter ?? gameObject.AddComponent<InputRouter>();
            SubscribeToActions();
        }

        private void SubscribeToActions()
        {
        }
    }

}
