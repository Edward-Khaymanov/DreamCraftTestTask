using Project.Core.Features.Units.Character.Core;
using UnityEngine;

namespace Project.Core.Features.Units.Character.Gameplay
{
    public class CharacterView : MonoBehaviour, ICharacterView
    {
        [SerializeField] private SpriteRenderer _headRenderer;
        [SerializeField] private SpriteRenderer _bodyRenderer;
        [SerializeField] private SpriteRenderer _rightHandRenderer;
        [SerializeField] private SpriteRenderer _leftHandRenderer;
        [SerializeField] private SpriteRenderer _weaponRenderer;

        public void SetWeapon(Sprite weaponSprite)
        {
            _weaponRenderer.sprite = weaponSprite;
        }
    }
}