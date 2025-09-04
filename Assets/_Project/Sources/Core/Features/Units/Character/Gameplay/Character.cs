using Project.Core.Common;
using Project.Core.Features.Units.Character.Core;
using Project.Core.Features.Weapon.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Core.Features.Units.Character.Gameplay
{
    public class Character : Unit, ICharacter
    {
        [SerializeField] private CharacterView _view;
        [SerializeField] private Transform _weaponOrigin;

        private RigidbodyMover _movement;
        private IHealth _health;
        private IDictionary<int, IWeapon> _slotWeapons;
        private int _activeSlot;

        public event Action<ICharacter> Died;

        public bool IsAlive => _health.Current != 0;
        public IHealth Health => _health;

        public void Init(CharacterData characterData)
        {
            _movement = new RigidbodyMover(Rigidbody);
            _health = new Health();
            _slotWeapons = new Dictionary<int, IWeapon>();

            _movement.SetSpeed(characterData.MoveSpeed);
            _health.SetTotal(characterData.TotalHealth);
            _health.SetCurrent(characterData.TotalHealth);
            _health.Exhausted += OnHealthExhausted;
        }

        private void OnDestroy()
        {
            _health.Exhausted -= OnHealthExhausted;
        }

        public override void ApplyDamage(DamageInfo damageInfo)
        {
            _health.Remove(damageInfo.Damage);
        }

        public void MoveToDirection(Vector2 direction)
        {
            _movement.MakeStep(direction);
        }

        public void Rotate(float angle)
        {
            _movement.Rotate(angle);
        }

        public void StartWeaponPrimaryAction()
        {
            if (_slotWeapons.TryGetValue(_activeSlot, out var weapon) == false)
                return;

            weapon.StartPrimaryAction();
        }

        public void StopWeaponPrimaryAction()
        {
            if (_slotWeapons.TryGetValue(_activeSlot, out var weapon) == false)
                return;

            weapon.StopPrimaryAction();
        }

        public void AddWeapon(IWeapon weapon, int slot = 0)
        {
            if (slot == 0)
                slot = GetNextFreeSlot();

            if (_slotWeapons.TryAdd(slot, weapon))
            {
                weapon.SetOrigin(_weaponOrigin);
                weapon.SetOwner(this);
            }
        }

        public void EquipSlot(int slot)
        {
            if (_activeSlot == slot)
                return;

            if (_slotWeapons.TryGetValue(slot, out var weapon) == false)
                return;

            StopWeaponPrimaryAction();
            _activeSlot = slot;
            _view.SetWeapon(weapon.Definition.Sprite);
        }

        private int GetNextFreeSlot()
        {
            return Enumerable.Range(1, _slotWeapons.Count + 1)
                .Except(_slotWeapons.Keys)
                .First();
        }

        private void OnHealthExhausted()
        {
            Died?.Invoke(this);
        }
    }
}
