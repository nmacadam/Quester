using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace Quester.QuestEditor.InternalEditor
{
    [CustomNodeGraphEditor(typeof(QuestGraph))]
    public class QuestGraphEditor : NodeGraphEditor
    {
        public override string GetNodeMenuName(System.Type type)
        {
            if (type.Namespace == "Quester.QuestEditor")
            {
                return base.GetNodeMenuName(type).Replace("Quest Editor/", "");
            }
            else return null;
        }

        /// <summary> Controls graph noodle colors </summary>
        public override Gradient GetNoodleGradient(NodePort output, NodePort input)
        {
            bool value = false;
            if (output.node is LogicNode logic) value = logic.output;
            else if (output.node is QuestNode questNode) value = questNode.output;

            var trueGradient = new Gradient();
            trueGradient.SetKeys(new [] { new GradientColorKey(Color.green, 0f) }, new GradientAlphaKey[0]);

            var falseGradient = new Gradient();
            falseGradient.SetKeys(new[] { new GradientColorKey(Color.red, 0f) }, new GradientAlphaKey[0]);

            return value ? trueGradient : falseGradient;
        }

        public override void OnGUI()
        {
            base.OnGUI();

            //TopToolbar(toolbarRect);

            //if (GUILayout.Button("Update Database Entry")) ((QuestGraph)serializedObject.targetObject).UpdateDatabaseEntry();
        }

        //Rect toolbarRect
        //{
        //    get { return new Rect(0f, 0f, window.position.width, 20f); }
        //}

        //void TopToolbar(Rect rect)
        //{
        //    GUILayout.BeginArea(rect);
        //    GUILayout.BeginHorizontal();

        //    var style = new GUIStyle();
        //    style.fontSize = 14;

        //    GUILayout.Label("Quest Editor", style);

        //    if (GUILayout.Button("New"))
        //    {
        //        //quests = _connection.Table<QuestEntry>();
        //    }
        //    if (GUILayout.Button("Refresh"))
        //    {
        //        //if (_connection == null) Connect();
        //        //quests = _connection.Table<QuestEntry>();
        //        //m_Initialized = false;
        //        //InitIfNeeded();
        //        //Repaint();
        //    }

        //    //treeView.searchString = m_SearchField.OnGUI(treeView.searchString);

        //    GUILayout.EndHorizontal();
        //    GUILayout.EndArea();
        //}
    }
}