using UnityEngine;

namespace Project.Core.Features.Units.Character.Core
{
    public interface ICharacterFactory
    {
        public ICharacter CreateCharacter();
        public ICharacter CreateCharacter(Vector3 position);
        public ICharacter CreateCharacter(Vector3 position, Quaternion rotation);
    }

}