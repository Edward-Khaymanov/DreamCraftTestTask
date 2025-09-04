using Project.Core.Common;
using Project.Core.Features.Weapon.Core;
using System;
using UnityEngine;

namespace Project.Core.Features.Units.Character.Core
{
    public interface ICharacter : IUnit
    {
        public event Action<ICharacter> Died;

        public bool IsAlive { get; }
        public IHealth Health { get; }

        public void Init(CharacterData characterData);
        public void MoveToDirection(Vector2 direction);
        public void Rotate(float angle);
        public void StartWeaponPrimaryAction();
        public void StopWeaponPrimaryAction();
        public void AddWeapon(IWeapon weapon, int slot = 0);
        public void EquipSlot(int slot);
    }
}