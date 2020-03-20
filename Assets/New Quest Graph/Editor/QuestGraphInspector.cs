using System.Collections;
using System.Collections.Generic;
using Quester.QuestEditor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestGraph))]
public class QuestGraphInspector : Editor
{
    private QuestGraph graph;

    private void OnEnable()
    {
        graph = (QuestGraph)serializedObject.targetObject;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(10f);
        if (GUILayout.Button("Update Database Entry")) { graph.UpdateDatabaseEntry(); }
    }
}
