using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class GridSplitter : MonoBehaviour
{
    [SerializeField] private int gridSize = 4;
    [SerializeField] private Material material;
    [SerializeField] private int _pieceCount = 7;
    [SerializeField] private PieceView _piecePrefab;
    
    private List<TriangleCell> _allTriangles = new List<TriangleCell>();
    private List<Piece> _pieces = new List<Piece>();
    private List<Vector3> _snapPoints = new List<Vector3>();
    private List<PieceView> _spawnedPieces = new List<PieceView>();

    private IInstantiator _instantiator;
    private IPieceSpawnPositionService _pieceSpawnPositionService;
    
    [Inject]
    private void Construct(IInstantiator instantiator, IPieceSpawnPositionService pieceSpawnPositionService)
    {
        _instantiator = instantiator;    
        _pieceSpawnPositionService = pieceSpawnPositionService;
    }
    
    private void Start()
    {
        GenerateNewLevel();
    }
    
    public int GetGridSize() => gridSize;
    public List<Vector3> GetSnapPoints() => _snapPoints;
    public List<Piece> GetPieces() => _pieces;
    public List<PieceView> GetSpawnedPieces() => _spawnedPieces;
    
    public void GenerateNewLevel()
    {
        GenerateGrid();
        CollectSnapPoints();
        FindNeighborTriangles();
        CreatePieces(_pieceCount);
        DrawPieces();
    }
    
    public void LoadFromLevelData(LevelData levelData)
    {
        gridSize = levelData.gridSize;
        _snapPoints = new List<Vector3>(levelData.snapPoints);
        
        _allTriangles.Clear();
        _pieces.Clear();
        
        foreach (var pieceData in levelData.pieces)
        {
            Piece piece = new Piece(pieceData.pieceId);
            
            foreach (var triData in pieceData.triangles)
            {
                TriangleCell triangle = new TriangleCell(triData.vertices, false, triData.cell);
                piece.AddTriangle(triangle);
                _allTriangles.Add(triangle);
            }
            
            _pieces.Add(piece);
        }
        FindNeighborTriangles();
        DrawLoadedPieces(levelData);
    }
    
    public void ClearLevel()
    {
        foreach (var piece in _spawnedPieces)
        {
            if (piece != null)
            {
                Destroy(piece.gameObject);
            }
        }
        
        _spawnedPieces.Clear();
        _allTriangles.Clear();
        _pieces.Clear();
        _snapPoints.Clear();
    }
    
    private void DrawLoadedPieces(LevelData levelData)
    {
        for (int i = 0; i < _pieces.Count; i++)
        {
            Piece piece = _pieces[i];
            Mesh mesh = piece.CreateMesh();
            
            var pieceGo = _instantiator.InstantiatePrefabForComponent<PieceView>(_piecePrefab, transform);
            pieceGo.name = "Piece_" + piece.ID;
            mesh.name = "Piece_" + piece.ID + "_Mesh";
            
            pieceGo.SetPiece(piece.Triangles,_snapPoints);
            pieceGo.SetMesh(mesh);

            PieceData pieceData = levelData.pieces.Find(p => p.pieceId == piece.ID);
            if (pieceData != null)
            {
                pieceGo.transform.position = pieceData.startPosition;
            }
            else
            {
                pieceGo.transform.position = _pieceSpawnPositionService.GetSpawnPosition();
            }
            
            _spawnedPieces.Add(pieceGo);
        }
    }
    
    private void GenerateGrid()
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector3 topLeft = new Vector3(x, y + 1, 0);             
                Vector3 topRight = new Vector3(x + 1, y + 1, 0);     
                Vector3 bottomLeft = new Vector3(x, y, 0);                
                Vector3 bottomRight = new Vector3(x + 1, y, 0);             
                Vector3 center = (bottomLeft + bottomRight + topLeft + topRight) / 4f;
                
                var topTriangle = new TriangleCell(new Vector3[] { topRight, center, topLeft }, false, new Vector2(x, y));
                var bottomTriangle = new TriangleCell( new Vector3[] { bottomLeft, center, bottomRight }, false, new Vector2(x, y));
                var leftTriangle = new TriangleCell(new Vector3[] { topLeft, center, bottomLeft }, false, new Vector2(x, y));
                var rightTriangle = new TriangleCell(new Vector3[] { bottomRight, center, topRight },false,new Vector2(x,y));
                
                _allTriangles.Add(topTriangle);
                _allTriangles.Add(bottomTriangle);
                _allTriangles.Add(leftTriangle);
                _allTriangles.Add(rightTriangle);
            }
        }
    }
    
    private void CollectSnapPoints()
    {
        HashSet<Vector3> uniquePoints = new HashSet<Vector3>();
        
        foreach (var triangle in _allTriangles)
        {
            Vector3 headVertex = triangle.Vertices[0];
            uniquePoints.Add(headVertex);
        }
        
        _snapPoints.AddRange(uniquePoints);
    }

    private void FindNeighborTriangles()
    {
        float epsilon = 0.01f;

        foreach (var triangleX in _allTriangles)
        {
            triangleX.Neighbors.Clear();
            
            foreach (var triangleY in _allTriangles)
            {
                if (triangleX == triangleY) continue;   
                
                int sharedVertices = 0;
                
                foreach (var va in triangleX.Vertices)
                {
                    foreach (var vb in triangleY.Vertices)
                    {
                        if (Vector3.Distance(va, vb) < epsilon)
                            sharedVertices++;
                    }
                }

                if (sharedVertices == 2)
                {
                    if (!triangleX.Neighbors.Contains(triangleY))
                        triangleX.Neighbors.Add(triangleY);
                }
            }
        }
    }

    private void CreatePieces(int targetCount)
    {
        int pieceIdCounter = 0;
        
        foreach (var tri in _allTriangles)
        {
            tri.Visited = false;
        }
        
        foreach (var tri in _allTriangles)
        {
            if (!tri.Visited)
            {
                Piece newPiece = new Piece(pieceIdCounter++);
                GrowRegion(tri, newPiece);
                _pieces.Add(newPiece);
            }
        }

        MergePiecesToTargetCount(targetCount);
    }

    private void MergePiecesToTargetCount(int targetCount)
    {
        while (_pieces.Count > targetCount)
        {
            int smallestIndex = 0;
            for (int i = 1; i < _pieces.Count; i++)
            {
                if (_pieces[i].TriangleCount < _pieces[smallestIndex].TriangleCount)
                    smallestIndex = i;
            }
            
            int neighborPieceIndex = FindNeighborPiece(smallestIndex);
            if (neighborPieceIndex == -1)
            {
                neighborPieceIndex = (smallestIndex + 1) % _pieces.Count;
            }
            
            var smallestPiece = _pieces[smallestIndex];
            _pieces[neighborPieceIndex].AddTriangles(smallestPiece.Triangles);
            _pieces.RemoveAt(smallestIndex);
        }
    }

    private int FindNeighborPiece(int smallestPieceIndex)
    {
        Piece smallestPiece = _pieces[smallestPieceIndex];
        
        for (int i = 0; i < _pieces.Count; i++)
        {
            if (i == smallestPieceIndex) continue;
            
            if (smallestPiece.IsNeighborWith(_pieces[i]))
                return i;
        }
        
        return -1; 
    }

    void GrowRegion(TriangleCell start, Piece piece)
    {
        Queue<TriangleCell> queue = new Queue<TriangleCell>();
        queue.Enqueue(start);
        start.Visited = true;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            piece.AddTriangle(current);

            foreach (var n in current.Neighbors)
            {
                if (!n.Visited && Random.value > 0.6f)
                {
                    n.Visited = true;
                    queue.Enqueue(n);
                }
            }
        }
    }

    void DrawPieces()
    {
        for (int i = 0; i < _pieces.Count; i++)
        {
            Piece piece = _pieces[i];
            Mesh mesh = piece.CreateMesh();
            
            var pieceGo = _instantiator.InstantiatePrefabForComponent<PieceView>(_piecePrefab, transform);
            pieceGo.name = "Piece_" + piece.ID;
            mesh.name = "Piece_" + piece.ID + "_Mesh";
            
            pieceGo.SetPiece(piece.Triangles,_snapPoints);
            pieceGo.SetMesh(mesh);
            
            float randomX = Random.Range(0, gridSize);
            float randomY = Random.Range(-4, -5);
            
            pieceGo.transform.position = new Vector3(randomX, randomY, 0);
            _spawnedPieces.Add(pieceGo);
        }
    }
    
    void OnDrawGizmos()
    {
        if (_snapPoints == null || _snapPoints.Count == 0) return;
        
        Gizmos.color = Color.red;
        foreach (var point in _snapPoints)
        {
            Gizmos.DrawWireSphere(point, 0.15f);
        }
    }
}