using UnityEngine;

namespace Core.Grid.Scripts.View
{
    public class LineRendererView : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _lineWidth = 0.06f;
        [SerializeField] private Material _lineMaterial;
    
        public void SetLineRenderer(Vector3 startPos, Vector3 endPos)
        {
            _lineRenderer.startWidth = _lineWidth;
            _lineRenderer.endWidth = _lineWidth;
            _lineRenderer.useWorldSpace = false;
            _lineRenderer.positionCount = 2;
            _lineRenderer.material = _lineMaterial;
        
            _lineRenderer.SetPosition(0, startPos);
            _lineRenderer.SetPosition(1, endPos);
        }
    }
}
