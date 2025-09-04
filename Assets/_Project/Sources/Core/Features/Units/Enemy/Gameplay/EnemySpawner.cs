using Project.Core.Features.Units.Enemy.Core;
using Project.Infrastructure.Pool;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.Features.Units.Enemy.Gameplay
{
    public class EnemySpawner
    {
        private IDictionary<string, EnemyBase> _idsTemplates;
        private IDictionary<string, IObjectPool<EnemyBase>> _idsPools;
        private Transform _container;

        public EnemySpawner(IDictionary<string, EnemyBase> enemyIdTemplates)
        {
            _idsTemplates = new Dictionary<string, EnemyBase>(enemyIdTemplates);
            _idsPools = new Dictionary<string, IObjectPool<EnemyBase>>();
        }

        public void Init()
        {
            _container = new GameObject($"[ ENEMIES POOL ]").transform;
        }

        public void Clear()
        {
            foreach (var pool in _idsPools.Values)
            {
                pool.Dispose();
            }

            GameObject.Destroy(_container.gameObject);

            _container = null;
            _idsPools.Clear();
        }

        public EnemyBase Spawn(string enemyId, EnemyData enemyData, Vector3 position = default, Quaternion rotation = default)
        {
            if (_idsPools.ContainsKey(enemyId) == false)
            {
                CreatePool(enemyId);
            }

            var pool = _idsPools[enemyId];
            var enemy = pool.Get();
            enemy.Init(enemyData);
            enemy.transform.SetPositionAndRotation(position, rotation);
            return enemy;
        }

        private IObjectPool<EnemyBase> CreatePool(string enemyId)
        {
            if (_idsPools.ContainsKey(enemyId))
                return _idsPools[enemyId];

            var pool = new GameObjectPool<EnemyBase>(() => CreateEnemy(_idsTemplates[enemyId]));
            _idsPools.Add(enemyId, pool);
            return pool;
        }

        private EnemyBase CreateEnemy(EnemyBase template)
        {
            var enemy = GameObject.Instantiate(template);
            enemy.transform.SetParent(_container);
            return enemy;
        }

    }
}