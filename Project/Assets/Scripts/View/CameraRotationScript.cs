using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationScript : MonoBehaviour {

    public Camera cam;
    public float angle = 1f;
    public float posInc = 250f;

    UnityEngine.UI.InputField[] inputFields;

	// Use this for initialization
	void Start () {
        inputFields = GameObject.FindObjectsOfType<UnityEngine.UI.InputField>();
	}

    bool IsUIInputFocused()
    {
        for (int z=0;z<inputFields.Length;z++)
        {
            if (inputFields[z].isFocused) return true;
        }
        return false;
    }
	
	// Update is called once per frame
	void Update () {
        if (!IsUIInputFocused())
        {
            Quaternion currentRotation = cam.transform.localRotation;
            Vector3 rotate = Vector3.zero;
            if (Input.GetKey(KeyCode.UpArrow)) rotate.x += angle;
            if (Input.GetKey(KeyCode.DownArrow)) rotate.x -= angle;
            if (Input.GetKey(KeyCode.LeftArrow)) rotate.y -= angle;
            if (Input.GetKey(KeyCode.RightArrow)) rotate.y += angle;
            if (Input.GetKey(KeyCode.PageUp)) rotate.z -= angle;
            if (Input.GetKey(KeyCode.PageDown)) rotate.z += angle;
            cam.transform.localRotation = currentRotation * Quaternion.Euler(rotate);

            Vector3 pos = cam.transform.localPosition;
            float posIncInc = posInc;
            if (Input.GetKey(KeyCode.LeftShift)) posIncInc *= 2f;
            if (Input.GetKey(KeyCode.A)) pos += (posIncInc * cam.transform.right * -1f);
            if (Input.GetKey(KeyCode.D)) pos += (posIncInc * cam.transform.right);
            if (Input.GetKey(KeyCode.W)) pos += (posIncInc * cam.transform.forward);
            if (Input.GetKey(KeyCode.S)) pos += (posIncInc * cam.transform.forward * -1f);
            if (Input.GetKey(KeyCode.Q)) pos += (posIncInc * cam.transform.up);
            if (Input.GetKey(KeyCode.E)) pos += (posIncInc * cam.transform.up * -1f);
            cam.transform.localPosition = pos;
        }
	}
}
