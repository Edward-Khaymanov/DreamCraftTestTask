using Project.Core.Features.Projectile;
using System;
using UnityEngine;

namespace Project.Core.Features.Weapon.Core
{
    [Serializable]
    public class WeaponDefinition
    {
        public string Name;
        public Sprite Sprite;
        public WeaponType WeaponType;
        public WeaponAttackMode AttackMode;
        public float AttackRateInSeconds;
        public float Damage;
        public ProjectileDefinition ProjectileDefinition;
    }
}