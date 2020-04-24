using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceRenderer : MonoBehaviour {

    public Vector3[,] Points;
    public Color VectorColor = Color.white;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void BuildRenderMesh(Transform parent, FunctionLineScript.LinePoints[] points, Material material, Color vectorColor)
    {
        Mesh m = new Mesh();
        int tindex = 0;
        List<CylinderScript.Quad> qlist = new List<CylinderScript.Quad>();
        for (int y=0;y<points.Length - 1;y++)
        {
            for (int x=0;x<points[y].Points.Length - 1;x++)
            {
                Vector3 tl = points[y].Points[x];
                Vector3 tr = points[y].Points[x + 1];
                Vector3 bl = points[y + 1].Points[x];
                Vector3 br = points[y + 1].Points[x + 1];

                CylinderScript.Quad q = CylinderScript.Quad.Build(tl, tr, bl, br, vectorColor, tindex);
                qlist.Add(q);
                tindex += 4;
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

        m.vertices = verts;
        m.normals = norms;
        m.colors = colors;
        m.triangles = tris;
        m.RecalculateNormals();
        m.RecalculateTangents();
        m = FunctionLineScript.NormalSolver.RecalculateNormals(m, 75f);
        FunctionLineScript.CreateChildObject(parent, "Curve_0_Surface", m, material);
        //return m;
    }
}
