using System;
using UnityEngine;

namespace Project.Core.Features.Units.Character.Input
{
    public interface ICharacterInput
    {
        public event Action PrimaryActionStarted;
        public event Action PrimaryActionStopped;
        public event Action<int> SlotSelected;

        public Vector2 Move { get; }
        public Vector2 LookDirection { get; }

        public void Enable();
        public void Disable();
    }
}