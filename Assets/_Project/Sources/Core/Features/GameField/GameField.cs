using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.Features.GameField
{
    public class GameField : MonoBehaviour, IGameField
    {
        [SerializeField] private Vector2 _center = Vector2.zero;
        [SerializeField] private Vector2 _fieldSize = new Vector2(20f, 10f);
        [SerializeField, Min(0.1f)] private Vector2 _sectorSize = new Vector2(5f, 5f);

        private readonly List<Rect> _sectors = new List<Rect>();

        public IReadOnlyList<Rect> Sectors => _sectors.AsReadOnly();
        public Vector2 FieldCenter => _center;
        public Vector2 FieldSize => _fieldSize;
        public Vector2 SectorSize => _sectorSize;

        private void OnValidate()
        {
            Rebuild();
        }

        public void SetFieldCenter(Vector2 value)
        {
            _center = value;
            Rebuild();
        }

        public void SetFieldSize(Vector2 value)
        {
            _fieldSize = value;
            Rebuild();
        }

        public void SetSectorSize(Vector2 value)
        {
            var x = Mathf.Max(value.x, 0.1f);
            var y = Mathf.Max(value.y, 0.1f);
            _sectorSize = new Vector2(x, y);
            Rebuild();
        }

        public void Rebuild()
        {
            _sectors.Clear();

            var half = _fieldSize * 0.5f;
            var min = _center - half;
            var max = _center + half;

            for (var x = min.x; x < max.x; x += _sectorSize.x)
            {
                var sx = x;
                var sw = Mathf.Min(_sectorSize.x, max.x - sx);

                for (var y = min.y; y < max.y; y += _sectorSize.y)
                {
                    var sy = y;
                    var sh = Mathf.Min(_sectorSize.y, max.y - sy);
                    var r = new Rect(sx, sy, sw, sh);
                    _sectors.Add(r);
                }
            }
        }

        public List<Rect> GetFreeSectors(Rect cameraRect)
        {
            var result = new List<Rect>();

            if (_sectors.Count == 0)
            {
                return result;
            }

            if (cameraRect.width == 0f && cameraRect.height == 0f)
            {
                return new List<Rect>(_sectors);
            }

            foreach (var s in _sectors)
            {
                if (s.Overlaps(cameraRect) == false)
                {
                    result.Add(s);
                }
            }

            return result;
        }
    }
}