using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionBehavior : MonoBehaviour {

    public Quaternion q;
    public Quaternion q2;
    public Quaternion q3;
    public Transform t;
    public Vector3 v1;
    public Vector3 v2;
    public Vector3 v3;
    public RotationalComponents c1;
    public RotationalComponents c2;
    public Matrix4x4 transformMatrixWorldToLocal;
    public Matrix4x4 transformMatrixLocalToWorld;

    public float Theta;
    public float CosineHalfTheta;
    public float SineHalfTheta;
    public float X;
    public float Y;
    public float Z;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        v2 = UnityEditor.TransformUtils.GetInspectorRotation(t);
        v3 = t.localEulerAngles;
        q = t.localRotation;
        RotationalComponents c1 = RotationalComponents.FromEuler(v2);
        RotationalComponents c2 = RotationalComponents.FromEuler(v3);
        q2 = c1.UnityQuaternion;
        q3 = c2.UnityQuaternion;
        transformMatrixWorldToLocal = t.worldToLocalMatrix;
        transformMatrixLocalToWorld = t.localToWorldMatrix;
        //t.localRotation = q3;
        t.localEulerAngles = t.localRotation.eulerAngles;
	}

    public Vector3 UnwrapAngle(Vector3 angle)
    {

        return angle;
    }

    public void QuaternionFunctions()
    {
        Theta = (float)Math.Acos(q.w) * 2.0f;
        CosineHalfTheta = (float)Math.Cos(Theta / 2.0f);
        SineHalfTheta = (float)Math.Sin(Theta / 2.0f);
        X = q.x / SineHalfTheta;
        Y = q.y / SineHalfTheta;
        Z = q.z / SineHalfTheta;
    }

    [Serializable]
    public class RotationalComponents
    {
        public MathExpression.Expression.Vector<double> EulerAngle { get; set; }
        public MathExpression.Expression.Vector<double> Quaternion { get; set; }

        public Quaternion UnityQuaternion
        {
            get
            {
                return new Quaternion((float)Quaternion.Values[1], (float)Quaternion.Values[2], (float)Quaternion.Values[3], (float)Quaternion.Values[0]);
            }
        }

        public RotationalComponents(double x, double y, double z)
        {
            EulerAngle = new MathExpression.Expression.Vector<double>(3);
            EulerAngle.Values[0] = x;
            EulerAngle.Values[1] = y;
            EulerAngle.Values[2] = z;

            Quaternion = FromEuler(EulerAngle.Values[0], EulerAngle.Values[1], EulerAngle.Values[2]);
        }

        public static RotationalComponents FromEuler(Vector3 euler)
        {
            RotationalComponents c = new RotationalComponents((double)euler.x, (double)euler.y, (double)euler.z);
            return c;
        }

        MathExpression.Expression.Vector<double> FromEuler(double x, double y, double z)
        {
            // Abbreviations for the various angular functions
            double cy = Math.Cos(z * 0.5);
            double sy = Math.Sin(z * 0.5);
            double cr = Math.Cos(x * 0.5);
            double sr = Math.Sin(x * 0.5);
            double cp = Math.Cos(y * 0.5);
            double sp = Math.Sin(y * 0.5);

            MathExpression.Expression.Vector<double> q = new MathExpression.Expression.Vector<double>(4);
            // w
            q.Values[0] = (float)(cy * cr * cp + sy * sr * sp);
            // x
            q.Values[1] = (float)(cy * sr * cp - sy * cr * sp);
            // y
            q.Values[2] = (float)(cy * cr * sp + sy * sr * cp);
            // z
            q.Values[3] = (float)(sy * cr * cp - cy * sr * sp);
            return q;
        }
    }

    public Vector3 toEuler(Quaternion q1)
    {
        float heading = 0f;
        float attitude = 0f;
        float bank = 0f;
        double test = q1.x * q1.y + q1.z * q1.w;
        Vector3 result = new Vector3(0f, 0f, 0f);
        if (test > 0.499)
        { // singularity at north pole
            heading = 2f * (float)Math.Atan2(q1.x, q1.w);
            attitude = (float)Math.PI / 2f;
            bank = 0;
            result.z = heading;
            result.y = attitude;
            result.x = bank;
            return result;
        }
        if (test < -0.499)
        { // singularity at south pole
            heading = -2 * (float)Math.Atan2(q1.x, q1.w);
            attitude = -1.0f * (float)Math.PI / 2f;
            bank = 0;
            result.z = heading;
            result.y = attitude;
            result.x = bank;
            return result;
        }
        double sqx = q1.x * q1.x;
        double sqy = q1.y * q1.y;
        double sqz = q1.z * q1.z;
        heading = (float)Math.Atan2(2f * q1.y * q1.w - 2f * q1.x * q1.z, 1f - 2f * sqy - 2f * sqz);
        attitude = (float)Math.Asin(2f * test);
        bank = (float)Math.Atan2(2f * q1.x * q1.w - 2f * q1.y * q1.z, 1f - 2f * sqx - 2f * sqz);
        result.z = heading;
        result.y = attitude;
        result.x = bank;
        return result;
    }
}
