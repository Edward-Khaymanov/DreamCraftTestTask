using Project.Core.Features.Projectile;
using Project.Core.Features.Weapon.Core;

namespace Project.Core.Features.Weapon.Gameplay
{
    public class StraightShot : WeaponBase
    {
        public StraightShot(WeaponDefinition definition, IProjectileFactory projectileFactory) : base(definition, projectileFactory)
        {
        }

        protected override void PrimaryAction()
        {
            var projectile = ProjectileFactory.CreateProjectile(Definition.ProjectileDefinition.Name, Origin.position);
            projectile.SetSource(Owner);
            projectile.SetDamage(Definition.Damage);
            projectile.MoveTo(Owner.Transform.right, Definition.ProjectileDefinition.Speed);
            projectile.StartLifeTimer();
        }
    }
}