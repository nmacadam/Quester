using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Quester.QuestEditor.InternalEditor
{
    [CustomNodeEditor(typeof(LogicNode))]
    public class LogicNodeEditor : NodeEditor
    {
        private LogicNode node;

        public override void OnHeaderGUI()
        {
            // Initialization
            if (node == null)
            {
                node = target as LogicNode;
            }

            base.OnHeaderGUI();
            Rect dotRectIn = GUILayoutUtility.GetLastRect();
            dotRectIn.size = new Vector2(16, 16);
            dotRectIn.y += 6;

            Rect dotRectOut = GUILayoutUtility.GetLastRect();
            dotRectOut.size = new Vector2(16, 16);
            dotRectOut.y += 6;
            dotRectOut.x = this.GetWidth() - 32;

            GUI.color = node.input ? Color.green : Color.red;
            GUI.DrawTexture(dotRectIn, NodeEditorResources.dot);
            GUI.color = node.output ? Color.green : Color.red;
            GUI.DrawTexture(dotRectOut, NodeEditorResources.dot);
            GUI.color = Color.white;
        }

        public override void OnBodyGUI()
        {
            if (target == null)
            {
                Debug.LogWarning("Null target node for node editor!");
                return;
            }
            EditorGUIUtility.labelWidth = 60;

            base.OnBodyGUI();
        }
    }
}