using Project.Core.Features.Weapon.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.Features.Weapon.Gameplay
{
    [CreateAssetMenu(menuName = "Custom/Weapon Definitions Catalog")]
    public class WeaponDefinitionsCatalog : ScriptableObject
    {
        [SerializeField] private List<WeaponDefinition> _weaponDefinitions;

        public List<WeaponDefinition> WeaponDefinitions => _weaponDefinitions;
    }
}