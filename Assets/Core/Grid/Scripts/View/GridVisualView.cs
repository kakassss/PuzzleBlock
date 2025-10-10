using System.Collections.Generic;
using Core.Grid.Scripts.Controller;
using UnityEngine;
using Zenject;

namespace Core.Grid.Scripts.View
{
    public class GridVisualView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LineRendererView _lineRendererPrefab;
        [SerializeField] private DotSpriteRendererView _dotSpriteRendererPrefab;
    
        [Header("Visual Settings")]
        [SerializeField] private float _lineDepthOffset = -0.8f;
        [SerializeField] private float _dotDepthOffset = -1f;
        private const int GRID_SIZE_OFFSET = 1;
    
        private IGridController _gridController;
        private DiContainer _diContainer;
        private int _gridSize;
    
        [Inject]
        private void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;

            _gridController = _diContainer.Resolve<IGridController>();
            _gridSize = _gridController.GetGridSize() + GRID_SIZE_OFFSET;
        }

        private void Start()
        {
            SetGridView();
        }
    
        private void SetGridView()
        {
            var allCells = _gridController.GetGridVisualPositions(); 
            Vector3 lineRendererOffset = new Vector3(0, 0, _lineDepthOffset);
        
            CreateGridBorders(allCells, lineRendererOffset);
            CreateCenterDots();
        }

        private void CreateGridBorders(List<Vector3> allCells, Vector3 offset)
        {
            int bottomLeftIndex = 0;
            int bottomRightIndex = _gridSize - 1;
            int topLeftIndex = _gridSize * (_gridSize - 1);
            int topRightIndex = _gridSize * _gridSize - 1;
        
            // Top
            CreateBorderLine(allCells[bottomLeftIndex], allCells[topLeftIndex], offset);
            //Bottom
            CreateBorderLine(allCells[bottomRightIndex], allCells[topRightIndex], offset);
            //Left
            CreateBorderLine(allCells[bottomLeftIndex], allCells[bottomRightIndex], offset);
            //Right
            CreateBorderLine(allCells[topLeftIndex], allCells[topRightIndex], offset);
        }
    
        private void CreateBorderLine(Vector3 startCell, Vector3 endCell, Vector3 offset) 
        {
            var lineRenderer = Instantiate(_lineRendererPrefab, transform);
            lineRenderer.SetLineRenderer(startCell, endCell);
            lineRenderer.transform.position += offset;
        }
    
        private void CreateCenterDots()
        {
            Vector3 dotOffset = new Vector3(0, 0, _dotDepthOffset);
            var centerCorners = _gridController.GetCenterPositions();
    
            foreach (var cornerPosition in centerCorners)
            {
                var dot = Instantiate(_dotSpriteRendererPrefab, transform);
                dot.transform.position = cornerPosition + dotOffset;
            }
        }
    }
}