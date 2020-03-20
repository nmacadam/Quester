using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

namespace Quester.QuestEditor.InternalEditor
{
    [CustomNodeEditor(typeof(QuestNode))]
    public class QuestNodeEditor : NodeEditor
    {
        public override void OnHeaderGUI()
        {
            GUI.color = Color.white;
            QuestNode node = target as QuestNode;
            QuestGraph graph = node.graph as QuestGraph;
            //if (graph.leaves.Contains(node)) GUI.color = Color.yellow;
            if (node.Active) GUI.color = Color.yellow;
            string title = target.name;
            GUILayout.Label(title, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
            GUI.color = Color.white;
        }

        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            QuestNode node = target as QuestNode;
            QuestGraph graph = node.graph as QuestGraph;
            //if (GUILayout.Button("MoveNext Node")) node.MoveNext();
            //if (GUILayout.Button("Continue Graph")) graph.Continue();
            //if (GUILayout.Button("Set as current"))
            //{
            //    graph.leaves.Clear();
            //    graph.leaves.Add(node);
            //}
        }
    }
}