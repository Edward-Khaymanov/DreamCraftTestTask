using Project.Core.Features.GameField;
using Project.Utils;
using UnityEngine;

namespace Project.Editor
{
    public class GameFieldVisualizer : MonoBehaviour
    {
        [SerializeField] private GameField _gameField;
        [SerializeField] private Camera _camera;
        [SerializeField] private bool _drawGizmos = true;
        [SerializeField] private Color _gridColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        [SerializeField] private Color _cameraOverlapColor = new Color(1f, 0f, 0f, 0.25f);
        [SerializeField] private Color _allowedColor = new Color(0f, 1f, 0f, 0.12f);

        private void OnDrawGizmos()
        {
            if (_drawGizmos == false || _gameField == null)
            {
                return;
            }

            var sectors = _gameField.Sectors;
            if (sectors == null || sectors.Count == 0)
            {
                return;
            }

            var camRect = _camera != null ? _camera.GetWorldRect() : new Rect(0f, 0f, 0f, 0f);

            DrawFieldBounds();

            foreach (var sector in sectors)
            {
                DrawSector(sector, camRect);
            }
        }

        private void DrawFieldBounds()
        {
            Gizmos.color = _gridColor;
            Gizmos.DrawWireCube(
                new Vector3(_gameField.FieldCenter.x, _gameField.FieldCenter.y, 0f),
                new Vector3(_gameField.FieldSize.x, _gameField.FieldSize.y, 0f));
        }

        private void DrawSector(Rect sector, Rect camRect)
        {
            var center = new Vector3(sector.center.x, sector.center.y, 0f);
            var size = new Vector3(sector.width, sector.height, 0f);

            if (camRect.width > 0f && camRect.height > 0f && sector.Overlaps(camRect))
            {
                Gizmos.color = _cameraOverlapColor;
            }
            else
            {
                Gizmos.color = _allowedColor;
            }

            Gizmos.DrawCube(center, size);

            Gizmos.color = _gridColor;
            Gizmos.DrawWireCube(center, size);
        }
    }

}