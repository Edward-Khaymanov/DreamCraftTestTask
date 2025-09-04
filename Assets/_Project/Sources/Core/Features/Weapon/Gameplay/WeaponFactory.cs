using Project.Core.Features.Projectile;
using Project.Core.Features.Weapon.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.Features.Weapon.Gameplay
{
    public class WeaponFactory : IWeaponFactory
    {
        private IDictionary<string, WeaponDefinition> _namesDefinitions;
        private IProjectileFactory _projectileFactory;

        public WeaponFactory(List<WeaponDefinition> weaponDefinitions, IProjectileFactory projectileFactory)
        {
            _namesDefinitions = new Dictionary<string, WeaponDefinition>();
            _projectileFactory = projectileFactory;

            foreach (var definition in weaponDefinitions)
            {
                _namesDefinitions.TryAdd(definition.Name, definition);
            }
        }

        public IWeapon CreateWeapon(string name)
        {
            if (_namesDefinitions.TryGetValue(name, out WeaponDefinition weaponDefinition) == false)
            {
                Debug.LogError($"Weapon with name {name} is not found");
                return null;
            }

            switch (weaponDefinition.WeaponType)
            {
                case WeaponType.StraightShot:
                    return new StraightShot(weaponDefinition, _projectileFactory);
                case WeaponType.SpreadShot:
                    return new SpreadShot(weaponDefinition, _projectileFactory);
                default:
                    Debug.LogError($"Type for weapon {name} is not found");
                    return null;
            }
        }
    }
}