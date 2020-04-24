using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereScript : MonoBehaviour {

    public Vector3 StartingPoint;
    public float Radius;
    public int Iterations = 16;
    public Color VertexColor = Color.white;

    public MeshFilter filter;
    //public LineRenderer lines;

	// Use this for initialization
	void Start () {
        filter.mesh = BuildMesh(StartingPoint, Radius, Iterations, VertexColor);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static Mesh BuildMesh(Vector3 startingPoint, float radius, int iterations, Color vertexColor)
    {
        //x = r cos(v) cos(u)
        //y = r cos(v) sin(u)     u = [0, 2 * Pi)
        //z = r sin(v)            v = [-Pi / 2, Pi / 2]

        Vector3[,] coordMatrix = new Vector3[iterations + 1, iterations + 1];

        List<Vector3> coords = new List<Vector3>();
        for (int v=0;v<=iterations;v++)
        {
            for (int u=0;u<=iterations;u++)
            {
                float ucoord = (float)u / (float)iterations * (2f * Mathf.PI);
                float vcoord = (float)v / (float)iterations * (Mathf.PI) - (Mathf.PI / 2f);
                float x = radius * Mathf.Cos(vcoord) * Mathf.Cos(ucoord);
                float y = radius * Mathf.Cos(vcoord) * Mathf.Sin(ucoord);
                float z = radius * Mathf.Sin(vcoord);
                Vector3 coord = new Vector3(x, y, z) + startingPoint;
                coords.Add(coord);
                coordMatrix[u, v] = coord;
            }
        }
        //lines.positionCount = coords.Count;
        //lines.SetPositions(coords.ToArray());

        int triangleIndex = 0;
        List<Quad> qlist = new List<Quad>();
        for (int v=0;v<iterations;v++)
        {
            for (int u=0;u<iterations;u++)
            {
                qlist.Add(Quad.Build(coordMatrix[u, v], coordMatrix[u + 1, v], coordMatrix[u, v + 1], coordMatrix[u + 1, v + 1], vertexColor, triangleIndex, true));
                triangleIndex += 4;
            }
        }

        Vector3[] verts = new Vector3[qlist.Count * 4];
        Vector3[] norms = new Vector3[qlist.Count * 4];
        Color[] colors = new Color[qlist.Count * 4];
        int[] tris = new int[qlist.Count * 6];
        for (int z = 0; z < qlist.Count; z++)
        {
            qlist[z].Vertices.CopyTo(verts, z * 4);
            qlist[z].Normals.CopyTo(norms, z * 4);
            qlist[z].Colors.CopyTo(colors, z * 4);
            qlist[z].TriangleIndices.CopyTo(tris, z * 6);
        }

        Mesh m = new Mesh();

        m.vertices = verts;
        m.normals = norms;
        m.colors = colors;
        m.triangles = tris;
        m.RecalculateNormals();
        m.RecalculateTangents();

        return m;

    }

    public class Quad
    {
        public Vector3[] Vertices { get; set; }
        public Vector3[] Normals { get; set; }
        public Color[] Colors { get; set; }
        public int[] TriangleIndices { get; set; }

        public Quad()
        {
            Vertices = new Vector3[4];
            Normals = new Vector3[4];
            Colors = new Color[4];
            TriangleIndices = new int[6];
        }

        public static Quad Build(Vector3 tl, Vector3 tr, Vector3 bl, Vector3 br, Color color, int triangleIndex, bool flipped = false)
        {
            Quad q = new Quad();
            q.Vertices = new Vector3[] { tl, tr, bl, br };
            q.Colors = new Color[] { color, color, color, color };
            if (flipped)
            {
                q.TriangleIndices = new int[] { 2 + triangleIndex, 0 + triangleIndex, 1 + triangleIndex, 2 + triangleIndex, 1 + triangleIndex, 3 + triangleIndex };
                q.Normals = new Vector3[] { Vector3.Cross(bl - tl, tr - tl), Vector3.Cross(tl - tr, br - tr), Vector3.Cross(br - bl, tl - bl), Vector3.Cross(tr - br, bl - br) };
            }
            else
            {
                q.TriangleIndices = new int[] { 2 + triangleIndex, 1 + triangleIndex, 0 + triangleIndex, 2 + triangleIndex, 3 + triangleIndex, 1 + triangleIndex };
                q.Normals = new Vector3[] { Vector3.Cross(bl - tl, tr - tl), Vector3.Cross(tl - tr, br - tr), Vector3.Cross(br - bl, tl - bl), Vector3.Cross(tr - br, bl - br) };
            }
            return q;
        }
    }

}
