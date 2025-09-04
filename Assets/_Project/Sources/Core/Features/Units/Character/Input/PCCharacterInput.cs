using Project.Core.Features.Units.Character.Gameplay;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Core.Features.Units.Character.Input
{
    public class PCCharacterInput : ICharacterInput
    {
        private InputActions _inputActions;
        private CharacterCamera _characterCamera;
        private Func<Vector3> _getCharacterPosition;

        public event Action PrimaryActionStarted;
        public event Action PrimaryActionStopped;
        public event Action<int> SlotSelected;

        public Vector2 Move => _inputActions.Character.Move.ReadValue<Vector2>();
        public Vector2 LookDirection => GetLookDirection();

        public PCCharacterInput(Func<Vector3> getCharacterPosition, CharacterCamera characterCamera)
        {
            _inputActions = new InputActions();
            _getCharacterPosition = getCharacterPosition;
            _characterCamera = characterCamera;
        }

        public void Enable()
        {
            _inputActions.Character.Enable();

            _inputActions.Character.Shoot.started += OnShootStarted;
            _inputActions.Character.Shoot.canceled += OnShootCanceled;

            _inputActions.Character.Weapon1.performed += OnSlot1Selected;
            _inputActions.Character.Weapon2.performed += OnSlot2Selected;
        }

        public void Disable()
        {
            _inputActions.Character.Disable();

            _inputActions.Character.Shoot.started -= OnShootStarted;
            _inputActions.Character.Shoot.canceled -= OnShootCanceled;

            _inputActions.Character.Weapon1.performed -= OnSlot1Selected;
            _inputActions.Character.Weapon2.performed -= OnSlot2Selected;
        }

        private Vector2 GetLookDirection()
        {
            var mousePos = _inputActions.Character.Look.ReadValue<Vector2>();
            var worldPos = _characterCamera.Camera.ScreenToWorldPoint(mousePos);
            var direction = worldPos - _getCharacterPosition();
            return direction.normalized;
        }

        private void OnShootStarted(InputAction.CallbackContext ctx)
        {
            PrimaryActionStarted?.Invoke();
        }

        private void OnShootCanceled(InputAction.CallbackContext ctx)
        {
            PrimaryActionStopped?.Invoke();
        }

        private void OnSlot1Selected(InputAction.CallbackContext ctx)
        {
            SlotSelected?.Invoke(1);
        }

        private void OnSlot2Selected(InputAction.CallbackContext ctx)
        {
            SlotSelected?.Invoke(2);
        }
    }
}