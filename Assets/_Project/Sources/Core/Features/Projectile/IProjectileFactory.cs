using UnityEngine;

namespace Project.Core.Features.Projectile
{
    public interface IProjectileFactory
    {
        public Projectile CreateProjectile(string name, Vector3 position);
    }
}