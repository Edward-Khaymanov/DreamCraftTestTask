using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.Features.Projectile
{
    public class ProjectileFactory : IProjectileFactory
    {
        private IDictionary<string, Projectile> _namesProjectiles;

        public ProjectileFactory(IDictionary<string, Projectile> namesProjectiles)
        {
            _namesProjectiles = new Dictionary<string, Projectile>(namesProjectiles);
        }

        public Projectile CreateProjectile(string name, Vector3 position)
        {
            if (_namesProjectiles.TryGetValue(name, out var projectile) == false)
            {
                Debug.LogError($"Projectile with name {name} is not found");
                return null;
            }

            var result = GameObject.Instantiate(projectile, position, Quaternion.identity);
            return result;
        }
    }
}