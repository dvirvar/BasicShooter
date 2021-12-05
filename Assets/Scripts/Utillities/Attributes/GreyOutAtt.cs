using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[SerializeField]
public class GreyOutAtt : PropertyAttribute
{
    
}

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(GreyOutAtt))]
    public class GreyOutDrawer: PropertyDrawer
    {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
#endif