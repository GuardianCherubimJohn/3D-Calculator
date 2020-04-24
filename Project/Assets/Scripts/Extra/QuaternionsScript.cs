using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class QuaternionsScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StringBuilder sb = new StringBuilder();
        for (float w = -1.0f; w <= 1.0f; w += 0.1f)
        {
            for (float z = -1.0f; z <= 1.0f; z += 0.1f)
            {
                for (float y = -1.0f; y <= 1.0f; y += 0.1f)
                {
                    for (float x = -1.0f; x <= 1.0f; x += 0.1f)
                    {
                        Vector4 quat = new Vector4(x, y, z, w);
                        Quaternion q = new Quaternion(x, y, z, w);
                        Vector3 euler1 = ToEulerAngle(quat);
                        Vector3 euler2 = q.eulerAngles;
                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", q.x, q.y, q.z, q.w, euler1.x, euler1.y, euler1.z, euler2.x, euler2.y, euler2.z));
                        //Debug.Log(string.Format("Quat Vec:{0} Q:{1} Euler1:{2} Euler2:{3}", quat, q, euler1, euler2));
                    }
                }
            }
        }
        System.IO.File.WriteAllText(@"C:\\Users\\johnr\\Desktop\\Quaternion.txt", sb.ToString());
        Debug.Log(sb.ToString());
    }

    Vector3 ToEulerAngle(Vector4 q)
    {
	    // roll (x-axis rotation)
	    float sinr = +2.0f * (q.w * q.x + q.y * q.z);
        float cosr = +1.0f - 2.0f * (q.x * q.x + q.y * q.y);
        float roll = Mathf.Atan2(sinr, cosr);

        // pitch (y-axis rotation)
        float sinp = +2.0f * (q.w * q.y - q.z * q.x);
        float pitch = 0f;
        if (Mathf.Abs(sinp) >= 1.0f)
        {
            float sign = Mathf.Sign(sinp);
            pitch = Mathf.PI / 2.0f * sign; // use 90 degrees if out of range
        }
        else
		    pitch = Mathf.Asin(sinp);

        // yaw (z-axis rotation)
        float siny = +2.0f * (q.w * q.z + q.x * q.y);
        float cosy = +1.0f - 2.0f * (q.y * q.y + q.z * q.z);
        float yaw = Mathf.Atan2(siny, cosy);
        //roll = roll * 180f / Mathf.PI;
        //pitch = pitch * 180f / Mathf.PI;
        //yaw = yaw * 180f / Mathf.PI;
        return new Vector3(roll, pitch, yaw);
    }

// Update is called once per frame
void Update () {
		
	}
}
