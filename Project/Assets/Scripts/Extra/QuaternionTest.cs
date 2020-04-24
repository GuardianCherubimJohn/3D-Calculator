using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionTest : MonoBehaviour {

    public Vector3 TestVector = new Vector3(0f, 1f, 0f);
    public Quaternion TestQuat = new Quaternion(0f, 0f, 0f, 0f);
    public Vector3 RotationVector = new Vector3(0f, 0f, 0f);
    public MeshFilter filter;
    public MeshFilter filter2;
    public MeshFilter filter3;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Mesh m1 = SphereScript.BuildMesh(Vector3.zero, 0.2f, 32, Color.white);
        Mesh m2 = SphereScript.BuildMesh(TestVector, 0.2f, 32, Color.white);
        Mesh m3 = SphereScript.BuildMesh(Quaternion.Euler(RotationVector) * TestVector, 0.2f, 32, Color.white);
        filter.mesh = m1;
        filter2.mesh = m2;
        filter3.mesh = m3;
    }
}
