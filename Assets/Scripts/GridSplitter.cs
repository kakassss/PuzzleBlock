using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TriangleCell
{
    public int id;
    public Vector3[] vertices;
    public List<TriangleCell> neighbors = new List<TriangleCell>();
    public bool visited;

    public TriangleCell(int id, Vector3[] verts)
    {
        this.id = id;
        this.vertices = verts;
    }
}

public class GridSplitter : MonoBehaviour
{
    [SerializeField] private int gridSize = 4; // 4x4, 5x5, 6x6
    [SerializeField] private Material material;
    [SerializeField] private int _pieceCount = 7;
    
    private List<TriangleCell> allTriangles = new List<TriangleCell>();
    private List<List<TriangleCell>> pieces = new List<List<TriangleCell>>();

    void Start()
    {
        GenerateGrid();
        BuildNeighbors();

        CreatePieces(_pieceCount);

        DrawPieces();
    }

    void GenerateGrid()
    {
        float cellSize = 1f;
        int idCounter = 0;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector3 bottomLeft = new Vector3(x, y, 0);                
                Vector3 bottomRight = new Vector3(x + 1, y, 0);             
                Vector3 topLeft = new Vector3(x, y + 1, 0);             
                Vector3 topRight = new Vector3(x + 1, y + 1, 0);     
                Vector3 center = (bottomLeft + bottomRight + topLeft + topRight) / 4f;

                allTriangles.Add(new TriangleCell(idCounter++, new Vector3[] { bottomLeft, center, bottomRight }));
                allTriangles.Add(new TriangleCell(idCounter++, new Vector3[] { bottomRight, center, topRight }));
                allTriangles.Add(new TriangleCell(idCounter++, new Vector3[] { topRight, center, topLeft }));
                allTriangles.Add(new TriangleCell(idCounter++, new Vector3[] { topLeft, center, bottomLeft }));
            }
        }
    }

    void BuildNeighbors()
    {
        float epsilon = 0.01f;

        foreach (var a in allTriangles)
        {
            foreach (var b in allTriangles)
            {
                if (a == b) continue;

                int shared = 0;
                foreach (var va in a.vertices)
                {
                    foreach (var vb in b.vertices)
                    {
                        if (Vector3.Distance(va, vb) < epsilon)
                            shared++;
                    }
                }

                if (shared >= 2)
                {
                    if (!a.neighbors.Contains(b))
                        a.neighbors.Add(b);
                }
            }
        }
    }

    void CreatePieces(int targetCount)
    {
        foreach (var tri in allTriangles)
            tri.visited = false;

        foreach (var tri in allTriangles)
        {
            if (!tri.visited)
            {
                List<TriangleCell> newPiece = new List<TriangleCell>();
                GrowRegion(tri, newPiece);
                pieces.Add(newPiece);
            }
        }

        while (pieces.Count > targetCount)
        {
            pieces[0].AddRange(pieces[1]);
            pieces.RemoveAt(1);
        }
    }

    void GrowRegion(TriangleCell start, List<TriangleCell> piece)
    {
        Queue<TriangleCell> queue = new Queue<TriangleCell>();
        queue.Enqueue(start);
        start.visited = true;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            piece.Add(current);

            foreach (var n in current.neighbors)
            {
                if (!n.visited && Random.value > 0.6f)
                {
                    n.visited = true;
                    queue.Enqueue(n);
                }
            }
        }
    }

    void DrawPieces()
    {
        int colorIndex = 0;

        foreach (var piece in pieces)
        {
            Mesh mesh = new Mesh();
            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();

            foreach (var tri in piece)
            {
                int startIndex = verts.Count;
                verts.AddRange(tri.vertices);
                tris.Add(startIndex);
                tris.Add(startIndex + 1);
                tris.Add(startIndex + 2);
            }

            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);

            GameObject go = new GameObject("Piece_" + colorIndex);
            go.transform.SetParent(this.transform);

            var mf = go.AddComponent<MeshFilter>();
            var mr = go.AddComponent<MeshRenderer>();
            mf.mesh = mesh;

            Material mat = new Material(material);
            mat.color = Random.ColorHSV();
            mr.material = mat;

            colorIndex++;
        }
    }
}