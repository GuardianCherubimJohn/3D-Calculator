using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Rotation : MonoBehaviour {

    public int SelectedIndex = 0;
    public Vector3 V;
    public Quaternion Q;
    public Quaternion UnityQ;
    public Transform T;
    public float AlphaAngle = 0f;
    public RPY RPYData;
    public RPY RPYDataNQxInv;
    public RPY RPYDataInvxNQ;
    public RPY RPYDataInv;
    public NewQuaternion NQ;
    //public NewQuaternion NQ1;
    //public NewQuaternion NQ2;
    //public NewQuaternion NQTest;
    //public NewQuaternion[] Quats;
    //public RPY RPYTest;
    public Vector3 VFixed;
    public Vector3 UnityEuler;
    public Vector3 LocalEuler;
    public Vector3 TestInverse;
    public Double3[] AllAngles;
    public double[,] QuatMatrix;

    public NewQuaternion NQInv;
    public NewQuaternion NQxInv;
    public NewQuaternion InvxNQ;

    public Double3[] QuatMatrixValues;

    public Transform EulerTest;

    [Serializable]
    public class Double3
    {
        public double X;
        public double Y;
        public double Z;
    }

	// Use this for initialization
	void Start () {
        QuatMatrixValues = new Double3[3];
	}
	
	// Update is called once per frame
	void Update () {
        //Q = Quaternion.Euler(V);
        //T.localRotation = Q;
        //VFixed = FixEulerAngle(V);
        VFixed = V;
        UnityQ = Quaternion.Euler(VFixed);
        NQ = toQuaternion(VFixed);
        //NQInv = NewQuaternion.Conjugate(NQ);
        //NQxInv = NewQuaternion.MultiplyQuaternion(NQ, NQInv);
        //InvxNQ = NewQuaternion.MultiplyQuaternion(NQInv, NQ);
        //RPYData = toEulerAngle(NQ);
        //RPYData = toEulerAngleNew(NQ);
        List<double[]> allAngles = GetAllEulerAngles(NQ);
        AllAngles = new Double3[allAngles.Count];
        Double3 selected = new Double3();
        Double3 test = new Double3();
        for (int z=0;z<AllAngles.Length;z++)
        {
            double x = allAngles[z][0] / Math.PI * 180.0;
            double y = allAngles[z][1] / Math.PI * 180.0;
            double z1 = allAngles[z][2] / Math.PI * 180.0;

            double tx = allAngles[z][3] / Math.PI * 180.0;
            double ty = allAngles[z][4] / Math.PI * 180.0;
            double tz = allAngles[z][5] / Math.PI * 180.0;
            if (z == SelectedIndex)
            {
                //Debug.Log("Created in UPDATE");
                QuatMatrix = CreateRotationMatrixFromQuaternion(NQ.z, NQ.x, NQ.y, NQ.w);
                QuatMatrixValues[0].X = QuatMatrix[0, 0];
                QuatMatrixValues[0].Y = QuatMatrix[0, 1];
                QuatMatrixValues[0].Z = QuatMatrix[0, 2];
                QuatMatrixValues[1].X = QuatMatrix[1, 0];
                QuatMatrixValues[1].Y = QuatMatrix[1, 1];
                QuatMatrixValues[1].Z = QuatMatrix[1, 2];
                QuatMatrixValues[2].X = QuatMatrix[2, 0];
                QuatMatrixValues[2].Y = QuatMatrix[2, 1];
                QuatMatrixValues[2].Z = QuatMatrix[2, 2];

                selected.X = x;
                selected.Y = y;
                selected.Z = z1;

                test.X = tx;
                test.Y = ty;
                test.Z = tz;

                //Debug.Log(selected.X);


                //Vector3 vecA = new Vector3((float)selected.X, (float)selected.Y, (float)selected.Z);
                //Vector3 vecB = new Vector3((float)test.X, (float)test.Y, (float)test.Z);
                ////vecA = FixEulerAngle(vecA);
                ////vecB = FixEulerAngle(vecB);

                //NQ1 = toQuaternion(vecA);
                //NQ2 = toQuaternion(vecB);
            }
            //Debug.Log(x);
            AllAngles[z] = new Double3() { X = x, Y = y, Z = z1 };
        }

        Vector3 vec = new Vector3((float)selected.X, (float)selected.Y, (float)selected.Z);
        vec = FixEulerAngle(vec);
        Vector3 vec2 = new Vector3((float)test.X, (float)test.Y, (float)test.Z);
        vec2 = FixEulerAngle(vec2);
        TestInverse = vec2;

        //RPYData = toEulerAxisAngle(NQ);
        //RPYDataInv = toEulerAngle(NQInv);
        //RPYDataInvxNQ = toEulerAngle(InvxNQ);
        //RPYDataNQxInv = toEulerAngle(NQxInv);
        //T.localRotation = NQ.toQuaternion();

        //Quats = toQuaternionTest(V);
        //NQTest = Quats[0];
        //RPYTest = toEulerTest(NQTest);
        Q = NQ.toQuaternion();
        T.localRotation = Q;
        LocalEuler = vec;
        //Vector3 vec = new Vector3((float)RPYData.X, (float)RPYData.Y, (float)RPYData.Z);
        //vec = T.localEulerAngles;
        //Debug.Log(T.localEulerAngles);

        EulerTest.localEulerAngles = vec;
        UnityEuler = EulerTest.localEulerAngles;
	}

    public static Vector3 FixEulerAngle(Vector3 euler)
    {
        euler.x = euler.x % 360f;
        euler.y = euler.y % 360f;
        euler.z = euler.z % 360f;
        if (euler.x < 0f) euler.x = 360f - Mathf.Abs(euler.x);
        if (euler.y < 0f) euler.y = 360f - Mathf.Abs(euler.y);
        if (euler.z < 0f) euler.z = 360f - Mathf.Abs(euler.z);
        return euler;
    }

    [Serializable]
    public class RPY
    {
        public double X;
        public double Y;
        public double Z;

        public double XAbs;
        public double YAbs;
        public double ZAbs;

        public double sinr_cosp;
        public double cosr_cosp;
        public double sinp;
        public double sinp_check;
        public double sinp_changed;
        public double siny_cosp;
        public double cosy_cosp;
        public double sinpCheckCosine;
        public double sinpCheckSine;

        public ResolveAtanResult ResolveRoll;
        public ResolveAtanResult ResolveYaw;

        public RPYMatrix InhomogeneousMatrix;
        public RPYMatrix HomogeneousMatrix;

        [Serializable]
        public class RPYMatrix
        {
            public double m1_1;
            public double m1_2;
            public double m1_3;
            public double m2_1;
            public double m2_2;
            public double m2_3;
            public double m3_1;
            public double m3_2;
            public double m3_3;

            public double mtestXDep1;
            public double mtestXDep2;
            public double mtestXDep3;
            public double mtestYDep1;
            public double mtestYDep2;
            public double mtestYDep3;
            public double mtestZDep1;
            public double mtestZDep2;
            public double mtestZDep3;

            public double mtestWZmXY;
            public double mtestWYmXZ;
            public double mtestWXmYZ;

            // need to track:
            // qx^2, qy^2, qz^2, qw^2
            // 2qx^2, 2qy^2, 2qz^2, 2qw^2
            // 1-qx^2, 1-qy^2, 1-qz^2, 1-qw^2
            // 1-2qx^2, 1-2qy^2, 1-2qz^2, 1-2qw^2

            public MetaInformation QXInformation;
            public MetaInformation QYInformation;
            public MetaInformation QZInformation;
            public MetaInformation QWInformation;

            [Serializable]
            public class MetaInformation
            {
                public double varsquared;
                public double twovarsquared;
                public double one_m_varsquared;
                public double one_m_twovarsquared;
            }
        }
    }

    [Serializable]
    public class NewQuaternion
    {
        public double w;
        public double x;
        public double y;
        public double z;

        public MetaInformation MetaStatistics;

        public class MetaInformation
        {
            public double X;
            public double Y;
            public double Z;
            public double cx;
            public double sx;
            public double cy;
            public double sy;
            public double cz;
            public double sz;

            public override string ToString()
            {
                return string.Format("Quaternion - X:[{6},{0},{1}], Y:[{7},{2},{3}], Z:[{8},{4},{5}]", cx, sx, cy, sy, cz, sz, X, Y, Z);
            }
        }

        public static NewQuaternion Clone(NewQuaternion q1)
        {
            NewQuaternion q = new NewQuaternion();
            q.w = q1.w;
            q.x = q1.x;
            q.y = q1.y;
            q.z = q1.z;
            q.ConvertedQuaternion = q1.ConvertedQuaternion;
            q.Stats = q1.Stats;
            return q;
        }

        public static NewQuaternion Normalize(NewQuaternion q1)
        {
            NewQuaternion q = Clone(q1);
            double n = Math.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
            q.x /= n;
            q.y /= n;
            q.z /= n;
            q.w /= n;
            return q;
        }

        public static NewQuaternion Scale(NewQuaternion q1, double s)
        {
            NewQuaternion q = Clone(q1);
            q.x *= s;
            q.y *= s;
            q.z *= s;
            q.w *= s;
            return q;
        }

        public static NewQuaternion Conjugate(NewQuaternion q1)
        {
            NewQuaternion q = Clone(q1);
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            return q;
        }

        public static NewQuaternion Add(NewQuaternion q1, NewQuaternion q2)
        {
            NewQuaternion q = Clone(q1);
            q.x = q1.x + q2.x;
            q.y = q1.y + q2.y;
            q.z = q1.z + q2.z;
            q.w = q1.w + q2.w;
            return q;
        }

        public static NewQuaternion MultiplyQuaternion(NewQuaternion q1, NewQuaternion q2)
        {
            NewQuaternion q = Clone(q1);
            q.x = q1.x * q2.w + q1.y * q2.z - q1.z * q2.y + q1.w * q2.x;
            q.y = -q1.x * q2.z + q1.y * q2.w + q1.z * q2.x + q1.w * q2.y;
            q.z = q1.x * q2.y - q1.y * q2.x + q1.z * q2.w + q1.w * q2.z;
            q.w = -q1.x * q2.x - q1.y * q2.y - q1.z * q2.z + q1.w * q2.w;
            return q;
        }

        [Serializable]
        public class QuaternionStats
        {
            public double QuadrantX;
            public double QuadrantY;
            public double QuadrantZ;

            public bool FlipX;
            public bool FlipY;
            public bool FlipZ;
            public bool FlipW;

            public double halfX;
            public double halfY;
            public double halfZ;
            public double cosX;
            public double sineX;
            public double cosY;
            public double sineY;
            public double cosZ;
            public double sineZ;

            public double Wpart1;
            public double Wpart2;
            public double Xpart1;
            public double Xpart2;
            public double Ypart1;
            public double Ypart2;
            public double Zpart1;
            public double Zpart2;
        }

        public QuaternionStats Stats;
        public RPY ConvertedQuaternion;

        public Quaternion toQuaternion()
        {
            Quaternion q = new Quaternion((float)x, (float)y, (float)z, (float)w);
            return q;
        }

        public override string ToString()
        {
            return string.Format("[w:{0}, x:{1}, y:{2}, z:{3}]", this.w, this.x, this.y, this.z);
        }
    }

    public static NewQuaternion[] toQuaternionTest(Vector3 euler)
    {
        double eX = euler.x * Math.PI / 180.0;
        double eY = euler.y * Math.PI / 180.0;
        double eZ = euler.z * Math.PI / 180.0;
        double c1 = Math.Cos(eX / 2.0);
        double s1 = Math.Sin(eX / 2.0);
        double c2 = Math.Cos(eY / 2.0);
        double s2 = Math.Sin(eY / 2.0);
        double c3 = Math.Cos(eZ / 2.0);
        double s3 = Math.Sin(eZ / 2.0);

        double w1 = c1 * c2 * c3;
        double w2 = s1 * s2 * s3;
        double x1 = s1 * c2 * c3;
        double x2 = c1 * s2 * s3;
        double y1 = c1 * s2 * c3;
        double y2 = s1 * c2 * s3;
        double z1 = c1 * c2 * s3;
        double z2 = s1 * s2 * c3;

        double qw = w1 + w2;
        double qx = x1 - x2;
        double qy = y1 + y2;
        double qz = z1 - z2;
        double quadrantX = (double)euler.x / 360.0 * 4.0;
        double quadrantY = (double)euler.y / 360.0 * 4.0;
        double quadrantZ = (double)euler.z / 360.0 * 4.0;

        NewQuaternion[] q = new NewQuaternion[16];
        for (int z=0;z<16;z++)
        {
            bool flipZ = ((z & 1) == 1) ? true : false;
            bool flipY = ((z >> 1 & 1) == 1) ? true : false;
            bool flipX = ((z >> 2 & 1) == 1) ? true : false;
            bool flipW = ((z >> 3 & 1) == 1) ? true : false;
            q[z] = new NewQuaternion();
            q[z].Stats = new NewQuaternion.QuaternionStats();
            q[z].Stats.FlipW = flipW;
            q[z].Stats.FlipX = flipX;
            q[z].Stats.FlipY = flipY;
            q[z].Stats.FlipZ = flipZ;
            q[z].w = (flipW) ? w1 - w2 : w1 + w2;
            q[z].x = (flipX) ? x1 - x2 : x1 + x2;
            q[z].y = (flipY) ? y1 - y2 : y1 + y2;
            q[z].z = (flipZ) ? z1 - z2 : z1 + z2;
            q[z].Stats.QuadrantX = quadrantX;
            q[z].Stats.QuadrantY = quadrantY;
            q[z].Stats.QuadrantZ = quadrantZ;
            q[z].ConvertedQuaternion = toEulerAngle(q[z]);
        }
        return q;
    }

    public static Vector4 SandwichProduct(Vector4 q1, Vector4 q2)
    {
        Vector4 q;
        q.w = -q1.x * q2.x - q1.y * q2.y - q1.z * q2.z + q1.w * q2.w;
        q.x = q1.x * q2.w + q1.y * q2.z - q1.z * q2.y + q1.w * q2.x;
        q.y = -q1.x * q2.z + q1.y * q2.w + q1.z * q2.x + q1.w * q2.y;
        q.z = q1.x * q2.y - q1.y * q2.x + q1.z * q2.w + q1.w * q2.z;
        return q;
    }

    public static Quaternion CreateVectorSandwich(Vector4 a, Vector4 b, Vector4 c)
    {
        Vector4 v = SandwichProduct(SandwichProduct(a, b), c);
        Quaternion q = new Quaternion(v.x, v.y, v.z, v.w);
        return q;
    }

    public static NewQuaternion toQuaternion(Vector3 euler)
    {
        double x = ((double)euler.x) / 360.0 * 2.0 * Math.PI;
        double y = ((double)euler.y) / 360.0 * 2.0 * Math.PI;
        double z = ((double)euler.z) / 360.0 * 2.0 * Math.PI;

        // Abbreviations for the various angular functions
        double cx = Math.Cos(x * 0.5);
        double sx = Math.Sin(x * 0.5);
        double cy = Math.Cos(y * 0.5);
        double sy = Math.Sin(y * 0.5);
        double cz = Math.Cos(z * 0.5);
        double sz = Math.Sin(z * 0.5);

        Vector4 xSandwich = new Vector4((float)sx, 0f, 0f, (float)cx);
        Vector4 ySandwich = new Vector4(0f, (float)sy, 0f, (float)cy);
        Vector4 zSandwich = new Vector4(0f, 0f, (float)sz, (float)cz);

        NewQuaternion q = new NewQuaternion();
        q.MetaStatistics = new NewQuaternion.MetaInformation() { cx = cx, sx = sx, cy = cy, sy = sy, cz = cz, sz = sz, X = (double)euler.x, Y = (double)euler.y, Z = (double)euler.z };

        //Quaternion q1 = CreateVectorSandwich(xSandwich, ySandwich, zSandwich);
        //Quaternion q2 = CreateVectorSandwich(xSandwich, zSandwich, ySandwich);
        //Quaternion q3 = CreateVectorSandwich(zSandwich, xSandwich, ySandwich);
        //Quaternion q4 = CreateVectorSandwich(zSandwich, ySandwich, xSandwich);
        Quaternion q5 = CreateVectorSandwich(ySandwich, xSandwich, zSandwich);
        //Quaternion q6 = CreateVectorSandwich(ySandwich, zSandwich, xSandwich);

        q.x = q5.x;
        q.y = q5.y;
        q.z = q5.z;
        q.w = q5.w;

        q.Stats = new NewQuaternion.QuaternionStats();
        q.Stats.QuadrantX = (double)euler.x / 360.0 * 4.0;
        q.Stats.QuadrantY = (double)euler.y / 360.0 * 4.0;
        q.Stats.QuadrantZ = (double)euler.z / 360.0 * 4.0;
        return q;
    }

    [Serializable]
    public class ResolveAtanResult
    {
        public double Quadrant;
        public double AtanQuadrant;
    }

    public static ResolveAtanResult ResolveAtanQuadrant(double atanResult, double sine, double cosine)
    {
        atanResult = atanResult % 360.0;
        //if (atanResult < 0.0)
        //    atanResult = Math.Abs(atanResult);

        double quadrant = 0.0;
        if (cosine >= 0.0 && sine >= 0.0)
            quadrant = 1.0;
        else if (cosine < 0.0 && sine >= 0.0)
            quadrant = 2.0;
        else if (cosine < 0.0 && sine < 0.0)
            quadrant = 3.0;
        else if (cosine >= 0.0 && sine < 0.0)
            quadrant = 4.0;

        double atanQuadrant = 0.0;
        if (atanResult >= 0.0 && atanResult <= 90.0)
            atanQuadrant = 1.0;
        else if (atanResult > 90.0 && atanResult <= 180.0)
            atanQuadrant = 2.0;
        else if (atanResult > 180.0 && atanResult <= 270.0)
            atanQuadrant = 3.0;
        else if (atanResult > 270.0 && atanResult <= 360.0)
            atanQuadrant = 4.0;
        else if (atanResult < 0.0 && atanResult >= -90.0)
            atanQuadrant = 4.0;
        else if (atanResult < -90.0 && atanResult >= -180.0)
            atanQuadrant = 3.0;
        else if (atanResult < -180.0 && atanResult >= -270.0)
            atanQuadrant = 2.0;
        else if (atanResult < -270.0 && atanResult >= -360.0)
            atanQuadrant = 1.0;

        return new ResolveAtanResult() { AtanQuadrant = atanQuadrant, Quadrant = quadrant };
    }

    public string MatrixToString(double[,] matrix)
    {
        StringBuilder sb = new StringBuilder();
        int rows = matrix.GetUpperBound(0) + 1;
        int columns = matrix.GetUpperBound(1) + 1;
        sb.AppendLine("Matrix");
        for (int z=0;z<rows;z++)
        {
            sb.Append("[");
            for (int c=0;c<columns;c++)
            {
                sb.Append(matrix[z, c] + ", ");
            }
            sb.AppendLine("]");
        }
        return sb.ToString();
    }

    public double[,] CreateRotationMatrixFromQuaternion(double first, double second, double third, double w)
    {
        // In Unity: q.z = first(x) , q.x = second(y) , q.y = third(z), q.w = w

        // So that means in Unity:
        // CreateRotationMatrixFromQuaternion(q.z, q.x, q.y, q.w)
        // y = q.x:  sy*cx*sz + cy*sx*cz
        // z = q.y: -cy*sx*sz + sy*cx*cz
        // x = q.z: -sy*sx*cz + cy*cx*sz
        // q.w: -sy*sx*sz + cy*cx*cz

        // Making the final matrix when substituting variables:

        double[,] matrix = new double[3, 3];
        double x = first; // Unity: q.z
        double y = second; // Unity: q.x
        double z = third; // Unity: q.y
        double m1_1 = 1.0 - 2.0 * (y * y + z * z);
        double m1_2 = 2.0 * (x * y - z * w);
        double m1_3 = 2.0 * (x * z + y * w);

        double m2_1 = 2.0 * (x * y + z * w);
        double m2_2 = 1.0 - 2.0 * (x * x + z * z);
        double m2_3 = 2.0 * (y * z - x * w);

        double m3_1 = 2.0 * (x * z - y * w);
        double m3_2 = 2.0 * (y * z + x * w);
        double m3_3 = 1.0 - 2.0 * (x * x + y * y);
        matrix[0, 0] = m1_1; matrix[0, 1] = m1_2; matrix[0, 2] = m1_3;
        matrix[1, 0] = m2_1; matrix[1, 1] = m2_2; matrix[1, 2] = m3_3;
        matrix[2, 0] = m3_1; matrix[2, 1] = m3_2; matrix[2, 2] = m3_3;

        //Debug.Log(string.Format("Matrix Creation: {{1st:{0}, 2nd:{1}, 3rd:{2}, W:{3}}}", first, second, third, w));
        return matrix;
    }

    public class QuaternionDeltaComparison
    {
        public double DeltaW;
        public double DeltaX;
        public double DeltaY;
        public double DeltaZ;
        public double SumDelta;

        public double X;
        public double Y;
        public double Z;
        public double[] Angles;
        public NewQuaternion CompareQuaternion;
        public NewQuaternion InputQuaternion;

        public override string ToString()
        {
            return string.Format("Compare: Input:{0}/{1}/{2} {6}Input Quaternion:{3} - Comparison Quaternion:{4} - Sum of Delta:{5}", X, Y, Z, InputQuaternion, CompareQuaternion, SumDelta, Environment.NewLine);
        }
    }

    public QuaternionDeltaComparison CompareDeltaQuaternion(NewQuaternion q, double[] angles)
    {
        // Pitch = 0, Yaw = 1, Roll = 2
        NewQuaternion inQuat = toQuaternion(new Vector3((float)angles[0], (float)angles[1], (float)angles[2]));
        //double cx = Math.Cos(angles[0] / 2.0);
        //double sx = Math.Sin(angles[0] / 2.0);
        //double cy = Math.Cos(angles[1] / 2.0);
        //double sy = Math.Sin(angles[1] / 2.0);
        //double cz = Math.Cos(angles[2] / 2.0);
        //double sz = Math.Sin(angles[2] / 2.0);

        //double w = sy * sx * sz + cy * cx * cz;
        //double x = sy * cx * sz + cy * sx * cz;
        //double y = sy * cx * cz - cy * sx * sz;
        //double z = cy * cx * sz - sy * sx * cz;

        double dW = Math.Abs(inQuat.w - q.w);
        double dX = Math.Abs(inQuat.x - q.x);
        double dY = Math.Abs(inQuat.y - q.y);
        double dZ = Math.Abs(inQuat.z - q.z);
        double sumDelta = dW + dX + dY + dZ;

        QuaternionDeltaComparison d = new QuaternionDeltaComparison() { DeltaW = dW, DeltaX = dX, DeltaY = dY, DeltaZ = dZ, SumDelta = sumDelta, X = angles[0], Y = angles[1], Z = angles[2], Angles = angles, CompareQuaternion = q, InputQuaternion = inQuat };
        Debug.Log(d.ToString());
        return d;
    }

    public double[] CreateEulerFromRotationMatrix(double[,] matrix, int idx1, int idx2, int idx3, NewQuaternion q)
    {
        // In Unity: Euler to Quaternion is built YXZ, rotation matrix is built ZXY, variables are pulled out ZYX,
        // with X being the "pitch" angle pulled by Arcsine.

        double[] angles = new double[6];
        double ftany = matrix[2,1];
        double ftanx = matrix[2,2];
        double first = System.Math.Atan2(ftany, ftanx);
        double stany = matrix[1,0];
        double stanx = matrix[0,0];
        double second = System.Math.Atan2(stany, stanx);
        double tsiny = matrix[2, 0];
        double third = System.Math.Asin(-1.0 * tsiny);
        angles[idx1] = first;
        angles[idx2] = second;
        angles[idx3] = third;

        // Get the second Euler angle describing the same rotation defined as Pi - Asine(-1*mat[2,0])
        double sixth = Math.PI - third;
        double fourth = Math.Atan2(ftany / Math.Cos(sixth), ftanx / Math.Cos(sixth));
        double fifth = Math.Atan2(stany / Math.Cos(sixth), stanx / Math.Cos(sixth));
        angles[idx1 + 3] = fourth;
        angles[idx2 + 3] = fifth;
        angles[idx3 + 3] = sixth;

        // Note: Copied from the above CreateRotationMatrixFromQuaternion I wrote, to relate the gathering of Euler angles to Quaternion Rotation matrices
        //
        // Making the final matrix when substituting variables:
        // Unity: first/fourth = Z, second/fifth = Y, third/sixth = X, yes it's backwards here because of the order of rotations applied by Unity.
        //
        // Note: At this point, I've related Euler angles to rotation matrices in a way that avoids gimbal lock, why even bother storing a W parameter? Except that we want all of the functionality of Quaternions and Euler together.
        //       Also there's the sad fact that Unity doesn't allow you to set the Translation/Scaling/Rotation matrix directly, it's a read-only parameter. So, I'm going to attempt to get the original Euler angles here.
        //
        // Second Note: Leeeeeeroooooyyyyyyy Jennnnnnnnnkinnnnnnns! :) Am I the only person on the internet who thinks of the greater ecosystem of balance in planning versus taking action? I mean come on, give Leroy a break. His team
        //              was just sitting around planning forever. Who knows if they would have succeeded or failed if they had planned a bit more? I don't even really play games anymore, World of Warcraft was not a favorite of mine
        //              though. I happened to like Phantasy Star Universe more, but I played as a Wartecher and was hated by everyone for choosing generalization over specialization, because a Wartecher can use melee attacks, range
        //              weapons, and healing, support, and attack magic, except that it wasn't the greatest in any category, therefore, I beat bosses alone that would have taken a party a shorter time. Consider that though, I still
        //              beat the bosses, repeatedly, except that the game imposed its own grind fest making you re-run missions forever with a party to get the rare item drops you needed. In a strange sense, if you will, I have
        //              Wartecher'd my way through understanding the connection between Euler and Quaternion rotations, which is a NASA problem essentially. The internet was full of assumptions and sometimes misleading or incomplete
        //              information. I had to hunt for little fragments of hints buried in Wikipedia posts, websites, technical whitepapers, and so on, with a needle-in-a-haystack kind of method of extraction. Yes, even Satan himself
        //              tried to attack me and put me in what I call a "Twilight Zone Black Hole Matrix of Lies from Hell" on this journey, he tried to take over my vocal cords, which goes against Christian theology that Christians
        //              can't be possessed by the Devil, well, they can't, except he tried. He actually tried to kill me, and told me that I'm guided by God. Here we are, a worldwide rotational problem solved, by using a gaming
        //              engine of all things, except that Unity, if used in a real world manner, can be a useful tool for mathematical simulation which includes the sciences. I've already partially built a graphing calculator for
        //              Unity, and many other things that I hope to release in time. 
        //                                                                                 -12/4/2018: John Russell Ernest : March 2015 AT&T Hackathon team winner

        // In Unity: q.z = first(x) , q.x = second(y) , q.y = third(z), q.w = w

        // So that means in Unity:

        //q.w = (Sin(x/2)*Sin(y/2)*Sin(z/2) + Cos(x/2)*Cos(y/2)*Cos(z/2))

        //q.x = (Sin(x/2)*Cos(y/2)*Cos(z/2) + Cos(x/2)*Sin(y/2)*Sin(z/2))
        //q.y = (Cos(x/2)*Sin(y/2)*Cos(z/2) - Sin(x/2)*Cos(y/2)*Sin(z/2))
        //q.z = (Cos(x/2)*Cos(y/2)*Sin(z/2) - Sin(x/2)*Sin(y/2)*Cos(z/2))

        // m1_1 = 1.0 - 2.0 * (q.x * q.x + q.y * q.y)
        // m1_2 = 2.0 * (q.z * q.x - q.y * q.w)
        // m1_3 = 2.0 * (q.z * q.y + q.x * q.w)

        // m2_1 = 2.0 * (q.z * q.x + q.y * q.w)
        // m2_2 = 1.0 - 2.0 * (q.z * q.z + q.y * q.y)
        // m2_3 = 2.0 * (q.x * q.y - q.z * q.w)

        // m3_1 = 2.0 * (q.z * q.y - q.x * q.w)
        // m3_2 = 2.0 * (q.x * q.y + q.z * q.w)
        // m3_3 = 1.0 - 2.0 * (q.z * q.z + q.x * q.x)

        // Making the final matrix when substituting variables:

        // m1_1 = 1.0 - 2.0 * ((Sin(x/2)*Cos(y/2)*Cos(z/2) + Cos(x/2)*Sin(y/2)*Sin(z/2)) * (Sin(x/2)*Cos(y/2)*Cos(z/2) + Cos(x/2)*Sin(y/2)*Sin(z/2)) + (Cos(x/2)*Sin(y/2)*Cos(z/2) - Sin(x/2)*Cos(y/2)*Sin(z/2)) * (Cos(x/2)*Sin(y/2)*Cos(z/2) - Sin(x/2)*Cos(y/2)*Sin(z/2)))
        // m1_2 = 2.0 * ((Cos(x/2)*Cos(y/2)*Sin(z/2) - Sin(x/2)*Sin(y/2)*Cos(z/2)) * (Sin(x/2)*Cos(y/2)*Cos(z/2) + Cos(x/2)*Sin(y/2)*Sin(z/2)) - (Cos(x/2)*Sin(y/2)*Cos(z/2) - Sin(x/2)*Cos(y/2)*Sin(z/2)) * (Sin(x/2)*Sin(y/2)*Sin(z/2) + Cos(x/2)*Cos(y/2)*Cos(z/2)))
        // m1_3 = 2.0 * ((Cos(x/2)*Cos(y/2)*Sin(z/2) - Sin(x/2)*Sin(y/2)*Cos(z/2)) * (Cos(x/2)*Sin(y/2)*Cos(z/2) - Sin(x/2)*Cos(y/2)*Sin(z/2)) + (Sin(x/2)*Cos(y/2)*Cos(z/2) + Cos(x/2)*Sin(y/2)*Sin(z/2)) * (Sin(x/2)*Sin(y/2)*Sin(z/2) + Cos(x/2)*Cos(y/2)*Cos(z/2)))

        // m2_1 = 2.0 * ((Cos(x/2)*Cos(y/2)*Sin(z/2) - Sin(x/2)*Sin(y/2)*Cos(z/2)) * (Sin(x/2)*Cos(y/2)*Cos(z/2) + Cos(x/2)*Sin(y/2)*Sin(z/2)) + (Cos(x/2)*Sin(y/2)*Cos(z/2) - Sin(x/2)*Cos(y/2)*Sin(z/2)) * (Sin(x/2)*Sin(y/2)*Sin(z/2) + Cos(x/2)*Cos(y/2)*Cos(z/2)))
        // m2_2 = 1.0 - 2.0 * ((Cos(x/2)*Cos(y/2)*Sin(z/2) - Sin(x/2)*Sin(y/2)*Cos(z/2)) * (Cos(x/2)*Cos(y/2)*Sin(z/2) - Sin(x/2)*Sin(y/2)*Cos(z/2)) + (Cos(x/2)*Sin(y/2)*Cos(z/2) - Sin(x/2)*Cos(y/2)*Sin(z/2)) * (Cos(x/2)*Sin(y/2)*Cos(z/2) - Sin(x/2)*Cos(y/2)*Sin(z/2)))
        // m2_3 = 2.0 * ((Sin(x/2)*Cos(y/2)*Cos(z/2) + Cos(x/2)*Sin(y/2)*Sin(z/2)) * (Cos(x/2)*Sin(y/2)*Cos(z/2) - Sin(x/2)*Cos(y/2)*Sin(z/2)) - (Cos(x/2)*Cos(y/2)*Sin(z/2) - Sin(x/2)*Sin(y/2)*Cos(z/2)) * (Sin(x/2)*Sin(y/2)*Sin(z/2) + Cos(x/2)*Cos(y/2)*Cos(z/2)))

        // m3_1 = 2.0 * ((Cos(x/2)*Cos(y/2)*Sin(z/2) - Sin(x/2)*Sin(y/2)*Cos(z/2)) * (Cos(x/2)*Sin(y/2)*Cos(z/2) - Sin(x/2)*Cos(y/2)*Sin(z/2)) - (Sin(x/2)*Cos(y/2)*Cos(z/2) + Cos(x/2)*Sin(y/2)*Sin(z/2)) * (Sin(x/2)*Sin(y/2)*Sin(z/2) + Cos(x/2)*Cos(y/2)*Cos(z/2)))
        // m3_2 = 2.0 * ((Sin(x/2)*Cos(y/2)*Cos(z/2) + Cos(x/2)*Sin(y/2)*Sin(z/2)) * (Cos(x/2)*Sin(y/2)*Cos(z/2) - Sin(x/2)*Cos(y/2)*Sin(z/2)) + (Cos(x/2)*Cos(y/2)*Sin(z/2) - Sin(x/2)*Sin(y/2)*Cos(z/2)) * (Sin(x/2)*Sin(y/2)*Sin(z/2) + Cos(x/2)*Cos(y/2)*Cos(z/2)))
        // m3_3 = 1.0 - 2.0 * ((Cos(x/2)*Cos(y/2)*Sin(z/2) - Sin(x/2)*Sin(y/2)*Cos(z/2)) * (Cos(x/2)*Cos(y/2)*Sin(z/2) - Sin(x/2)*Sin(y/2)*Cos(z/2)) + (Sin(x/2)*Cos(y/2)*Cos(z/2) + Cos(x/2)*Sin(y/2)*Sin(z/2)) * (Sin(x/2)*Cos(y/2)*Cos(z/2) + Cos(x/2)*Sin(y/2)*Sin(z/2)))

        // Pitch Isolation Method: This method reconstructs the original Cosine and Sine to spec
        //double aXC = Math.Cos(third / 2.0); double aXS = Math.Sin(third / 2.0);
        ////double aYC = Math.Cos(second / 2.0); double aYS = Math.Sin(second / 2.0);
        ////double aZC = Math.Cos(first / 2.0); double aZS = Math.Sin(first / 2.0);
        //double bXC = Math.Cos(third / 2.0); double bXS = Math.Sin(third / 2.0);
        ////double bYC = Math.Cos(fifth / 2.0); double bYS = Math.Sin(fifth / 2.0);
        ////double bZC = Math.Cos(fourth / 2.0); double bZS = Math.Sin(fourth / 2.0);
        //double aYC = Math.Cos(0); double aYS = Math.Sin(0);
        //double aZC = Math.Cos(0); double aZS = Math.Sin(0);
        //double bYC = Math.Cos(0); double bYS = Math.Sin(0);
        //double bZC = Math.Cos(0); double bZS = Math.Sin(0);

        //double[,] matA = new double[3, 3]; double[,] matB = new double[3, 3];

        //matA[0, 0] = 1.0 - 2.0 * ((aXS * aYC * aZC + aXC * aYS * aZS) * (aXS * aYC * aZC + aXC * aYS * aZS) + (aXC * aYS * aZC - aXS * aYC * aZS) * (aXC * aYS * aZC - aXS * aYC * aZS));
        //matA[0, 1] = 2.0 * ((aXC * aYC * aZS - aXS * aYS * aZC) * (aXS * aYC * aZC + aXC * aYS * aZS) - (aXC * aYS * aZC - aXS * aYC * aZS) * (aXS * aYS * aZS + aXC * aYC * aZC));
        //matA[0, 2] = 2.0 * ((aXC * aYC * aZS - aXS * aYS * aZC) * (aXC * aYS * aZC - aXS * aYC * aZS) + (aXS * aYC * aZC + aXC * aYS * aZS) * (aXS * aYS * aZS + aXC * aYC * aZC));

        //matA[1, 0] = 2.0 * ((aXC * aYC * aZS - aXS * aYS * aZC) * (aXS * aYC * aZC + aXC * aYS * aZS) + (aXC * aYS * aZC - aXS * aYC * aZS) * (aXS * aYS * aZS + aXC * aYC * aZC));
        //matA[1, 1] = 1.0 - 2.0 * ((aXC * aYC * aZS - aXS * aYS * aZC) * (aXC * aYC * aZS - aXS * aYS * aZC) + (aXC * aYS * aZC - aXS * aYC * aZS) * (aXC * aYS * aZC - aXS * aYC * aZS));
        //matA[1, 2] = 2.0 * ((aXS * aYC * aZC + aXC * aYS * aZS) * (aXC * aYS * aZC - aXS * aYC * aZS) - (aXC * aYC * aZS - aXS * aYS * aZC) * (aXS * aYS * aZS + aXC * aYC * aZC));

        //matA[2, 0] = 2.0 * ((aXC * aYC * aZS - aXS * aYS * aZC) * (aXC * aYS * aZC - aXS * aYC * aZS) - (aXS * aYC * aZC + aXC * aYS * aZS) * (aXS * aYS * aZS + aXC * aYC * aZC));
        //matA[2, 1] = 2.0 * ((aXS * aYC * aZC + aXC * aYS * aZS) * (aXC * aYS * aZC - aXS * aYC * aZS) + (aXC * aYC * aZS - aXS * aYS * aZC) * (aXS * aYS * aZS + aXC * aYC * aZC));
        //matA[2, 2] = 1.0 - 2.0 * ((aXC * aYC * aZS - aXS * aYS * aZC) * (aXC * aYC * aZS - aXS * aYS * aZC) + (aXS * aYC * aZC + aXC * aYS * aZS) * (aXS * aYC * aZC + aXC * aYS * aZS));

        //matB[0, 0] = 1.0 - 2.0 * ((bXS * bYC * bZC + bXC * bYS * bZS) * (bXS * bYC * bZC + bXC * bYS * bZS) + (bXC * bYS * bZC - bXS * bYC * bZS) * (bXC * bYS * bZC - bXS * bYC * bZS));
        //matB[0, 1] = 2.0 * ((bXC * bYC * bZS - bXS * bYS * bZC) * (bXS * bYC * bZC + bXC * bYS * bZS) - (bXC * bYS * bZC - bXS * bYC * bZS) * (bXS * bYS * bZS + bXC * bYC * bZC));
        //matB[0, 2] = 2.0 * ((bXC * bYC * bZS - bXS * bYS * bZC) * (bXC * bYS * bZC - bXS * bYC * bZS) + (bXS * bYC * bZC + bXC * bYS * bZS) * (bXS * bYS * bZS + bXC * bYC * bZC));

        //matB[1, 0] = 2.0 * ((bXC * bYC * bZS - bXS * bYS * bZC) * (bXS * bYC * bZC + bXC * bYS * bZS) + (bXC * bYS * bZC - bXS * bYC * bZS) * (bXS * bYS * bZS + bXC * bYC * bZC));
        //matB[1, 1] = 1.0 - 2.0 * ((bXC * bYC * bZS - bXS * bYS * bZC) * (bXC * bYC * bZS - bXS * bYS * bZC) + (bXC * bYS * bZC - bXS * bYC * bZS) * (bXC * bYS * bZC - bXS * bYC * bZS));
        //matB[1, 2] = 2.0 * ((bXS * bYC * bZC + bXC * bYS * bZS) * (bXC * bYS * bZC - bXS * bYC * bZS) - (bXC * bYC * bZS - bXS * bYS * bZC) * (bXS * bYS * bZS + bXC * bYC * bZC));

        //matB[2, 0] = 2.0 * ((bXC * bYC * bZS - bXS * bYS * bZC) * (bXC * bYS * bZC - bXS * bYC * bZS) - (bXS * bYC * bZC + bXC * bYS * bZS) * (bXS * bYS * bZS + bXC * bYC * bZC));
        //matB[2, 1] = 2.0 * ((bXS * bYC * bZC + bXC * bYS * bZS) * (bXC * bYS * bZC - bXS * bYC * bZS) + (bXC * bYC * bZS - bXS * bYS * bZC) * (bXS * bYS * bZS + bXC * bYC * bZC));
        //matB[2, 2] = 1.0 - 2.0 * ((bXC * bYC * bZS - bXS * bYS * bZC) * (bXC * bYC * bZS - bXS * bYS * bZC) + (bXS * bYC * bZC + bXC * bYS * bZS) * (bXS * bYC * bZC + bXC * bYS * bZS));

        //Debug.Log(string.Format("{{1st:{0}, 2nd:{1}, 3rd:{2}}}", first, second, third));
        //Debug.Log(string.Format("{{4th:{0}, 5th:{1}, 6th:{2}}}", first, second, third));
        NewQuaternion t1 = toQuaternion(new Vector3((float)third, 0f, 0f));
        //Debug.Log("Creating Second Quaternion Matrix!!!");
        double[,] mat = CreateRotationMatrixFromQuaternion(t1.z, t1.x, t1.y, t1.w);

        //double Aftany = mat[2, 1];
        //double Aftanx = mat[2, 2];
        //double Afirst = System.Math.Atan2(Aftany / Math.Cos(third), Aftanx / Math.Cos(third));
        //double AfirstB = System.Math.Atan2(Aftany, Aftanx);
        //double Bftany = mat[2, 1];
        //double Bftanx = mat[2, 2];
        //double Bfirst = System.Math.Atan2(Bftany / Math.Cos(sixth), Bftanx / Math.Cos(sixth));
        //double BfirstB = System.Math.Atan2(Bftany, Bftanx);
        //double c1 = Math.Acos(mat[2, 2]);
        //double c2 = Math.Acos(mat[2, 2]);

        //Debug.Log(string.Format("Pitch: [{0}, {1}, {2}]", mat[0, 0], mat[0, 1], mat[0, 2]));
        //Debug.Log(string.Format("Pitch: [{0}, {1}, {2}]", mat[1, 0], mat[1, 1], mat[1, 2]));
        //Debug.Log(string.Format("Pitch: [{0}, {1}, {2}]", mat[2, 0], mat[2, 1], mat[2, 2]));
        //Debug.Log(string.Format("Original: [{0}, {1}, {2}]", matrix[0, 0], matrix[0, 1], matrix[0, 2]));
        //Debug.Log(string.Format("Original: [{0}, {1}, {2}]", matrix[1, 0], matrix[1, 1], matrix[1, 2]));
        //Debug.Log(string.Format("Original: [{0}, {1}, {2}]", matrix[2, 0], matrix[2, 1], matrix[2, 2]));

        //Debug.Log(BfirstB);
        //Debug.Log(AfirstB);

        //double[] angles1 = new double[6];
        double ftany1 = matrix[2, 1];
        double ftanx1 = matrix[2, 2];
        double first1 = System.Math.Atan2(ftany1, ftanx1);
        double stany1 = matrix[1, 0];
        double stanx1 = matrix[0, 0];
        double second1 = System.Math.Atan2(stany1, stanx1);
        double tsiny1 = matrix[2, 0];
        double third1 = System.Math.Asin(-1.0 * tsiny1);
        //angles1[idx1] = first1;
        //angles1[idx2] = second1;
        //angles1[idx3] = third1;

        //// Get the second Euler angle describing the same rotation defined as Pi - Asine(-1*mat[2,0])
        double sixth1 = Math.PI - third1;
        double fourth1 = Math.Atan2(ftany1 / Math.Cos(sixth1), ftanx1 / Math.Cos(sixth1));
        double fifth1 = Math.Atan2(stany1 / Math.Cos(sixth1), stanx1 / Math.Cos(sixth1));
        //angles1[idx1 + 3] = fourth1;
        //angles1[idx2 + 3] = fifth1;
        //angles1[idx3 + 3] = sixth1;

        Debug.Log(string.Format("New 1st: [{0}, {1}, {2}]", first1, second1, third1));
        Debug.Log(string.Format("New 2nd: [{0}, {1}, {2}]", fourth1, fifth1, sixth1));

        bool useFirst = true;
        Debug.Log("FIRST!!! " + first);
        double delta1 = Math.Abs(0.0 - first1);
        double delta2 = Math.Abs(0.0 - fourth1);

        double delta3 = Math.Abs(0.0 - second1);
        double delta4 = Math.Abs(0.0 - fifth1);

        double minDeltaFirst = Math.Min(delta1, delta3);
        double minDeltaSecond = Math.Min(delta2, delta4);
        //if (delta2 < delta1)
        if (minDeltaSecond < minDeltaFirst)
        {
            Debug.Log("USE THE SECOND!!!");
            useFirst = false;
        }
        else if (delta4 < delta3)
        {

        }

        if (useFirst)
        {
            Debug.Log("Using the First Set");
            angles[idx1] = first;
            angles[idx2] = second;
            angles[idx3] = third;
        }
        else
        {
            Debug.Log("Using the Second Set");
            angles[idx1] = fourth;
            angles[idx2] = fifth;
            angles[idx3] = sixth;
        }

        //Debug.Log(t1.w);
        //Debug.Log(t1.x);
        //Debug.Log(t1.y);
        //Debug.Log(t1.z);
        //Debug.Log(string.Format("A0:[{0}, {1}, {2}] B0:[{3}, {4}, {5}]", matA[0, 0], matA[0, 1], matA[0, 2], matB[0, 0], matB[0, 1], matB[0, 2]));
        //Debug.Log(string.Format("A1:[{0}, {1}, {2}] B1:[{3}, {4}, {5}]", matA[1, 0], matA[1, 1], matA[1, 2], matB[1, 0], matB[1, 1], matB[1, 2]));
        //Debug.Log(string.Format("A2:[{0}, {1}, {2}] B2:[{3}, {4}, {5}]", matA[2, 0], matA[2, 1], matA[2, 2], matB[2, 0], matB[2, 1], matB[2, 2]));
        //Debug.Log(string.Format("O0:[{0}, {1}, {2}]", matrix[0, 0], matrix[0, 1], matrix[0, 2]));
        //Debug.Log(string.Format("O1:[{0}, {1}, {2}]", matrix[1, 0], matrix[1, 1], matrix[1, 2]));
        //Debug.Log(string.Format("O2:[{0}, {1}, {2}]", matrix[2, 0], matrix[2, 1], matrix[2, 2]));
        //Debug.Log(string.Format("A:{0} B:{0}", Afirst, Bfirst));
        //Debug.Log(string.Format("C1:{0} C2:{0}", c1, c2));
        //Debug.Log(string.Format("{0}, {1}, {2}, {3}, {4}, {5}", Aftanx, Aftany, Bftanx, Bftany, Math.Cos(sixth), Math.Cos(third)));
        //Debug.Log(string.Format("A0:[{0}, {1}, {2}]", matA[0, 0], matA[0, 1], matA[0, 2]));
        //Debug.Log(string.Format("A1:[{0}, {1}, {2}]", matA[1, 0], matA[1, 1], matA[1, 2]));
        //Debug.Log(string.Format("A2:[{0}, {1}, {2}]", matA[2, 0], matA[2, 1], matA[2, 2]));
        //Debug.Log(string.Format("Second Quaternion:{{{0},{1},{2},w:{3}}}",t1.x,t1.y,t1.z,t1.w));

        double sine_pitch = -1.0 * matrix[2, 0];
        double asine_pitch = Math.Asin(sine_pitch);
        double[] pitchPossibilities = new double[2];
        if (asine_pitch < 0)
        {
            double firstTry = Math.Abs(asine_pitch) / Math.PI * 180.0;
            pitchPossibilities[0] = 180.0 + firstTry;
            pitchPossibilities[1] = 360.0 - firstTry;
        }
        else if (asine_pitch > 0)
        {
            double firstTry = Math.Abs(asine_pitch) / Math.PI * 180.0;
            pitchPossibilities[0] = firstTry;
            pitchPossibilities[1] = 180.0 - firstTry;
        }
        else
        {
            pitchPossibilities[0] = 0.0;
            pitchPossibilities[1] = 180.0;
        }

        double sine_roll = matrix[2, 1];
        double cosine_roll = matrix[2, 2];
        //double first = System.Math.Atan2(ftany, ftanx);
        //double sixth1 = Math.PI - third1;
        //double fourth1 = Math.Atan2(ftany1 / Math.Cos(sixth1), ftanx1 / Math.Cos(sixth1));
        //double fifth1 = Math.Atan2(stany1 / Math.Cos(sixth1), stanx1 / Math.Cos(sixth1));

        double sine_yaw = matrix[1, 0];
        double cosine_yaw = matrix[0, 0];

        double[] rollPossibilities = new double[4];
        double[] yawPossibilities = new double[4];

        rollPossibilities[0] = Math.Atan2(sine_roll, cosine_roll) / Math.PI * 180.0;
        yawPossibilities[0] = Math.Atan2(sine_yaw, cosine_yaw) / Math.PI * 180.0;
        rollPossibilities[1] = rollPossibilities[0] + 180.0;
        rollPossibilities[2] = rollPossibilities[0] - 180.0;
        rollPossibilities[3] = 360.0 - rollPossibilities[0];
        yawPossibilities[1] = yawPossibilities[0] + 180.0;
        yawPossibilities[2] = yawPossibilities[0] - 180.0;
        yawPossibilities[3] = 360.0 - yawPossibilities[0];

        //pitchPossibilities[0] = MakePositive(pitchPossibilities[0] % 360.0);
        //pitchPossibilities[1] = MakePositive(pitchPossibilities[1] % 360.0);
        //rollPossibilities[0] = MakePositive(rollPossibilities[0] % 360.0);
        //rollPossibilities[1] = MakePositive(rollPossibilities[1] % 360.0);
        //rollPossibilities[2] = MakePositive(rollPossibilities[2] % 360.0);
        //rollPossibilities[3] = MakePositive(rollPossibilities[3] % 360.0);
        //yawPossibilities[0] = MakePositive(yawPossibilities[0] % 360.0);
        //yawPossibilities[1] = MakePositive(yawPossibilities[1] % 360.0);
        //yawPossibilities[2] = MakePositive(yawPossibilities[2] % 360.0);
        //yawPossibilities[3] = MakePositive(yawPossibilities[3] % 360.0);

        double[][] possibilities = new double[8][];
        possibilities[0] = new double[3] { pitchPossibilities[0], yawPossibilities[0], rollPossibilities[0] };
        possibilities[1] = new double[3] { pitchPossibilities[1], yawPossibilities[0], rollPossibilities[0] };
        possibilities[2] = new double[3] { pitchPossibilities[0], yawPossibilities[1], rollPossibilities[1] };
        possibilities[3] = new double[3] { pitchPossibilities[1], yawPossibilities[1], rollPossibilities[1] };
        possibilities[4] = new double[3] { pitchPossibilities[0], yawPossibilities[2], rollPossibilities[2] };
        possibilities[5] = new double[3] { pitchPossibilities[1], yawPossibilities[2], rollPossibilities[2] };
        possibilities[6] = new double[3] { pitchPossibilities[0], yawPossibilities[3], rollPossibilities[3] };
        possibilities[7] = new double[3] { pitchPossibilities[1], yawPossibilities[3], rollPossibilities[3] };

        QuaternionDeltaComparison[] comparisons = new QuaternionDeltaComparison[8];

        comparisons[0] = CompareDeltaQuaternion(q, possibilities[0]);
        comparisons[1] = CompareDeltaQuaternion(q, possibilities[1]);
        comparisons[2] = CompareDeltaQuaternion(q, possibilities[2]);
        comparisons[3] = CompareDeltaQuaternion(q, possibilities[3]);
        comparisons[4] = CompareDeltaQuaternion(q, possibilities[4]);
        comparisons[5] = CompareDeltaQuaternion(q, possibilities[5]);
        comparisons[6] = CompareDeltaQuaternion(q, possibilities[6]);
        comparisons[7] = CompareDeltaQuaternion(q, possibilities[7]);

        double[] bestAngles = comparisons.OrderBy(x => x.SumDelta).ToList()[0].Angles;
        Debug.Log(string.Format("BEST ANGLES: {0},{1},{2}", bestAngles[0], bestAngles[1], bestAngles[2]));
        Debug.Log("This is the Sine of the Pitch:" + sine_pitch);

        double cosine_pitch_1 = Math.Sqrt(1 - sine_pitch * sine_pitch);
        double cosine_pitch_2 = -1.0 * Math.Sqrt(1 - sine_pitch * sine_pitch);
        Debug.Log("These are the Cosines of the Pitch:" + cosine_pitch_1 + " / " + cosine_pitch_2);
        Debug.Log("qW:" + q.w);
        RotationAngle.GetAngle(cosine_pitch_1, sine_pitch);
        RotationAngle.GetAngle(cosine_pitch_2, sine_pitch);

        ////////double second = System.Math.Atan2(stany, stanx);

        //////double test_new_sine_pitch = Math.Sin(pitchPossibilities[0]);
        //////double test_new_cosine_pitch = Math.Cos(pitchPossibilities[0]);

        ////////double test_new_second_pitch = (Math.PI - Math.Asin(sine_pitch));

        //////double test_new_sine_pitch_2 = Math.Sin(pitchPossibilities[1] / 2);
        //////double test_new_cosine_pitch_2 = Math.Cos(pitchPossibilities[1] / 2);

        //////double new_roll_1 = Math.Atan2(sine_roll / test_new_cosine_pitch, cosine_roll / test_new_cosine_pitch);
        //////double new_yaw_1 = Math.Atan2(sine_yaw / test_new_cosine_pitch, cosine_yaw / test_new_cosine_pitch);
        //////double new_roll_2 = Math.Atan2(sine_roll / test_new_cosine_pitch_2, cosine_roll / test_new_cosine_pitch_2);
        //////double new_yaw_2 = Math.Atan2(sine_yaw / test_new_cosine_pitch_2, cosine_yaw / test_new_cosine_pitch_2);


        //////double test_new_cosine_roll = Math.Cos(Math.Atan2(sine_roll / test_new_cosine_pitch, cosine_roll / test_new_cosine_pitch) / 2);
        //////double test_new_sine_roll = Math.Sin(Math.Atan2(sine_roll / test_new_cosine_pitch, cosine_roll / test_new_cosine_pitch) / 2);
        //////double test_new_cosine_yaw = Math.Cos(Math.Atan2(sine_yaw / test_new_cosine_pitch, cosine_yaw / test_new_cosine_pitch) / 2);
        //////double test_new_sine_yaw = Math.Sin(Math.Atan2(sine_yaw / test_new_cosine_pitch, cosine_yaw / test_new_cosine_pitch) / 2);

        //////double test_new_cosine_roll_2 = Math.Cos(Math.Atan2(sine_roll / test_new_cosine_pitch_2, cosine_roll / test_new_cosine_pitch_2) / 2);
        //////double test_new_sine_roll_2 = Math.Sin(Math.Atan2(sine_roll / test_new_cosine_pitch_2, cosine_roll / test_new_cosine_pitch_2) / 2);
        //////double test_new_cosine_yaw_2 = Math.Cos(Math.Atan2(sine_yaw / test_new_cosine_pitch_2, cosine_yaw / test_new_cosine_pitch_2) / 2);
        //////double test_new_sine_yaw_2 = Math.Sin(Math.Atan2(sine_yaw / test_new_cosine_pitch_2, cosine_yaw / test_new_cosine_pitch_2) / 2);

        ////////double test_new_third_pitch = (0.0 - Math.Asin(sine_pitch));

        ////////double test_new_fourth_pitch = -1.0 * (Math.PI - Math.Asin(sine_pitch));

        //////Debug.Log("Pitch Possibilities: " + pitchPossibilities[0] + " / " + pitchPossibilities[1]);
        //////Debug.Log("Roll Possibilities: " + rollPossibilities[0] + " / " + rollPossibilities[1] + " / " + rollPossibilities[2]);
        //////Debug.Log("Yaw Possibilities: " + yawPossibilities[0] + " / " + yawPossibilities[1] + " / " + yawPossibilities[2]);
        ////////Debug.Log(string.Format("1 - Roll:[{0}], Yaw:[{1}]; 2 - Roll:[{2}], Yaw:[{3}]", new_roll_1, new_yaw_1, new_roll_2, new_yaw_2));
        ////////Debug.Log("ASINE OF SINE PITCH = " + Math.Asin(sine_pitch));
        ////////Debug.Log("First:" + Math.Asin(sine_pitch) / Math.PI * 180.0);
        ////////Debug.Log("Second:" + test_new_second_pitch / Math.PI * 180.0);
        ////////Debug.Log("Third:" + test_new_third_pitch / Math.PI * 180.0);
        ////////Debug.Log("Fourth:" + test_new_fourth_pitch / Math.PI * 180.0);

        ////////double test_new_sine_pitch_3 = Math.Sin(test_new_third_pitch / 2);
        ////////double test_new_cosine_pitch_3 = Math.Cos(test_new_third_pitch / 2);
        ////////double test_new_cosine_roll_3 = Math.Cos(Math.Atan2(sine_roll / test_new_cosine_pitch_3, cosine_roll / test_new_cosine_pitch_3) / 2);
        ////////double test_new_sine_roll_3 = Math.Sin(Math.Atan2(sine_roll / test_new_cosine_pitch_3, cosine_roll / test_new_cosine_pitch_3) / 2);
        ////////double test_new_cosine_yaw_3 = Math.Cos(Math.Atan2(sine_yaw / test_new_cosine_pitch_3, cosine_yaw / test_new_cosine_pitch_3) / 2);
        ////////double test_new_sine_yaw_3 = Math.Sin(Math.Atan2(sine_yaw / test_new_cosine_pitch_3, cosine_yaw / test_new_cosine_pitch_3) / 2);

        ////////double third = System.Math.Asin(-1.0 * tsiny);
        //////double cosine_pitch = (q.w - (sine_roll * sine_yaw * sine_pitch)) / (cosine_roll * cosine_yaw) % 1.0;
        ////////int quadrant_pitch = GetQuadrantForAngle(cosine_pitch, sine_pitch);
        ////////int quadrant_yaw = GetQuadrantForAngle(cosine_yaw, sine_yaw);
        ////////int quadrant_roll = GetQuadrantForAngle(cosine_roll, sine_roll);
        //////// sy*cx*sz+cy*sx*cz
        //////double test1 = (q.x - (cosine_roll * cosine_yaw * sine_pitch)) / (sine_roll * sine_yaw);

        ////////Debug.Log(string.Format("Angle Quadrants: Pitch:{0}, Yaw:{1}, Roll:{2}", quadrant_pitch, quadrant_yaw, quadrant_roll));
        //////Debug.Log(string.Format("Pitch:({0} / {1}), Yaw:({2} / {3}), Roll:({4} / {5})", Math.Round(cosine_pitch, 3), Math.Round(sine_pitch, 3), Math.Round(cosine_yaw, 3), Math.Round(sine_yaw, 3), Math.Round(cosine_roll, 3), Math.Round(sine_roll, 3) ));
        ////////Debug.Log(string.Format("Q:{0}, Known Sines:{1}, Known Cosines:{2}", Math.Round(q.w, 3), (sine_roll * sine_yaw * sine_pitch), (cosine_roll * cosine_yaw) ));
        //////Debug.Log(string.Format("SP:{0}, CY:{1}, SY:{2}, CR:{3}, SR:{4}", sine_pitch, cosine_yaw, sine_yaw, cosine_roll, sine_roll));
        ////////Debug.Log(string.Format("QX:{0}, SR*SP*SY:{1} CR*CY*SP:{2} SR*SY:{3}", q.x, (sine_roll * sine_pitch * sine_yaw), (cosine_roll * cosine_yaw * sine_pitch)/2.0, (sine_roll * sine_yaw)));
        ////////Debug.Log(string.Format("qX/qW:{0}, qW/qX:{1}", q.x / q.w, q.w / q.x));
        ////////Debug.Log(string.Format(""));
        //////Debug.Log(string.Format("qW:{0}, qX:{1}, qY:{2}, qZ:{3}", q.w, q.x, q.y, q.z));
        //Debug.Log(string.Format(""));
        //w: sy* sx*sz + cy * cx * cz
        //x: sy* cx*sz + cy * sx * cz
        //y: sy* cx*cz - cy * sx * sz
        //z: cy* cx*sz - sy * sx * cz

        // In Unity: Euler to Quaternion is built YXZ, rotation matrix is built ZXY, variables are pulled out ZYX,
        // with X being the "pitch" angle pulled by Arcsine.

        //double[] angles = new double[6];
        //double ftany = matrix[2, 1];
        //double ftanx = matrix[2, 2];
        //double first = System.Math.Atan2(ftany, ftanx); // Roll
        //double stany = matrix[1, 0];
        //double stanx = matrix[0, 0];
        //double second = System.Math.Atan2(stany, stanx); // Yaw
        //double tsiny = matrix[2, 0];
        //double third = System.Math.Asin(-1.0 * tsiny); // Pitch
        //angles[idx1] = first; // Z - Roll
        //angles[idx2] = second; // Y - Yaw
        //angles[idx3] = third; // X - Pitch

        ////double test_new_w = test_new_cosine_pitch * test_new_cosine_roll * test_new_cosine_yaw + test_new_sine_pitch * test_new_sine_roll * test_new_sine_yaw;
        ////double test_new_x = test_new_sine_pitch * test_new_cosine_roll * test_new_cosine_yaw + test_new_cosine_pitch * test_new_sine_roll * test_new_sine_yaw;
        ////double test_new_y = test_new_sine_yaw * test_new_cosine_roll * test_new_cosine_pitch - test_new_cosine_yaw * test_new_sine_roll * test_new_sine_pitch;
        ////double test_new_z = test_new_sine_roll * test_new_cosine_pitch * test_new_cosine_yaw - test_new_cosine_roll * test_new_sine_pitch * test_new_sine_yaw;

        ////double test_new_w_2 = test_new_cosine_pitch_2 * test_new_cosine_roll_2 * test_new_cosine_yaw_2 + test_new_sine_pitch_2 * test_new_sine_roll_2 * test_new_sine_yaw_2;
        ////double test_new_x_2 = test_new_sine_pitch_2 * test_new_cosine_roll_2 * test_new_cosine_yaw_2 + test_new_cosine_pitch_2 * test_new_sine_roll_2 * test_new_sine_yaw_2;
        ////double test_new_y_2 = test_new_sine_yaw_2 * test_new_cosine_roll_2 * test_new_cosine_pitch_2 - test_new_cosine_yaw_2 * test_new_sine_roll_2 * test_new_sine_pitch_2;
        ////double test_new_z_2 = test_new_sine_roll_2 * test_new_cosine_pitch_2 * test_new_cosine_yaw_2 - test_new_cosine_roll_2 * test_new_sine_pitch_2 * test_new_sine_yaw_2;

        //////double test_new_w_3 = test_new_cosine_pitch_3 * test_new_cosine_roll_3 * test_new_cosine_yaw_3 + test_new_sine_pitch_3 * test_new_sine_roll_3 * test_new_sine_yaw_3;
        //////double test_new_x_3 = test_new_sine_pitch_3 * test_new_cosine_roll_3 * test_new_cosine_yaw_3 + test_new_cosine_pitch_3 * test_new_sine_roll_3 * test_new_sine_yaw_3;
        //////double test_new_y_3 = test_new_sine_yaw_3 * test_new_cosine_roll_3 * test_new_cosine_pitch_3 - test_new_cosine_yaw_3 * test_new_sine_roll_3 * test_new_sine_pitch_3;
        //////double test_new_z_3 = test_new_sine_roll_3 * test_new_cosine_pitch_3 * test_new_cosine_yaw_3 - test_new_cosine_roll_3 * test_new_sine_pitch_3 * test_new_sine_yaw_3;

        //////double test_new_w_2 = test_new_cosine_pitch_2 * test_new_cosine_roll_2 * test_new_cosine_yaw_2 + test_new_sine_pitch_2 * test_new_sine_roll_2 * test_new_sine_yaw_2;
        //////double test_new_x_2 = test_new_cosine_pitch_2 * test_new_sine_roll_2 * test_new_sine_yaw_2 + test_new_sine_pitch_2 * test_new_cosine_roll_2 * test_new_cosine_yaw_2;
        ////Debug.Log(string.Format("test_qW1:{0}, test_qX1:{1}, test_qY1:{2}, test_qZ1:{3}", test_new_w, test_new_x, test_new_y, test_new_z));
        ////Debug.Log(string.Format("test_qW2:{0}, test_qX2:{1}, test_qY2:{2}, test_qZ2:{3}", test_new_w_2, test_new_x_2, test_new_y_2, test_new_z_2));
        //////Debug.Log(string.Format("test_qW3:{0}, test_qX3:{1}, test_qY3:{2}, test_qZ3:{3}", test_new_w_3, test_new_x_3, test_new_y_3, test_new_z_3));

        ////RotationAngle quadX1 = RotationAngle.GetAngle(test_new_cosine_pitch, test_new_sine_pitch);
        ////RotationAngle quadY1 = RotationAngle.GetAngle(test_new_cosine_yaw, test_new_sine_yaw);
        ////RotationAngle quadZ1 = RotationAngle.GetAngle(test_new_cosine_roll, test_new_sine_roll);
        ////RotationAngle quadX2 = RotationAngle.GetAngle(test_new_cosine_pitch_2, test_new_sine_pitch_2);
        ////RotationAngle quadY2 = RotationAngle.GetAngle(test_new_cosine_yaw_2, test_new_sine_yaw_2);
        ////RotationAngle quadZ2 = RotationAngle.GetAngle(test_new_cosine_roll_2, test_new_sine_roll_2);
        //////RotationAngle quadX3 = RotationAngle.GetAngle(test_new_cosine_pitch_3, test_new_sine_pitch_3);
        //////RotationAngle quadY3 = RotationAngle.GetAngle(test_new_cosine_yaw_3, test_new_sine_yaw_3);
        //////RotationAngle quadZ3 = RotationAngle.GetAngle(test_new_cosine_roll_3, test_new_sine_roll_3);
        ////Debug.Log(string.Format("Euler 1:({0}, {1}, {2})", quadX1, quadY1, quadZ1));
        ////Debug.Log(string.Format("Euler 2:({0}, {1}, {2})", quadX2, quadY2, quadZ2));
        //Debug.Log(string.Format("Euler 3:({0}, {1}, {2})", quadX3, quadY3, quadZ3));
        Debug.Log(q.MetaStatistics);
        return angles;
    }

    public class RotationAngle
    {
        public double Angle { get; set; }
        public double Arctangent { get; set; }
        public double Cosine { get; set; }
        public double Sine { get; set; }
        public int Quadrant { get; set; }

        public static RotationAngle GetAngle(double cosine, double sine)
        {
            RotationAngle angle = new RotationAngle();
            int cosineInt = (int)cosine;
            int sineInt = (int)sine;
            double cosineFraction = cosine - (double)cosineInt;
            double sineFraction = sine - (double)sineInt;
            angle.Quadrant = angle.GetQuadrantForAngle(cosine, sine);
            angle.Arctangent = Math.Atan2(sine, cosine) / Math.PI * 180.0;
            angle.Angle = (Math.Abs(angle.Arctangent) % 90.0) + ((angle.Quadrant - 1) * 90.0);
            angle.Cosine = cosine;
            angle.Sine = sine;
            string test = $"Cosine:{cosine}, Sine:{sine}, CI:{cosineInt}, SI:{sineInt}, CF:{cosineFraction}, SF:{sineFraction}, Quadrant:{angle.Quadrant}, Angle:{angle.Angle}, Arctangent:{angle.Arctangent}";
            Debug.Log(test);
            return angle;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1},{2},{3},{4}]", Angle, Cosine, Sine, Arctangent, Quadrant);
        }

        public int GetQuadrantForAngle(double cosine, double sine)
        {
            decimal cosineDecimal = (decimal)cosine;
            decimal sineDecimal = (decimal)sine;

            if (cosineDecimal >= 0.0M && sineDecimal < 0.0M) return 4;
            if (cosineDecimal < 0.0M && sineDecimal < 0.0M) return 3;
            if (cosineDecimal < 0.0M && sineDecimal >= 0.0M) return 2;
            if (cosineDecimal >= 0.0M && sineDecimal >= 0.0M) return 1;
            return -1;
        }
    }

    public List<double[]> GetAllEulerAngles(NewQuaternion q)
    {
        List<double[,]> matrices = new List<double[,]>();
        List<double[]> angles = new List<double[]>();
        //matrices.Add(CreateRotationMatrixFromQuaternion(q.x, q.y, q.z, q.w));
        //matrices.Add(CreateRotationMatrixFromQuaternion(q.x, q.z, q.y, q.w));
        //matrices.Add(CreateRotationMatrixFromQuaternion(q.y, q.x, q.y, q.w));
        //matrices.Add(CreateRotationMatrixFromQuaternion(q.y, q.y, q.x, q.w));
        matrices.Add(CreateRotationMatrixFromQuaternion(q.z, q.x, q.y, q.w));
        //matrices.Add(CreateRotationMatrixFromQuaternion(q.z, q.y, q.x, q.w));
        for (int i1=0;i1<matrices.Count;i1++)
        {
            //angles.Add(CreateEulerFromRotationMatrix(matrices[i1], 0, 1, 2, q));
            //angles.Add(CreateEulerFromRotationMatrix(matrices[i1], 0, 2, 1, q));
            //angles.Add(CreateEulerFromRotationMatrix(matrices[i1], 1, 0, 2, q));
            //angles.Add(CreateEulerFromRotationMatrix(matrices[i1], 1, 2, 0, q));
            //angles.Add(CreateEulerFromRotationMatrix(matrices[i1], 2, 0, 1, q));
            angles.Add(CreateEulerFromRotationMatrix(matrices[i1], 2, 1, 0, q));
        }
        return angles;
    }

    public static RPY toEulerAxisAngle(NewQuaternion q)
    {
        q.x = -q.x;
        q.z = -q.z;
        q.w = -q.w;
        double angle = 2.0 * Math.Acos(q.w);
        double x = q.x / Math.Sqrt(1 - q.w * q.w);
        double y = q.y / Math.Sqrt(1 - q.w * q.w);
        double z = q.z / Math.Sqrt(1 - q.w * q.w);
        double heading1 = y * Math.Sin(angle) - x * z * (1 - Math.Cos(angle));
        double heading2 = 1 - (y * y + z * z) * (1 - Math.Cos(angle));
        double heading = Math.Atan2(heading1, heading2);
        double attitude1 = x * y * (1 - Math.Cos(angle)) + z * Math.Sin(angle);
        double attitude = Math.Asin(attitude1);
        double bank1 = x * Math.Sin(angle) - y * z * (1 - Math.Cos(angle));
        double bank2 = 1 - (x * x + z * z) * (1 - Math.Cos(angle));
        double bank = Math.Atan2(bank1, bank2);
        RPY t = new RPY();
        double pitchAngle = 2.0 * (q.x * q.y - q.w * q.z);
        t.Y = heading / Math.PI * 180.0;
        t.Z = attitude / Math.PI * 180.0;
        t.X = bank / Math.PI * 180.0;
        t.Y = MakePositive(t.Y);
        t.Z = MakePositive(t.Z);
        t.X = MakePositive(t.X);
        t.ZAbs = 180.0 + t.Z;
        t.sinp = pitchAngle;
        t.XAbs = x;
        t.YAbs = y;
        t.ZAbs = z;
        t.cosr_cosp = heading1;
        t.sinr_cosp = heading2;
        t.sinp_check = attitude1;
        t.cosr_cosp = bank1;
        t.sinr_cosp = bank2;
        t.sinp = angle;

        double t1 = (q.y * q.z - q.x * q.w);
        double t2 = (q.x * q.y - q.z * q.w);
        double t3 = (q.x * q.y + q.z * q.w);
        double t4 = (q.x * q.w - q.y * q.z);
        double t5 = (q.y * q.z + q.x * q.w);
        double wminusall = q.y * q.y - q.z * q.z - q.x * q.x + q.w * q.w;
        t.X = Math.Asin(2.0 * t1) * -1.0;
        t.Y = (Math.Abs(2.0 * t1) > 0.4999989867) ? Math.Atan2((t2 * t4) + (t5 * t2), (t2 * t4) + (t5 * t2)) : Math.Atan2(2.0 * t3 + q.w * q.w, 2.0 * t3);
        t.Z = Math.Atan2(wminusall, 2.0 * t3);

        t.Y = t.Y / Math.PI * 180.0;
        t.Z = t.Z / Math.PI * 180.0;
        t.X = t.X / Math.PI * 180.0;
        t.Y = MakePositive(t.Y);
        t.Z = MakePositive(t.Z);
        t.X = MakePositive(t.X);
        return t;
    }

    public static double MakePositive(double euler)
    {
        double single = -0.005729578;
        double single1 = 360.0 + single;
        if (euler < single)
        {
            euler += 360.0;
        }
        else if (euler > single1)
        {
            euler -= 360.0;
        }
        return euler;
    }

    private static Vector3 Internal_MakePositive(Vector3 euler)
    {
        float single = -0.005729578f;
        float single1 = 360f + single;
        if (euler.x < single)
        {
            euler.x += 360f;
        }
        else if (euler.x > single1)
        {
            euler.x -= 360f;
        }
        if (euler.y < single)
        {
            euler.y += 360f;
        }
        else if (euler.y > single1)
        {
            euler.y -= 360f;
        }
        if (euler.z < single)
        {
            euler.z += 360f;
        }
        else if (euler.z > single1)
        {
            euler.z -= 360f;
        }
        return euler;
    }

    public static RPY toEulerAngleNew(NewQuaternion q)
    {
        double sqw = q.w * q.w;
        double sqx = q.x * q.x;
        double sqy = q.y * q.y;
        double sqz = q.z * q.z;
        double unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
        double test = q.x * q.y + q.z * q.w;
        double heading = 0.0;
        double attitude = 0.0;
        double bank = 0.0;
        RPY rpy = new RPY();
        if (test > 0.4999999 * unit)
        { // singularity at north pole
            Debug.Log("Singularity Greater");
            heading = 2 * Math.Atan2(q.x, q.w);
            attitude = Math.PI / 2.0;
            bank = 0;
            rpy.Y = heading / Math.PI * 180.0;
            rpy.Z = attitude / Math.PI * 180.0;
            rpy.X = bank / Math.PI * 180.0;
            return rpy;
        }
        if (test < -0.4999999 * unit)
        { // singularity at south pole
            Debug.Log("Singularity Less");
            heading = -2 * Math.Atan2(q.x, q.w);
            attitude = -Math.PI / 2.0;
            bank = 0;
            rpy.Y = heading / Math.PI * 180.0;
            rpy.Z = attitude / Math.PI * 180.0;
            rpy.X = bank / Math.PI * 180.0;
            return rpy;
        }
        heading = Math.Atan2(2.0 * q.y * q.w - 2.0 * q.x * q.z, sqx - sqy - sqz + sqw);
        attitude = Math.Asin(2.0 * test / unit);
        bank = Math.Atan2(2.0 * q.x * q.w - 2.0 * q.y * q.z, -sqx + sqy - sqz + sqw);
        rpy.X = heading / Math.PI * 180.0;
        rpy.Z = attitude / Math.PI * 180.0;
        rpy.Y = bank / Math.PI * 180.0;
        rpy.Y = MakePositive(rpy.Y);
        rpy.Z = MakePositive(rpy.Z);
        rpy.X = MakePositive(rpy.X);
        return rpy;
    }

    public static RPY toEulerAngle(NewQuaternion q)
    {
        double roll = 0.0;
        double pitch = 0.0;
        double yaw = 0.0;

        RPY result = new RPY();

        // m1_1: 1-2(q.y^2 + q.z^2)
        // m1_2:   2(q.x*q.y - q.w*q.z)
        // m1_3:   2(q.w*q.y + q.x*q.z)

        // m2_1:   2(q.x*q.y + q.w*q.z)
        // m2_2: 1-2(q.x^2 + q.z^2)
        // m2_3:   2(q.y*q.z - q.w*q.x)

        // m3_1:   2(q.x*q.z - q.w*q.y)
        // m3_2:   2(q.w*q.x + q.y*q.z)
        // m3_3: 1-2(q.x^2 + q.y^2)
        q = NewQuaternion.Normalize(q);

        double m1_1 = 1.0 - 2.0 * (q.y * q.y + q.z * q.z);
        double m1_2 = 2.0 * (q.x * q.y - q.z * q.w);
        double m1_3 = 2.0 * (q.x * q.z + q.y * q.w);

        double m2_1 = 2.0 * (q.x * q.y + q.z * q.w);
        double m2_2 = 1.0 - 2.0 * (q.x * q.x + q.z * q.z);
        double m2_3 = 2.0 * (q.y * q.z - q.x * q.w);

        double m3_1 = 2.0 * (q.x * q.z - q.y * q.w);
        double m3_2 = 2.0 * (q.y * q.z + q.x * q.w);
        double m3_3 = 1.0 - 2.0 * (q.x * q.x + q.y * q.y);

        double angle = Math.Acos(q.w);

        // need to track:
        // qx^2, qy^2, qz^2, qw^2
        // 2qx^2, 2qy^2, 2qz^2, 2qw^2
        // 1-qx^2, 1-qy^2, 1-qz^2, 1-qw^2
        // 1-2qx^2, 1-2qy^2, 1-2qz^2, 1-2qw^2

        double mtestWZmXY = 2.0 * (q.w * q.z - q.x * q.y);
        double mtestWYmXZ = 2.0 * (q.w * q.y - q.x * q.z);
        double mtestWXmYZ = 2.0 * (q.w * q.x - q.y * q.y);

        double mtestXDep1 = m1_1 - m2_2;
        double mtestXDep2 = m1_1 - m3_3;
        double mtestXDep3 = mtestXDep2 - mtestXDep1;

        double mtestYDep1 = m2_2 - m1_1;
        double mtestYDep2 = m2_2 - m3_3;
        double mtestYDep3 = mtestYDep2 - mtestYDep1;

        double mtestZDep1 = m3_3 - m1_1;
        double mtestZDep2 = m3_3 - m2_2;
        double mtestZDep3 = mtestZDep2 - mtestZDep1;
        result.InhomogeneousMatrix = new RPY.RPYMatrix()
        {
            m1_1 = m1_1, m1_2 = m1_2, m1_3 = m1_3, m2_1 = m2_1, m2_2 = m2_2, m2_3 = m2_3, m3_1 = m3_1, m3_2 = m3_2, m3_3 = m3_3,
            mtestXDep1 = mtestXDep1,
            mtestXDep2 = mtestXDep2,
            mtestXDep3 = mtestXDep3,
            mtestYDep1 = mtestYDep1,
            mtestYDep2 = mtestYDep2,
            mtestYDep3 = mtestYDep3,
            mtestZDep1 = mtestZDep1,
            mtestZDep2 = mtestZDep2,
            mtestZDep3 = mtestZDep3,
            mtestWXmYZ = mtestWXmYZ,
            mtestWYmXZ = mtestWYmXZ,
            mtestWZmXY = mtestWZmXY,
            QWInformation = new RPY.RPYMatrix.MetaInformation() { twovarsquared = 2.0*q.w*q.w, varsquared = q.w * q.w, one_m_varsquared = 1.0-(q.w * q.w), one_m_twovarsquared = 1.0-(2.0 * q.w * q.w) },
            QXInformation = new RPY.RPYMatrix.MetaInformation() { twovarsquared = 2.0 * q.x * q.x, varsquared = q.x * q.x, one_m_varsquared = 1.0 - (q.x * q.x), one_m_twovarsquared = 1.0 - (2.0 * q.x * q.x) },
            QYInformation = new RPY.RPYMatrix.MetaInformation() { twovarsquared = 2.0 * q.y * q.y, varsquared = q.y * q.y, one_m_varsquared = 1.0 - (q.y * q.y), one_m_twovarsquared = 1.0 - (2.0 * q.y * q.y) },
            QZInformation = new RPY.RPYMatrix.MetaInformation() { twovarsquared = 2.0 * q.z * q.z, varsquared = q.z * q.z, one_m_varsquared = 1.0 - (q.z * q.z), one_m_twovarsquared = 1.0 - (2.0 * q.z * q.z) }
        };

        //// Abbreviations for the various angular functions
        //double cy = Math.Cos(x * 0.5);
        //double sy = Math.Sin(x * 0.5);
        //double cp = Math.Cos(y * 0.5);
        //double sp = Math.Sin(y * 0.5);
        //double cr = Math.Cos(z * 0.5);
        //double sr = Math.Sin(z * 0.5);

        //NewQuaternion q = new NewQuaternion();

        //double w1 = cy * cp * cr;
        //double w2 = sy * sp * sr;
        //double x1 = sy * cp * cr;
        //double x2 = cy * sp * sr;
        //double y1 = cy * cp * sr;
        //double y2 = sy * sp * cr;
        //double z1 = cy * sp * cr;
        //double z2 = sy * cp * sr;

        //q.w = w1 + w2;
        //q.x = x1 - x2;
        //q.y = y1 + y2;
        //q.z = z1 - z2;

        double sinp_check = 2.0 * (q.w * q.y - q.x * q.z);
        result.sinp_check = sinp_check;
        result.sinpCheckCosine = System.Math.Acos(sinp_check) * 180.0 / Math.PI;
        result.sinpCheckSine = System.Math.Asin(sinp_check) * 180.0 / Math.PI;
        if (sinp_check < 0.0)
        {
            // Original Pitch is between 0 and 180
        }
        else
        {
            // Original Pitch is between 180 and 360
        }

        // roll (x-axis rotation)
        //double sinr_cosp = 2.0 * (q.w * q.x + q.y * q.z);
        double sinr_cosp = m3_2;
        //double cosr_cosp = +1.0 - 2.0 * (q.x * q.x + q.y * q.y);
        double cosr_cosp = m3_3;
        result.sinr_cosp = sinr_cosp;
        result.cosr_cosp = cosr_cosp;
        roll = System.Math.Atan2(sinr_cosp, cosr_cosp);
        result.ResolveRoll = ResolveAtanQuadrant(roll, sinr_cosp, cosr_cosp);
        double xAbs = System.Math.Atan2(Math.Abs(sinr_cosp), Math.Abs(cosr_cosp));

        // pitch (y-axis rotation)
        double sinp = +2.0 * (q.w * q.y - q.z * q.x);
        // This looks to be the flip of m3_1
        result.sinp = sinp;
        if (Math.Abs(sinp) >= 1.0)
        {
            double signSineP = Math.Sign(sinp);
            double pi = Math.PI / 2.0;
            sinp = pi * signSineP;
            result.sinp_changed = sinp;
        }
        pitch = System.Math.Asin(sinp);
        double yAbs = System.Math.Asin(Math.Abs(sinp));

        // yaw (z-axis rotation)
        //double siny_cosp = 2.0 * (q.w * q.z + q.x * q.y);
        double siny_cosp = m2_1;
        //double cosy_cosp = +1.0 - 2.0 * (q.y * q.y + q.z * q.z);
        double cosy_cosp = m1_1;
        result.siny_cosp = siny_cosp;
        result.cosy_cosp = cosy_cosp;
        yaw = System.Math.Atan2(siny_cosp, cosy_cosp);
        result.ResolveYaw = ResolveAtanQuadrant(yaw, siny_cosp, cosr_cosp);
        double zAbs = System.Math.Atan2(Math.Abs(siny_cosp), Math.Abs(cosy_cosp));

        pitch = pitch * 180.0 / Math.PI;
        roll = roll * 180.0 / Math.PI;
        yaw = yaw * 180.0 / Math.PI;

        xAbs = xAbs / Math.PI * 180.0;
        yAbs = yAbs / Math.PI * 180.0;
        zAbs = zAbs / Math.PI * 180.0;

        //pitch = pitch % 180.0;
        //roll = roll % 180.0;
        //yaw = yaw % 180.0;
        //if (pitch < 0f) pitch = 360f - Math.Abs(pitch);
        //if (roll < 0f) roll = 360f - Math.Abs(roll);
        //if (yaw < 0f) yaw = 360f - Math.Abs(yaw);
        //pitch = pitch % 180.0;
        //roll = roll % 180.0;
        //yaw = yaw % 180.0;

        result.X = roll;
        result.Y = pitch;
        result.Z = yaw;
        result.XAbs = xAbs;
        result.YAbs = yAbs;
        result.ZAbs = zAbs;
        return result;
    }
}