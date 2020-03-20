using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

namespace Quester.QuestEditor.InternalEditor
{
    [CustomNodeEditor(typeof(EntryPoint))]
    public class EntryPointEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            base.OnBodyGUI();
            EntryPoint node = target as EntryPoint;
            QuestGraph graph = node.graph as QuestGraph;
            //if (GUILayout.Button("Start")) node.Start();
            //if (GUILayout.Button("Continue")) graph.Continue();
        }
    }
}