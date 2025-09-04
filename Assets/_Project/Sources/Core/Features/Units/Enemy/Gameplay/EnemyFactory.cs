using Project.Core.Features.Units.Enemy.Core;
using UnityEngine;

namespace Project.Core.Features.Units.Enemy.Gameplay
{
    public class EnemyFactory
    {
        private EnemyBase _template;
        private Transform _parent;

        public EnemyFactory(EnemyBase template, Transform parent = null)
        {
            _template = template;
            _parent = parent;
        }

        public EnemyBase Create()
        {
            var enemy = GameObject.Instantiate(_template, _parent);
            enemy.gameObject.SetActive(false);
            return enemy;
        }
    }
}