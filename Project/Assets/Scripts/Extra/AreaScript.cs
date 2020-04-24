using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AreaScript : MonoBehaviour {

    [Serializable]
    public class Triangle
    {
        [SerializeField]
        public Vector3 a;
        [SerializeField]
        public Vector3 b;
        [SerializeField]
        public Vector3 c;

        public Vector3 CrossAB { get { return Vector3.Cross(a, b); } }
        public Vector3 CrossBC { get { return Vector3.Cross(b, c); } }
        public Vector3 CrossAC { get { return Vector3.Cross(a, c); } }
        public float DotAB { get { return Vector3.Dot(a, b); } }
        public float DotBC { get { return Vector3.Dot(b, c); } }
        public float DotAC { get { return Vector3.Dot(a, c); } }
        public Vector3 NormalA { get { return Vector3.Cross(b - a, c - a); } }
        public Vector3 NormalB { get { return Vector3.Cross(c - b, a - b); } }
        public Vector3 NormalC { get { return Vector3.Cross(a - c, b - c); } }
        public Vector3 ReverseNormalA { get { return -1f * Vector3.Cross(b - a, c - a); } }
        public Vector3 ReverseNormalB { get { return -1f * Vector3.Cross(c - b, a - b); } }
        public Vector3 ReverseNormalC { get { return -1f * Vector3.Cross(a - c, b - c); } }
        public float LengthNormalA { get { return Vector3.Cross(b - a, c - a).magnitude; } }
        public float LengthNormalB { get { return Vector3.Cross(c - b, a - b).magnitude; } }
        public float LengthNormalC { get { return Vector3.Cross(a - c, b - c).magnitude; } }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}", a, b, c, NormalA, NormalB, NormalC, ReverseNormalA, ReverseNormalB, ReverseNormalC, LengthNormalA, LengthNormalB, LengthNormalC);
        }
    }

    [SerializeField]
    public Triangle[] Shape;
    public TextMesh Text;
    public Vector4 CubeRotation;
    public Transform CubeTransform;
    public List<LineRenderer> Lines;
    public Material[] Materials;

    // Use this for initialization
    void Start () {
        BuildLines();
	}

    void BuildLines()
    {
        int z = 0;
        return;
        foreach(Triangle tri in Shape)
        {
            BuildLineRender(new Vector3[] { tri.a, tri.b, tri.c }, "tri_" + z, Materials[0], true);

            BuildLineRender(new Vector3[] { tri.a, tri.CrossAB }, "triC_AB_" + z, Materials[1]);
            BuildLineRender(new Vector3[] { tri.a, tri.CrossBC }, "tric_BC_" + z, Materials[1]);
            BuildLineRender(new Vector3[] { tri.a, tri.CrossAC }, "triC_AC_" + z, Materials[1]);

            BuildLineRender(new Vector3[] { tri.a, tri.NormalA }, "triNA_" + z, Materials[2]);
            BuildLineRender(new Vector3[] { tri.b, tri.NormalB }, "triNB_" + z, Materials[2]);
            BuildLineRender(new Vector3[] { tri.c, tri.NormalC }, "triNC_" + z, Materials[2]);

            BuildLineRender(new Vector3[] { tri.a, tri.ReverseNormalA }, "triRNA_" + z, Materials[3]);
            BuildLineRender(new Vector3[] { tri.b, tri.ReverseNormalB }, "triRNB_" + z, Materials[3]);
            BuildLineRender(new Vector3[] { tri.c, tri.ReverseNormalC }, "triRNC_" + z, Materials[3]);
            z++;
        }
    }

    void UpdateLines()
    {
        int z = 0;
        return;
        foreach(Triangle tri in Shape)
        {
            foreach(LineRenderer l in Lines)
            {
                if (l.gameObject.name == "tri_" + z) l.SetPositions(new Vector3[] { tri.a, tri.b, tri.c });
                if (l.gameObject.name == "triC_AB_" + z) l.SetPositions(new Vector3[] { tri.a, tri.CrossAB });
                if (l.gameObject.name == "triC_BC_" + z) l.SetPositions(new Vector3[] { tri.a, tri.CrossBC });
                if (l.gameObject.name == "triC_AC_" + z) l.SetPositions(new Vector3[] { tri.a, tri.CrossAC });

                if (l.gameObject.name == "triNA_" + z) l.SetPositions(new Vector3[] { tri.a, tri.NormalA });
                if (l.gameObject.name == "triNB_" + z) l.SetPositions(new Vector3[] { tri.b, tri.NormalB });
                if (l.gameObject.name == "triNC_" + z) l.SetPositions(new Vector3[] { tri.c, tri.NormalC });
                if (l.gameObject.name == "triRNA_" + z) l.SetPositions(new Vector3[] { tri.a, tri.ReverseNormalA });
                if (l.gameObject.name == "triRNB_" + z) l.SetPositions(new Vector3[] { tri.b, tri.ReverseNormalB });
                if (l.gameObject.name == "triRNC_" + z) l.SetPositions(new Vector3[] { tri.c, tri.ReverseNormalC });
            }
            z++;
        }
    }

    void BuildLineRender(Vector3[] lines, string name, Material mat, bool loop = false)
    {
        var obj1 = new GameObject(name);
        obj1.transform.parent = gameObject.transform;
        obj1.transform.localPosition = Vector3.zero;
        LineRenderer ren = obj1.AddComponent<LineRenderer>();
        ren.material = mat;
        ren.widthMultiplier = 0.1f;
        ren.positionCount = lines.Length;
        ren.SetPositions(lines);
        ren.useWorldSpace = false;
        ren.loop = loop;
        Lines.Add(ren);
    }

    // Update is called once per frame
    void Update () {
        //StringBuilder sb = new StringBuilder();
        //sb.AppendLine("Cross Distance:"+ Vector3.Cross(Shape[1] - Shape[0], Shape[2] - Shape[0]));
        //sb.AppendLine("Dot Distance:" + Vector3.Dot(Shape[1] - Shape[0], Shape[2] - Shape[0]));
        //sb.AppendLine("Cross 0-1:" + Vector3.Cross(Shape[0], Shape[1]));
        //sb.AppendLine("Cross 0-2:" + Vector3.Cross(Shape[0], Shape[2]));
        //sb.AppendLine("Cross 1-2:" + Vector3.Cross(Shape[1], Shape[2]));
        //sb.AppendLine("Dot 0-1:" + Vector3.Dot(Shape[0], Shape[1]));
        //sb.AppendLine("Dot 0-2:" + Vector3.Dot(Shape[0], Shape[2]));
        //sb.AppendLine("Dot 1-2:" + Vector3.Dot(Shape[1], Shape[2]));
        ////sb.AppendLine("Area Geometry 1/2b*h", ((Shape[1]-Shape[0]))/2.0f);
        //Text.text = sb.ToString();

        CubeTransform.rotation = new Quaternion(CubeRotation.x, CubeRotation.y, CubeRotation.z, CubeRotation.w);
        StringBuilder sb = new StringBuilder();
        foreach(Triangle tri in Shape)
        {
            sb.AppendLine(tri.ToString());
        }
        Text.text = sb.ToString();
        UpdateLines();
    }
}


