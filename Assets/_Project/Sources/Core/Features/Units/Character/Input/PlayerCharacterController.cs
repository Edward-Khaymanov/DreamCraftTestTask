using Project.Core.Features.Units.Character.Core;
using UnityEngine;

namespace Project.Core.Features.Units.Character.Input
{
    public class PlayerCharacterController : MonoBehaviour
    {
        private bool _enabled;
        private ICharacterInput _input;
        private ICharacter _character;

        public void Init(ICharacterInput input, ICharacter character)
        {
            _input = input;
            _character = character;
        }

        private void OnDestroy()
        {
            Disable();
        }

        private void Update()
        {
            if (_enabled == false)
                return;

        }

        private void FixedUpdate()
        {
            if (_enabled == false)
                return;

            if (_input.Move != Vector2.zero)
            {
                _character.MoveToDirection(_input.Move);
            }

            var lookDirection = _input.LookDirection;
            var angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            var delta = Mathf.DeltaAngle(_character.Rotation, angle);
            delta = Mathf.Abs(delta);
            if (delta > 0.1f)
            {
                _character.Rotate(angle);
            }
        }

        public void Enable()
        {
            _input.Enable();
            _input.PrimaryActionStarted += OnPrimaryActionStarted;
            _input.PrimaryActionStopped += OnPrimaryActionStopped;
            _input.SlotSelected += OnSlotSelected;
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
            _input.Disable();
            _input.PrimaryActionStarted -= OnPrimaryActionStarted;
            _input.PrimaryActionStopped -= OnPrimaryActionStopped;
            _input.SlotSelected -= OnSlotSelected;
        }

        private void OnPrimaryActionStarted()
        {
            _character.StartWeaponPrimaryAction();
        }

        private void OnPrimaryActionStopped()
        {
            _character.StopWeaponPrimaryAction();
        }

        private void OnSlotSelected(int slot)
        {
            _character.EquipSlot(slot);
        }
    }
}