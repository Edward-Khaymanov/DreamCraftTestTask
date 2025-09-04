using Project.Core.Features.Units.Character.Core;
using UnityEngine;

namespace Project.Core.Features.Units.Character.Gameplay
{
    public class CharacterFactory : ICharacterFactory
    {
        private Character _characterTemplate;

        public CharacterFactory(Character characterTemplate)
        {
            _characterTemplate = characterTemplate;
        }

        public ICharacter CreateCharacter()
        {
            return CreateCharacter(default, default);
        }

        public ICharacter CreateCharacter(Vector3 position)
        {
            return CreateCharacter(position, default);
        }

        public ICharacter CreateCharacter(Vector3 position, Quaternion rotation)
        {
            return GameObject.Instantiate(_characterTemplate, position, rotation);
        }
    }
}