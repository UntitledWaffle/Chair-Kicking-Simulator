using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

namespace HealthbarGames
{
    public class DemoWeaponInput : WeaponInput
    {
        private InputAction fire1Action;
        private InputAction fire2Action;
        private InputActionMap playerInput;

        private void Awake()
        {
            // Set up input actions
            playerInput = new InputActionMap("Player");
            fire1Action = playerInput.AddAction("Fire1", binding: "<Mouse>/leftButton");
            fire2Action = playerInput.AddAction("Fire2", binding: "<Mouse>/rightButton");

            // Enable the input actions
            playerInput.Enable();
        }

        public override bool GetActionActivate(WeaponInput.Action action)
        {
            if (action == WeaponInput.Action.Primary)
                return fire1Action.triggered;
            else
                return fire2Action.triggered;
        }

        public override bool GetActionState(WeaponInput.Action action)
        {
            if (action == WeaponInput.Action.Primary)
                return fire1Action.ReadValue<float>() > 0;
            else
                return fire2Action.ReadValue<float>() > 0;
        }

        public override bool GetActionDeactivate(WeaponInput.Action action)
        {
            if (action == WeaponInput.Action.Primary)
                return fire1Action.phase == InputActionPhase.Canceled;
            else
                return fire2Action.phase == InputActionPhase.Canceled;
        }

        private void OnDestroy()
        {
            playerInput?.Dispose();
        }
    }
}