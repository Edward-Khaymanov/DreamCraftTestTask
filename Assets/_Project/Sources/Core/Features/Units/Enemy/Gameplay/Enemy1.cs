using Cysharp.Threading.Tasks;
using Project.Core.Common;
using Project.Core.Features.Units.Character.Core;
using Project.Core.Features.Units.Enemy.Core;
using Project.Utils;
using System;
using System.Threading;
using UnityEngine;

namespace Project.Core.Features.Units.Enemy.Gameplay
{

    public class Enemy1 : EnemyBase
    {
        private float _attackCooldown;
        private float _damage;
        private float _currentAttackCooldown;
        private ICharacter _victim;
        private CancellationTokenSource _chaseTokenSource;
        private CancellationTokenSource _attackCooldownTokenSource;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<ICharacter>(out var character) == false)
                return;

            if (character != _victim)
                return;

            TryAttackCharacter(character);
        }

        protected override void OnInit(EnemyData enemyData)
        {
            _attackCooldown = enemyData.AttackCooldown;
            _damage = enemyData.Damage;
        }

        protected override void OnClearState()
        {
            CancellationTokenUtils.Destroy(ref _chaseTokenSource);
            CancellationTokenUtils.Destroy(ref _attackCooldownTokenSource);
            _currentAttackCooldown = 0;
            _victim = null;
        }

        public override void SeeCharacter(ICharacter character)
        {
            _victim = character;
            CancellationTokenUtils.Destroy(ref _chaseTokenSource);
            _chaseTokenSource = new CancellationTokenSource();
            Chase(_victim, _chaseTokenSource.Token).Forget();
        }

        protected override void OnDying()
        {
            _chaseTokenSource?.Cancel();
            _attackCooldownTokenSource?.Cancel();
        }

        private async UniTaskVoid Chase(ICharacter character, CancellationToken cancellationToken)
        {
            while (character.IsAlive && cancellationToken.IsCancellationRequested == false && ObjectLifetimeToken.IsCancellationRequested == false)
            {
                await UniTask.WaitForFixedUpdate(cancellationToken);
                if (ObjectLifetimeToken.IsCancellationRequested)
                    return;

                var direction = character.Position - Position;
                Movement.MakeStep(direction);

                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                var delta = Mathf.DeltaAngle(Rotation, angle);
                delta = Mathf.Abs(delta);
                if (delta > 0.1f)
                {
                    Movement.Rotate(angle);
                }
            }
        }

        private bool TryAttackCharacter(ICharacter character)
        {
            if (_currentAttackCooldown > 0)
                return false;

            if (character.IsAlive == false)
                return false;

            var damageInfo = new DamageInfo()
            {
                Source = this,
                Damage = _damage,
            };
            character.ApplyDamage(damageInfo);

            CancellationTokenUtils.Destroy(ref _attackCooldownTokenSource);
            _attackCooldownTokenSource = new CancellationTokenSource();
            WaitAttackCooldown(_attackCooldown, _attackCooldownTokenSource.Token).Forget();

            return true;
        }

        private async UniTaskVoid WaitAttackCooldown(float attackCooldown, CancellationToken cancellationToken)
        {
            _currentAttackCooldown = attackCooldown;
            var cooldown = TimeSpan.FromSeconds(attackCooldown);
            await UniTask.Delay(cooldown, cancellationToken: cancellationToken);
            if (ObjectLifetimeToken.IsCancellationRequested)
                return;

            _currentAttackCooldown = 0;
        }
    }
}