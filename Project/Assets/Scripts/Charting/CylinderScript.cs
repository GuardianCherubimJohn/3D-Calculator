using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderScript : MonoBehaviour {

    public Vector3 StartingPoint;
    public Vector3 EndingPoint;
    public Vector3 AngleRotation;
    public Vector3 AngleRotation2;
    public float WorldAngle;
    //public LineRenderer lineRenderer;
    public float Radius = 2f;
    public float RadiusEnd = 0f;

    public int CapSegments = 8;
    public MeshFilter filter;
    public MeshRenderer meshRenderer;
    public Color VertexColor;

    Mesh mesh;

    // Use this for initialization
    void Start () {
        filter.mesh = BuildMesh(StartingPoint, EndingPoint, Radius, RadiusEnd, VertexColor, AngleRotation, CapSegments, true);
    }

    // Update is called once per frame
    void Update () {
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

    public static Mesh BuildMesh(Vector3 startingPoint, Vector3 endingPoint, float radiusStart, float radiusEnd, Color vertexColor, Vector3 angleRotation, int capSegments, bool capEnds)
    {
        if (startingPoint.y == 0f)
        {
            startingPoint = new Vector3(startingPoint.x, 0.01f, startingPoint.z);
        }

        /*
        if (endingPoint.y == 0f)
        {
            startingPoint = new Vector3(endingPoint.x, 0.01f, endingPoint.z);
        }
        */
        float aMinus = startingPoint.x;
        Vector3 delta = new Vector3(aMinus, 0f, 0f);
        Vector3 direction = endingPoint - startingPoint;
        Vector3 startShifted = (startingPoint - delta).normalized;
        if (startShifted == Vector3.zero)
        {
            startShifted = new Vector3(0.0f, 1.0f, 0.0f);
        }
        Vector3 ndir = direction.normalized;
        Vector3 rotation = angleRotation;

        List<Vector3[]> vertices = new List<Vector3[]>();
        for (int z=0;z<capSegments;z++)
        {
            double pct = (double)z / (double)capSegments * 360.0f;
            Vector3 angle1 = new Vector3(angleRotation.x + (float)pct, angleRotation.y, angleRotation.z);
            Quaternion a1 = Quaternion.Euler(angle1);
            Quaternion rot1 = (Quaternion.LookRotation(ndir) * a1);
            Vector3 new1 = rot1 * (startShifted * radiusStart) + startingPoint;
            Vector3 new2 = rot1 * (startShifted * radiusEnd) + endingPoint;

            vertices.Add(new Vector3[]{ new1, new2 });
        }
        List<Quad> qlist = new List<Quad>();
        int tindex = 0;
        for (int z=1;z<=capSegments;z++)
        {
            if (z == capSegments)
            {
                qlist.Add(Quad.Build(vertices[0][0], vertices[0][1], vertices[z - 1][0], vertices[z - 1][1], vertexColor, tindex));

                if (capEnds)
                {
                    tindex += 4;

                    qlist.Add(Quad.Build(startingPoint, vertices[0][0], startingPoint, vertices[z - 1][0], vertexColor, tindex));
                    tindex += 4;

                    qlist.Add(Quad.Build(vertices[0][1], endingPoint, vertices[z - 1][1], endingPoint, vertexColor, tindex));
                }
            }
            else
            {
                qlist.Add(Quad.Build(vertices[z][0], vertices[z][1], vertices[z - 1][0], vertices[z - 1][1], vertexColor, tindex));

                if (capEnds)
                {
                    tindex += 4;

                    qlist.Add(Quad.Build(startingPoint, vertices[z][0], startingPoint, vertices[z - 1][0], vertexColor, tindex));
                    tindex += 4;

                    qlist.Add(Quad.Build(vertices[z][1], endingPoint, vertices[z - 1][1], endingPoint, vertexColor, tindex));
                }
            }
            tindex += 4;
        }
        

        Vector3[] verts = new Vector3[qlist.Count * 4];
        Vector3[] norms = new Vector3[qlist.Count * 4];
        Color[] colors = new Color[qlist.Count * 4];
        int[] tris = new int[qlist.Count * 6];
        for(int z=0;z<qlist.Count;z++)
        {
            qlist[z].Vertices.CopyTo(verts, z * 4);
            qlist[z].Normals.CopyTo(norms, z * 4);
            qlist[z].Colors.CopyTo(colors, z * 4);
            qlist[z].TriangleIndices.CopyTo(tris, z * 6);
        }

        /*
        lineRenderer.positionCount = verts.Length;
        lineRenderer.SetPositions(verts);
        */

        Mesh m = new Mesh();

        m.vertices = verts;
        m.normals = norms;
        m.colors = colors;
        m.triangles = tris;
        m.RecalculateNormals();
        m.RecalculateTangents();

        return m;
    }
}
