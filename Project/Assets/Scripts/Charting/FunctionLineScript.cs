using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FunctionLineScript : MonoBehaviour {

    [Serializable]
    public class LinePoints
    {
        public Vector3[] Points;
        public bool ArrowAtStart;
        public bool ArrowAtEnd;
        public float ArrowLength = 1f;
        public float ArrowRadius = 1f;
    }

    public LinePoints[] CurvePoints;

    public float LineRadius = 1f;
    public int SphereIterations = 8;
    public int CylinderCapSegments = 8;
    public Color VertexColor = Color.white;

    public MeshFilter filter;
    public Material MainMaterial;

	// Use this for initialization
	void Start () {
        BuildMesh(this.transform, CurvePoints, MainMaterial, LineRadius, SphereIterations, CylinderCapSegments, VertexColor);
        
    }

    // Update is called once per frame
    void Update () {
    }

    public static void DeleteChildObjects(Transform parent, string keyMask)
    {
        for (int z=parent.childCount - 1; z >= 0; z--)
        {
            Transform t = parent.GetChild(z);
            if (t.name.ToLower().StartsWith(keyMask))
            {
                DestroyImmediate(t.gameObject);
            }
        }
    }

    public static void CreateChildObject(Transform parent, string name, Mesh mesh, Material material)
    {
        GameObject g = new GameObject();
        MeshRenderer r = g.AddComponent<MeshRenderer>();
        MeshFilter f = g.AddComponent<MeshFilter>();
        r.material = material;
        f.mesh = mesh;
        g.transform.parent = parent;
        g.transform.name = name;
    }

    public static Mesh TransformMesh(Transform parent, Mesh input)
    {
        Vector3 scale = parent.lossyScale;
        for (int z=0;z<input.vertices.Length;z++)
        {
            input.vertices[z] = new Vector3(scale.x * input.vertices[z].x, scale.y * input.vertices[z].y, scale.z * input.vertices[z].z);
        }
        return input;
    }

    public static void BuildMesh(Transform parent, LinePoints[] curves, Material material, float lineRadius, int sphereIterations, int cylinderCapSegments, Color vertexColor)
    {
        DeleteChildObjects(parent, "Curve");
        for (int z = 0; z < curves.Length; z++)
        {
            for (int v = 0; v < curves[z].Points.Length; v++)
            {
                Mesh m = SphereScript.BuildMesh(curves[z].Points[v], lineRadius, sphereIterations, vertexColor);
                m = NormalSolver.RecalculateNormals(m, 75f);
                CreateChildObject(parent, "Curve_" + z + "_Sphere_" + v, m, material);

                if (v < curves[z].Points.Length - 1)
                {
                    Mesh m2 = CylinderScript.BuildMesh(curves[z].Points[v], curves[z].Points[v + 1], lineRadius, lineRadius, vertexColor, new Vector3(0f, 90f, 0f), cylinderCapSegments, true);
                    m2 = NormalSolver.RecalculateNormals(m2, 75f);
                    CreateChildObject(parent, "Curve_" + z + "_Cylinder_" + v, m2, material);
                }
            }

            if (curves[z].ArrowAtStart)
            {
                Vector3 p0 = curves[z].Points[0];
                Vector3 p1 = curves[z].Points[1];
                Vector3 parrow = (p0 - p1).normalized * curves[z].ArrowLength + p0;
                Mesh m = CylinderScript.BuildMesh(p0, parrow, curves[z].ArrowRadius, 0f, vertexColor, new Vector3(0f, 90f, 0f), cylinderCapSegments, true);
                m = NormalSolver.RecalculateNormals(m, 75f);
                CreateChildObject(parent, "Curve_" + z + "_ArrowStart", m, material);
            }

            if (curves[z].ArrowAtEnd)
            {
                Vector3 p0 = curves[z].Points[curves[z].Points.Length - 1];
                Vector3 p1 = curves[z].Points[curves[z].Points.Length - 2];
                Vector3 parrow = (p0 - p1).normalized * curves[z].ArrowLength + p0;
                Mesh m = CylinderScript.BuildMesh(p0, parrow, curves[z].ArrowRadius, 0f, vertexColor, new Vector3(0f, 90f, 0f), cylinderCapSegments, true);
                m = NormalSolver.RecalculateNormals(m, 75f);
                CreateChildObject(parent, "Curve_" + z + "_ArrowEnd", m, material);
            }
        }
    }

    public static class NormalSolver
    {
        /// <summary>
        ///     Recalculate the normals of a mesh based on an angle threshold. This takes
        ///     into account distinct vertices that have the same position.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="angle">
        ///     The smoothing angle. Note that triangles that already share
        ///     the same vertex will be smooth regardless of the angle! 
        /// </param>
        public static Mesh RecalculateNormals(Mesh mesh, float angle)
        {
            var cosineThreshold = Mathf.Cos(angle * Mathf.Deg2Rad);

            var vertices = mesh.vertices;
            var normals = new Vector3[vertices.Length];

            // Holds the normal of each triangle in each sub mesh.
            var triNormals = new Vector3[mesh.subMeshCount][];

            var dictionary = new Dictionary<VertexKey, List<VertexEntry>>(vertices.Length);

            for (var subMeshIndex = 0; subMeshIndex < mesh.subMeshCount; ++subMeshIndex)
            {

                var triangles = mesh.GetTriangles(subMeshIndex);

                triNormals[subMeshIndex] = new Vector3[triangles.Length / 3];

                for (var i = 0; i < triangles.Length; i += 3)
                {
                    int i1 = triangles[i];
                    int i2 = triangles[i + 1];
                    int i3 = triangles[i + 2];

                    // Calculate the normal of the triangle
                    Vector3 p1 = vertices[i2] - vertices[i1];
                    Vector3 p2 = vertices[i3] - vertices[i1];
                    Vector3 normal = Vector3.Cross(p1, p2).normalized;
                    int triIndex = i / 3;
                    triNormals[subMeshIndex][triIndex] = normal;

                    List<VertexEntry> entry;
                    VertexKey key;

                    if (!dictionary.TryGetValue(key = new VertexKey(vertices[i1]), out entry))
                    {
                        entry = new List<VertexEntry>(4);
                        dictionary.Add(key, entry);
                    }
                    entry.Add(new VertexEntry(subMeshIndex, triIndex, i1));

                    if (!dictionary.TryGetValue(key = new VertexKey(vertices[i2]), out entry))
                    {
                        entry = new List<VertexEntry>();
                        dictionary.Add(key, entry);
                    }
                    entry.Add(new VertexEntry(subMeshIndex, triIndex, i2));

                    if (!dictionary.TryGetValue(key = new VertexKey(vertices[i3]), out entry))
                    {
                        entry = new List<VertexEntry>();
                        dictionary.Add(key, entry);
                    }
                    entry.Add(new VertexEntry(subMeshIndex, triIndex, i3));
                }
            }

            // Each entry in the dictionary represents a unique vertex position.

            foreach (var vertList in dictionary.Values)
            {
                for (var i = 0; i < vertList.Count; ++i)
                {

                    var sum = new Vector3();
                    var lhsEntry = vertList[i];

                    for (var j = 0; j < vertList.Count; ++j)
                    {
                        var rhsEntry = vertList[j];

                        if (lhsEntry.VertexIndex == rhsEntry.VertexIndex)
                        {
                            sum += triNormals[rhsEntry.MeshIndex][rhsEntry.TriangleIndex];
                        }
                        else
                        {
                            // The dot product is the cosine of the angle between the two triangles.
                            // A larger cosine means a smaller angle.
                            var dot = Vector3.Dot(
                                triNormals[lhsEntry.MeshIndex][lhsEntry.TriangleIndex],
                                triNormals[rhsEntry.MeshIndex][rhsEntry.TriangleIndex]);
                            if (dot >= cosineThreshold)
                            {
                                sum += triNormals[rhsEntry.MeshIndex][rhsEntry.TriangleIndex];
                            }
                        }
                    }

                    normals[lhsEntry.VertexIndex] = sum.normalized;
                }
            }

            mesh.normals = normals;
            return mesh;
        }

        private struct VertexKey
        {
            private readonly long _x;
            private readonly long _y;
            private readonly long _z;

            // Change this if you require a different precision.
            private const int Tolerance = 100000;

            // Magic FNV values. Do not change these.
            private const long FNV32Init = 0x811c9dc5;
            private const long FNV32Prime = 0x01000193;

            public VertexKey(Vector3 position)
            {
                _x = (long)(Mathf.Round(position.x * Tolerance));
                _y = (long)(Mathf.Round(position.y * Tolerance));
                _z = (long)(Mathf.Round(position.z * Tolerance));
            }

            public override bool Equals(object obj)
            {
                var key = (VertexKey)obj;
                return _x == key._x && _y == key._y && _z == key._z;
            }

            public override int GetHashCode()
            {
                long rv = FNV32Init;
                rv ^= _x;
                rv *= FNV32Prime;
                rv ^= _y;
                rv *= FNV32Prime;
                rv ^= _z;
                rv *= FNV32Prime;

                return rv.GetHashCode();
            }
        }

        private struct VertexEntry
        {
            public int MeshIndex;
            public int TriangleIndex;
            public int VertexIndex;

            public VertexEntry(int meshIndex, int triIndex, int vertIndex)
            {
                MeshIndex = meshIndex;
                TriangleIndex = triIndex;
                VertexIndex = vertIndex;
            }
        }
    }
}
