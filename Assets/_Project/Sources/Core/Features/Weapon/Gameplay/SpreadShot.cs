using Project.Core.Features.Projectile;
using Project.Core.Features.Weapon.Core;
using Project.Utils;
using UnityEngine;

namespace Project.Core.Features.Weapon.Gameplay
{
    public class SpreadShot : WeaponBase
    {
        public SpreadShot(WeaponDefinition definition, IProjectileFactory projectileFactory) : base(definition, projectileFactory)
        {
        }

        protected override void PrimaryAction()
        {
            CreateProjectile();
            CreateProjectile(5f);
            CreateProjectile(-5f);
        }

        private void CreateProjectile(float angle = 0f)
        {
            var projectile = ProjectileFactory.CreateProjectile(Definition.ProjectileDefinition.Name, Origin.position);
            var direction = ((Vector2)Owner.Transform.right).Rotate(angle).normalized;

            projectile.SetSource(Owner);
            projectile.SetDamage(Definition.Damage);
            projectile.MoveTo(direction, Definition.ProjectileDefinition.Speed);
            projectile.StartLifeTimer();
        }
    }
}
