using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeScript : MonoBehaviour {

    public MeshFilter Filter;

    public class Triangle
    {
        public Vector3 A;
        public Vector3 B;
        public Vector3 C;

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            A = a; B = b; C = c;
        }

        public float TripleScalar
        {
            get
            {
                return (Vector3.Dot(Vector3.Cross(A, B),C));
            }
        }
    }

    public float CalculateVolume(MeshFilter filter)
    {
        float volume = 0.0f;
        for(int t = 0; t < filter.mesh.triangles.Length; )
        {
            // Get the Mesh vertices
            Vector3 a = filter.mesh.vertices[filter.mesh.triangles[t]];
            t++;
            Vector3 b = filter.mesh.vertices[filter.mesh.triangles[t]];
            t++;
            Vector3 c = filter.mesh.vertices[filter.mesh.triangles[t]];
            t++;

            // Transform the vertices from Local to World coordinates
            a = filter.transform.TransformPoint(a);
            b = filter.transform.TransformPoint(b);
            c = filter.transform.TransformPoint(c);

            // Get the Triple Scalar
            Triangle T = new Triangle(a, b, c);
            volume += T.TripleScalar;
        }
        // Return the Volume
        return Mathf.Abs(volume/6.0f);
    }

    // Use this for initialization
    void Start () {
        Debug.Log(CalculateVolume(Filter));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
