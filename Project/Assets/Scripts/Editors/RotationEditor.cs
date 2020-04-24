using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(RotationRangeAttribute))]
public class RotationEditor : PropertyDrawer {

    private double value;

    //
    // Methods
    //
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var rangeAttribute = (RotationRangeAttribute)base.attribute;

        value = EditorGUI.DoubleField(position, label, value);

        //if (property.propertyType == SerializedPropertyType.doub)
        //{
        //value = EditorGUI.DoubleField(position, label, value, rangeAttribute.min, rangeAttribute.max);
        //int vin = (int)(value * rangeAttribute.step);
        //EditorGUI.IntSlider(position, vin, rangeAttribute.minimum, rangeAttribute.maximum);
        ////int v = EditorGUI.IntField(position, label, value);

        //double v = (double)value / (double)rangeAttribute.maximum;
        //value = v;
        //value = (value * rangeAttribute.step);
        //property.doubleValue = value;
        //}
        //else
        //{
        //    EditorGUI.LabelField(position, label.text, "Use Range with float or int.");
        //}
    }
}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyEditor : PropertyDrawer
{

    private double value;

    //
    // Methods
    //
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var rangeAttribute = (ReadOnlyAttribute)base.attribute;
        double v = property.doubleValue;
        string val = v.ToString();
        string current = label.text;
        label.text = string.Format("{0}: {1}", current, val);
        EditorGUI.LabelField(position, label);
        //value = EditorGUI.DoubleField(position, label, value);
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class RotationRangeAttribute : PropertyAttribute
{
    public readonly int step;
    public readonly int minimum;
    public readonly int maximum;

    public RotationRangeAttribute(int minimum, int maximum, int step)
    {
        this.step = step;
        this.minimum = minimum;
        this.maximum = maximum;
    }
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
public class ReadOnlyAttribute : PropertyAttribute
{
    public ReadOnlyAttribute()
    {

    }
}