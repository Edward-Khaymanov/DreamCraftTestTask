using Project.Core.Features.Units.Character.Core;
using UnityEngine;

namespace Project.Core.Features.Units.Character.Gameplay
{
    public class CharacterCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _smoothTime = 0.1f;

        private ICharacter _character;
        private Vector3 _velocity;

        public Camera Camera => _camera;

        private void LateUpdate()
        {
            if (_character == null)
                return;

            var targetPosition = new Vector3(_character.Position.x, _character.Position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
        }

        public void SetCharacter(ICharacter character)
        {
            _character = character;
        }
    }
}