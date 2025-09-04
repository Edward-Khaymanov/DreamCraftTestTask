using Project.Core.Common;
using UnityEngine;

namespace Project.Core.Features.Units
{
    public interface IUnit : IDamageable
    {
        public Transform Transform { get; }
        public Vector2 Position { get; }
        public float Rotation { get; }

        public void Destroy();
    }

}