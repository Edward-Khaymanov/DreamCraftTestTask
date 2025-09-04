using System;

namespace Project.Core.Features.Units.Enemy.Core
{
    [Serializable]
    public class EnemyData
    {
        public float Health;
        public float MoveSpeed;
        public float AttackCooldown;
        public float Damage;
    }
}