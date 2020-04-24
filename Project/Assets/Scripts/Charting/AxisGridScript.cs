using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisGridScript : MonoBehaviour {

    public float AxisLength = 100f;
    public Vector3 AxisDirection = new Vector3(0f, 1f, 0f);
    public Vector3 OppositeAxisDirection = new Vector3(1f, 0f, 0f);
    public Vector3 Origin = Vector3.zero;
    public Material MajorAxisMaterial;
    public Material MinorAxisMaterial;
    public int AxisSteps = 1000;

    public float MajorAxisRadius = 1f;
    public float MinorAxisRadius = 0.5f;
    public float MajorAxisArrowRadius = 2f;
    public float MajorAxisArrowLength = 3f;

    FunctionLineScript.LinePoints[] MajorLinePoints;
    FunctionLineScript.LinePoints[] MinorLinePoints;
    FunctionLineScript.LinePoints[] OppositeMinorLinePoints;

    // Use this for initialization
    void Start () {
        BuildMesh();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void BuildMesh()
    {
        MajorLinePoints = new FunctionLineScript.LinePoints[1];
        MajorLinePoints[0] = new FunctionLineScript.LinePoints();

        float halfLength = AxisLength / 2f;

        MajorLinePoints[0].ArrowAtStart = true;
        MajorLinePoints[0].ArrowAtEnd = true;
        MajorLinePoints[0].ArrowLength = MajorAxisArrowLength;
        MajorLinePoints[0].ArrowRadius = MajorAxisArrowRadius;
        MajorLinePoints[0].Points = new Vector3[] { halfLength * AxisDirection, -1f * halfLength * AxisDirection };
        FunctionLineScript.BuildMesh(this.transform, MajorLinePoints, MajorAxisMaterial, MajorAxisRadius, 32, 64, Color.white);

        MinorLinePoints = new FunctionLineScript.LinePoints[AxisSteps + 1];
        OppositeMinorLinePoints = new FunctionLineScript.LinePoints[AxisSteps + 1];

        for (int u=0;u<AxisSteps+1;u++)
        {
            float axisPoint = (float)u / (float)AxisSteps * AxisLength - halfLength;
            float minPoint = 0f / (float)AxisSteps * AxisLength - halfLength;
            float maxPoint = AxisLength - halfLength;

            Vector3 point1 = (axisPoint * AxisDirection) + (minPoint * OppositeAxisDirection);
            Vector3 point2 = (axisPoint * AxisDirection) + (maxPoint * OppositeAxisDirection);
            Vector3 opoint1 = (axisPoint * OppositeAxisDirection) + (minPoint * AxisDirection);
            Vector3 opoint2 = (axisPoint * OppositeAxisDirection) + (maxPoint * AxisDirection);

            MinorLinePoints[u] = new FunctionLineScript.LinePoints();
            OppositeMinorLinePoints[u] = new FunctionLineScript.LinePoints();

            MinorLinePoints[u].Points = new Vector3[] { point1, point2 };
            OppositeMinorLinePoints[u].Points = new Vector3[] { opoint1, opoint2 };
        }
        FunctionLineScript.BuildMesh(this.transform, MinorLinePoints, MinorAxisMaterial, MinorAxisRadius, 16, 32, Color.white);
        FunctionLineScript.BuildMesh(this.transform, OppositeMinorLinePoints, MinorAxisMaterial, MinorAxisRadius, 16, 32, Color.white);
    }
}
