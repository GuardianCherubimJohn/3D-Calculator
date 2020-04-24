using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringsRenderer : MonoBehaviour {

    public Text[] TextLines;
    public string MaskName;

	// Use this for initialization
	void Start () {
        BuildText(this.transform, MaskName, TextLines);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void DeleteChildObjects(Transform parent, string keyMask)
    {
        for (int z = parent.childCount - 1; z >= 0; z--)
        {
            Transform t = parent.GetChild(z);
            if (t.name.ToLower().StartsWith(keyMask))
            {
                DestroyImmediate(t.gameObject);
            }
        }
    }

    public static void CreateChildObject(Transform parent, string name, Text text)
    {
        GameObject g = new GameObject();
        g.transform.parent = parent;
        g.transform.name = name;
        g.transform.localPosition = text.Location;
        g.transform.localRotation = Quaternion.Euler(text.Rotation);
        g.transform.localScale = text.Scale;
        var t = g.AddComponent<UnityEngine.TextMesh>();
        t.text = text.TextString;
        t.fontSize = text.FontSize;
        t.font = text.Font;
        t.fontStyle = text.FontStyle;
        t.font.material = text.FontMaterial;
        MeshRenderer rend = t.gameObject.GetComponentInChildren<MeshRenderer>();
        rend.material = t.font.material;  /* ADDED THIS */
        text.Transform = g.transform;
    }


    private static void BuildText(Transform parent, string maskName, Text[] textLines)
    {
        DeleteChildObjects(parent, maskName);
        for (int z=0;z<textLines.Length;z++)
        {
            CreateChildObject(parent, maskName + "_Text" + z, textLines[z]);
        }
    }

    [Serializable]
    public class Text
    {
        public string TextString;
        public Vector3 Location;
        public Vector3 Rotation;
        public Vector3 Scale = new Vector3(1f,1f,1f);
        public Transform Transform;
        public int FontSize = 32;
        public UnityEngine.FontStyle FontStyle = FontStyle.Normal;
        public UnityEngine.Font Font;
        public Material FontMaterial;
    }
}
