using Project.Core.Features.Units;
using UnityEngine;

namespace Project.Core.Features.Weapon.Core
{
    public interface IWeapon
    {
        public WeaponDefinition Definition { get; }

        public void SetOrigin(Transform origin);
        public void SetOwner(IUnit owner);
        public void StartPrimaryAction();
        public void StopPrimaryAction();
    }
}