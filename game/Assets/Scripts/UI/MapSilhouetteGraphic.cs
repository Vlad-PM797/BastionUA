using UnityEngine;
using UnityEngine.UI;

namespace BastionUA.UI
{
    public sealed class MapSilhouetteGraphic : Graphic
    {
        [SerializeField]
        private Vector2[] _normalizedPoints;

        public void SetNormalizedPoints(Vector2[] normalizedPoints)
        {
            _normalizedPoints = normalizedPoints;
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            vertexHelper.Clear();

            if (_normalizedPoints == null || _normalizedPoints.Length < 3)
            {
                return;
            }

            var rect = rectTransform.rect;
            var origin = new Vector2(rect.xMin, rect.yMin);
            var size = rect.size;

            var center = Vector2.zero;
            for (var index = 0; index < _normalizedPoints.Length; index++)
            {
                var point = origin + Vector2.Scale(_normalizedPoints[index], size);
                center += point;
                vertexHelper.AddVert(point, color, Vector2.zero);
            }

            center /= _normalizedPoints.Length;
            var centerVertIndex = vertexHelper.currentVertCount;
            vertexHelper.AddVert(center, color, Vector2.zero);

            for (var index = 0; index < _normalizedPoints.Length; index++)
            {
                var nextIndex = (index + 1) % _normalizedPoints.Length;
                vertexHelper.AddTriangle(index, nextIndex, centerVertIndex);
            }
        }
    }
}
