using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Utils
{
    public static class Extensions
    {
        public static Rect GetWorldRect(this Camera camera)
        {
            if (camera == null)
            {
                return new Rect(0f, 0f, 0f, 0f);
            }

            if (camera.gameObject.activeInHierarchy == false)
            {
                return new Rect(0f, 0f, 0f, 0f);
            }

            var blWorld = camera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
            var trWorld = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, camera.pixelHeight, 0f));

            var minX = Mathf.Min(blWorld.x, trWorld.x);
            var minY = Mathf.Min(blWorld.y, trWorld.y);
            var w = Mathf.Abs(trWorld.x - blWorld.x);
            var h = Mathf.Abs(trWorld.y - blWorld.y);

            return new Rect(minX, minY, w, h);
        }

        public static T GetRandomElement<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return default;

            var size = source.Count();
            if (size == 0)
                return default;

            var randomIndex = Random.Range(0, size);
            return source.ElementAt(randomIndex);
        }

        public static Vector2 GetRandomPoint(this Rect rect)
        {
            var x = Random.Range(rect.xMin, rect.xMax);
            var y = Random.Range(rect.yMin, rect.yMax);
            return new Vector2(x, y);
        }

        public static Vector2 Rotate(this Vector2 vector, float angle)
        {
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            var rotated = rotation * vector;
            return rotated;
        }
    }
}