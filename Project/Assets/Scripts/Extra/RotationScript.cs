using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class RotationScript : MonoBehaviour {

    public double X;
    public double Y;
    public double Z;
    public MetaInformation Stats;

    [Serializable]
    public class MetaInformation
    {
        [ReadOnly]
        public double QuadrantX;
        [ReadOnly]
        public double QuadrantY;
        [ReadOnly]
        public double QuadrantZ;

        [ReadOnly]
        public double CosineX;
        [ReadOnly]
        public double CosineY;
        [ReadOnly]
        public double CosineZ;
        [ReadOnly]
        public double SineX;
        [ReadOnly]
        public double SineY;
        [ReadOnly]
        public double SineZ;

        [ReadOnly]
        public double CX_CY_CZ;
        [ReadOnly]
        public double SX_SY_SZ;
        [ReadOnly]
        public double CX_SY_SZ;
        [ReadOnly]
        public double SX_CY_CZ;
        [ReadOnly]
        public double SX_CY_SZ;
        [ReadOnly]
        public double CX_SY_CZ;
        [ReadOnly]
        public double SX_SY_CZ;
        [ReadOnly]
        public double CX_CY_SZ;

        [ReadOnly]
        public double cW_p_sW;
        [ReadOnly]
        public double cW_m_sW;
        [ReadOnly]
        public double sW_m_sW;
        [ReadOnly]
        public double cX_p_sX;
        [ReadOnly]
        public double cX_m_sX;
        [ReadOnly]
        public double sX_m_cX;
        [ReadOnly]
        public double cY_p_sY;
        [ReadOnly]
        public double cY_m_sY;
        [ReadOnly]
        public double sY_m_cY;
        [ReadOnly]
        public double cZ_p_sZ;
        [ReadOnly]
        public double cZ_m_sZ;
        [ReadOnly]
        public double sZ_m_cZ;

        [ReadOnly]
        public double TestX;
        [ReadOnly]
        public double TestY;
        [ReadOnly]
        public double TestZ;

        [ReadOnly]
        public double TestW;

        [ReadOnly]
        public double TestXCS;
        [ReadOnly]
        public double TestYCS;
        [ReadOnly]
        public double TestZCS;

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        SetTransform();
	}

    public static double DetermineAngle(double cosine, double sine)
    {
        // sine + = 0-180
        // sine - = 180-360
        // cosine + = 0-90 or 270-360
        // cosine - = 90-270

        // cosine + sine + = 0-90
        // cosine - sine + = 90-180
        // cosine + sine - = 180-270
        // cosine - sine - = 270-360

        int quadrant = 0;
        if (cosine >= 0 && sine >= 0) quadrant = 0;
        else if (cosine < 0 && sine >= 0) quadrant = 1;
        else if (cosine >= 0 && sine < 0) quadrant = 2;
        else if (cosine < 0 && sine < 0) quadrant = 3;
        double angle = Math.Atan2(sine, cosine) / 2.0 / Math.PI * 100.0;
        if (angle < 0) angle += 100.0;
        return angle;
    }

    public void SetTransform()
    {
        this.Stats.QuadrantX = X / 100.0 * 4.0;
        this.Stats.QuadrantY = Y / 100.0 * 4.0;
        this.Stats.QuadrantZ = Z / 100.0 * 4.0;
        this.Stats.CosineX = Math.Cos(X / 100.0 * 2.0 * Math.PI);
        this.Stats.CosineY = Math.Cos(Y / 100.0 * 2.0 * Math.PI);
        this.Stats.CosineZ = Math.Cos(Z / 100.0 * 2.0 * Math.PI);
        this.Stats.SineX = Math.Sin(X / 100.0 * 2.0 * Math.PI);
        this.Stats.SineY = Math.Sin(Y / 100.0 * 2.0 * Math.PI);
        this.Stats.SineZ = Math.Sin(Z / 100.0 * 2.0 * Math.PI);
        this.Stats.CX_CY_CZ = this.Stats.CosineX * this.Stats.CosineY * this.Stats.CosineZ;
        this.Stats.SX_SY_SZ = this.Stats.SineX * this.Stats.SineY * this.Stats.SineZ;
        this.Stats.CX_SY_SZ = this.Stats.CosineX * this.Stats.SineY * this.Stats.SineZ;
        this.Stats.SX_CY_CZ = this.Stats.SineX * this.Stats.CosineY * this.Stats.CosineZ;
        this.Stats.CX_SY_CZ = this.Stats.CosineX * this.Stats.SineY * this.Stats.CosineZ;
        this.Stats.SX_CY_SZ = this.Stats.SineX * this.Stats.CosineY * this.Stats.SineZ;
        this.Stats.CX_CY_SZ = this.Stats.CosineX * this.Stats.CosineY * this.Stats.SineZ;
        this.Stats.SX_SY_CZ = this.Stats.SineX * this.Stats.SineY * this.Stats.CosineZ;
        this.Stats.cW_p_sW = this.Stats.CX_CY_CZ + this.Stats.SX_SY_SZ;
        this.Stats.cW_m_sW = this.Stats.CX_CY_CZ - this.Stats.SX_SY_SZ;
        this.Stats.sW_m_sW = this.Stats.SX_SY_SZ - this.Stats.CX_CY_CZ;
        this.Stats.cX_p_sX = this.Stats.CX_SY_SZ + this.Stats.SX_CY_CZ;
        this.Stats.cX_m_sX = this.Stats.CX_SY_SZ - this.Stats.SX_CY_CZ;
        this.Stats.sX_m_cX = this.Stats.SX_CY_CZ - this.Stats.CX_SY_SZ;
        this.Stats.cY_p_sY = this.Stats.SX_CY_SZ + this.Stats.CX_SY_CZ;
        this.Stats.cY_m_sY = this.Stats.SX_CY_SZ - this.Stats.CX_SY_CZ;
        this.Stats.sY_m_cY = this.Stats.CX_SY_CZ - this.Stats.SX_CY_SZ;
        this.Stats.cZ_p_sZ = this.Stats.SX_SY_CZ + this.Stats.CX_CY_SZ;
        this.Stats.cZ_m_sZ = this.Stats.SX_SY_CZ - this.Stats.CX_CY_SZ;
        this.Stats.sZ_m_cZ = this.Stats.CX_CY_SZ - this.Stats.SX_SY_CZ;
        this.Stats.TestX = DetermineAngle(this.Stats.cW_p_sW, this.Stats.cX_p_sX);
        this.Stats.TestY = DetermineAngle(this.Stats.cW_p_sW, this.Stats.cY_p_sY);
        this.Stats.TestZ = DetermineAngle(this.Stats.cW_p_sW, this.Stats.cZ_p_sZ);
        this.Stats.TestW = DetermineAngle(this.Stats.CX_CY_CZ, this.Stats.SX_SY_SZ);
        this.Stats.TestXCS = DetermineAngle(this.Stats.CX_SY_SZ, this.Stats.SX_CY_CZ);
        this.Stats.TestYCS = DetermineAngle(this.Stats.SX_CY_SZ, this.Stats.CX_SY_CZ);
        this.Stats.TestZCS = DetermineAngle(this.Stats.SX_SY_CZ, this.Stats.CX_CY_SZ);
        this.transform.localRotation = RotationToQuaternion();
    }

    public Vector3 RotationToEuler()
    {
        return new Vector3((float)X / 100f * 360f, (float)Y / 100f * 360f, (float)Z / 100f * 360f);
    }

    public Vector4 SandwichProduct(Vector4 q1, Vector4 q2)
    {
        Vector4 q;
        q.w = -q1.x * q2.x - q1.y * q2.y - q1.z * q2.z + q1.w * q2.w;
        q.x = q1.x * q2.w + q1.y * q2.z - q1.z * q2.y + q1.w * q2.x;
        q.y = -q1.x * q2.z + q1.y * q2.w + q1.z * q2.x + q1.w * q2.y;
        q.z = q1.x * q2.y - q1.y * q2.x + q1.z * q2.w + q1.w * q2.z;
        return q;
    }

    public Quaternion CreateVectorSandwich(Vector4 a, Vector4 b, Vector4 c)
    {
        Vector4 v = SandwichProduct(SandwichProduct(a, b), c);
        Quaternion q = new Quaternion(v.x, v.y, v.z, v.w);
        return q;
    }

    public Quaternion RotationToQuaternion()
    {
        double x = ((double)X / 100.0) * 2.0 * Math.PI;
        double y = ((double)Y / 100.0) * 2.0 * Math.PI;
        double z = ((double)Z / 100.0) * 2.0 * Math.PI;
        x = x % 2.0 * Math.PI;
        y = y % 2.0 * Math.PI;
        z = z % 2.0 * Math.PI;

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

        Quaternion q5 = CreateVectorSandwich(ySandwich, xSandwich, zSandwich);
        return q5;
    }
}
