using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectID), true)]
public class ObjectIDEditor : Editor
{
    private string guid;

    void OnEnable()
    {
        var obj = (ObjectID)serializedObject.targetObject;
        guid = obj.GUID.ToString();
    }

    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        EditorGUILayout.TextField("GUID", guid);
        GUI.enabled = true;

        DrawDefaultInspector();
    }
}
