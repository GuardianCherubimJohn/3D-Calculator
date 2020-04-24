using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MathExpression.Expression.Vector<double>))]
[CanEditMultipleObjects]
public class VectorEditor : Editor {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnInspectorGUI()
    {
        //SerializedProperty p = target as SerializedProperty;
        //double[] values = ((MathExpression.Expression.Vector<double>)target).DoubleValues;
        //base.OnInspectorGUI();
    }
}
