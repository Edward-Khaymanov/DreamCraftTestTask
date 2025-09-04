using Cysharp.Threading.Tasks;
using Project.Core.Common;
using Project.Core.Features.Units.Character.Core;
using Project.Infrastructure.Pool;
using Project.Utils;
using System;
using System.Threading;

namespace Project.Core.Features.Units.Enemy.Core
{
    public abstract class EnemyBase : Unit, IPoolableObject<EnemyBase>
    {
        private IObjectPool<EnemyBase> _pool;
        private CancellationTokenSource _lifetimeTokenSource;

        protected RigidbodyMover Movement;
        protected IHealth Health;

        public event Action<EnemyBase> Died;

        protected CancellationToken ObjectLifetimeToken { get; private set; }

        public void Init(EnemyData enemyData)
        {
            _lifetimeTokenSource = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
            ObjectLifetimeToken = _lifetimeTokenSource.Token;
            Movement = new RigidbodyMover(Rigidbody);
            Health = new Health();
            Health.SetTotal(enemyData.Health);
            Health.SetCurrent(enemyData.Health);
            Movement.SetSpeed(enemyData.MoveSpeed);
            Health.Exhausted += OnHealthExhausted;
            OnInit(enemyData);
            gameObject.SetActive(true);
        }

        public void ClearState()
        {
            CancellationTokenUtils.Destroy(ref _lifetimeTokenSource);
            ObjectLifetimeToken = default;
            Health.Exhausted -= OnHealthExhausted;
            OnClearState();
            gameObject.SetActive(false);
        }

        public void OnCreated(IObjectPool<EnemyBase> objectPool)
        {
            _pool = objectPool;
        }

        public abstract void SeeCharacter(ICharacter character);

        public override void ApplyDamage(DamageInfo damageInfo)
        {
            Health.Remove(damageInfo.Damage);
        }

        protected virtual void OnDying() { }
        protected virtual void OnInit(EnemyData enemyData) { }
        protected virtual void OnClearState() { }

        private void OnHealthExhausted()
        {
            OnDying();
            Died?.Invoke(this);
            _pool.Release(this);
        }
    }
}