using UnityEngine;

namespace Project.Core.Common
{
    public class RigidbodyMover
    {
        private float _speed;
        private Rigidbody2D _rigidbody;

        public RigidbodyMover(Rigidbody2D rigidbody)
        {
            _rigidbody = rigidbody;
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void MakeStep(Vector2 direction)
        {
            _rigidbody.MovePosition(_rigidbody.position + _speed * Time.fixedDeltaTime * direction.normalized);
        }

        public void Rotate(float rotation)
        {
            _rigidbody.rotation = rotation;
        }
    }
}
