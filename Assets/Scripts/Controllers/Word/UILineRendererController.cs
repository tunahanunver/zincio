using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.Word
{
    public class UILineRendererController : MaskableGraphic
    {
        [SerializeField] private float lineWidth = 12f;
        [SerializeField] private Color lineColor = Color.white;
        private readonly List<Vector2> _points = new List<Vector2>();
        public override Color color => lineColor;
        
        public void SetPoints(List<Vector2> points)
        {
            _points.Clear();
            _points.AddRange(points);
            SetVerticesDirty();
        }
        
        public void ClearPoints()
        {
            _points.Clear();
            SetVerticesDirty();
        }
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (_points == null || _points.Count < 2) return;
            for (int i = 0; i < _points.Count - 1; i++)
            {
                DrawSegment(vh, _points[i], _points[i + 1], i * 2);
            }
        }
        
        private void DrawSegment(VertexHelper vh, Vector2 start, Vector2 end, int vertexIndex)
        {
            Vector2 direction = (end - start).normalized;
            Vector2 perpendicular = new Vector2(-direction.y, direction.x) * (lineWidth * 0.5f);
            UIVertex vertex = new UIVertex();
            vertex.color = lineColor;
            vertex.uv0 = Vector2.zero;
            vertex.position = start - perpendicular;
            vh.AddVert(vertex);
            vertex.position = start + perpendicular;
            vh.AddVert(vertex);
            vertex.position = end + perpendicular;
            vh.AddVert(vertex);
            vertex.position = end - perpendicular;
            vh.AddVert(vertex);
            vh.AddTriangle(vertexIndex, vertexIndex + 1, vertexIndex + 2);
            vh.AddTriangle(vertexIndex + 2, vertexIndex + 3, vertexIndex);
        }
    }
}