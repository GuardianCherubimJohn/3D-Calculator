using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesSceneScript : MonoBehaviour
{
    public Transform Cube1;
    public Transform Cube1Minus;
    public Transform Cube2;
    public TextMesh Cube1Text;
    public TextMesh Cube1MinusText;
    public TextMesh Cube2Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cube1Text.text = $"[{Cube1.localRotation.w},{Cube1.localRotation.x},{Cube1.localRotation.y},{Cube1.localRotation.z}]" + System.Environment.NewLine + $"[{Cube1.localEulerAngles.x},{Cube1.localEulerAngles.y},{Cube1.localEulerAngles.z}]";
        Cube1MinusText.text = $"[{Cube1Minus.localRotation.w},{Cube1Minus.localRotation.x},{Cube1Minus.localRotation.y},{Cube1Minus.localRotation.z}]" + System.Environment.NewLine + $"[{Cube1Minus.localEulerAngles.x},{Cube1Minus.localEulerAngles.y},{Cube1Minus.localEulerAngles.z}]";
        Cube2Text.text = $"[{Cube2.localRotation.w},{Cube2.localRotation.x},{Cube2.localRotation.y},{Cube2.localRotation.z}]" + System.Environment.NewLine + $"[{Cube2.localEulerAngles.x},{Cube2.localEulerAngles.y},{Cube2.localEulerAngles.z}]";
    }
}
