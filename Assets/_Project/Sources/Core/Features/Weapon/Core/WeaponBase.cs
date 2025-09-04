using Project.Core.Features.Projectile;
using Project.Core.Features.Units;
using UnityEngine;

namespace Project.Core.Features.Weapon.Core
{
    public abstract class WeaponBase : IWeapon
    {
        protected IProjectileFactory ProjectileFactory;
        protected float LastAttackTime;
        protected IUnit Owner;
        protected Transform Origin;

        public WeaponBase(WeaponDefinition definition, IProjectileFactory projectileFactory)
        {
            Definition = definition;
            ProjectileFactory = projectileFactory;
        }

        public WeaponDefinition Definition { get; private set; }

        public void SetOrigin(Transform origin)
        {
            Origin = origin;
        }

        public void SetOwner(IUnit owner)
        {
            Owner = owner;
        }

        public void StartPrimaryAction()
        {
            if (Definition.AttackMode == WeaponAttackMode.Single)
            {
                if (CanPrimaryAction())
                {
                    PrimaryAction();
                    LastAttackTime = Time.time;
                }
            }
        }

        public void StopPrimaryAction()
        {

        }

        protected abstract void PrimaryAction();

        protected virtual bool CanPrimaryAction()
        {
            return Time.time >= LastAttackTime + Definition.AttackRateInSeconds;
        }
    }
}
