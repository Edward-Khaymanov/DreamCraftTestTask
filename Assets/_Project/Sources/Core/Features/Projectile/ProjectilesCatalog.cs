using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.Features.Projectile
{
    [CreateAssetMenu(menuName = "Custom/Projectiles Catalog")]
    public class ProjectilesCatalog : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<string, Projectile> _namesProjectiles;

        public IDictionary<string, Projectile> NamesProjectiles => _namesProjectiles;
    }
}