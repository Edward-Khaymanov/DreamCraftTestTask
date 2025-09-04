using Cysharp.Threading.Tasks;
using Project.Core.Common;
using Project.Core.Features.Units;
using Project.Utils;
using System.Threading;
using UnityEngine;

namespace Project.Core.Features.Projectile
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        private CancellationTokenSource _moveCancellationTokenSource;
        private CancellationTokenSource _lifetimeTokenSource;
        private CancellationToken _lifetimeToken;
        private IUnit _owner;
        private float _damage;

        private void OnEnable()
        {
            _lifetimeTokenSource = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
            _lifetimeToken = _lifetimeTokenSource.Token;
            _moveCancellationTokenSource = new CancellationTokenSource();
        }

        private void OnDisable()
        {
            CancellationTokenUtils.Destroy(ref _lifetimeTokenSource);
            CancellationTokenUtils.Destroy(ref _moveCancellationTokenSource);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnHit(collision);
        }

        public void StartLifeTimer(float lifetime = 0)
        {
            lifetime = Mathf.Max(lifetime, 0f);
            if (lifetime == 0)
                lifetime = CONSTANTS.DEFAULT_PROJECTILE_LIFETIME;

            WaitLifeTime(lifetime, _lifetimeTokenSource.Token).Forget();
        }

        public void Despawn()
        {
            Destroy(gameObject);
        }

        public void SetSource(IUnit source)
        {
            _owner = source;
        }

        public void SetDamage(float damage)
        {
            _damage = damage;
        }

        public void MoveTo(Vector2 direction, float speed)
        {
            MoveToAsync(direction, speed, _moveCancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid MoveToAsync(Vector2 direction, float speed, CancellationToken cancellationToken)
        {
            direction = direction.normalized;
            while (cancellationToken.IsCancellationRequested == false)
            {
                await UniTask.WaitForFixedUpdate(cancellationToken);
                if (_lifetimeToken.IsCancellationRequested)
                    return;

                _rigidbody.MovePosition(_rigidbody.position + speed * Time.deltaTime * direction);
            }
        }

        private async UniTaskVoid WaitLifeTime(float time, CancellationToken cancellationToken)
        {
            var expirationTime = Time.time + time;
            while (Time.time < expirationTime && cancellationToken.IsCancellationRequested == false)
            {
                await UniTask.NextFrame(cancellationToken);
                if (_lifetimeToken.IsCancellationRequested)
                    return;
            }
            Despawn();
        }

        private void OnHit(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamageable>(out var target) == false)
                return;

            if (ReferenceEquals(target, _owner))
                return;

            target.ApplyDamage(new DamageInfo { Source = _owner, Damage = _damage });
            Despawn();
        }
    }
}
