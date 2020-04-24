using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatisticsScript))]
[CanEditMultipleObjects]
public class StatisticsEditor : Editor {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        StatisticsScript script = target as StatisticsScript;
        if (GUILayout.Button("Update Chart"))
        {
            script.UpdateChart();
        }
    }
}
