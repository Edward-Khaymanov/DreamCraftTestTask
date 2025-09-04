using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.Features.GameField
{
    public interface IGameField
    {
        public Vector2 FieldCenter { get; }
        public Vector2 FieldSize { get; }
        public Vector2 SectorSize { get; }
        public IReadOnlyList<Rect> Sectors { get; }

        public void SetFieldCenter(Vector2 value);
        public void SetFieldSize(Vector2 value);
        public void SetSectorSize(Vector2 value);
        public List<Rect> GetFreeSectors(Rect cameraRect);
        public void Rebuild();
    }
}