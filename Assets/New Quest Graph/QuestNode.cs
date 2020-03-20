using System.Linq;
using XNode;

namespace Quester.QuestEditor
{
     //[System.Serializable]

    public abstract class QuestNode : Node
    {
        //public bool active;
        public bool complete;

        public bool Active => (!complete && input);

        [Input] public bool input;
        [Output] public bool output;

        //public void MoveNext()
        //{
        //    //QuestGraph qGraph = graph as QuestGraph;

        //    //if (!qGraph.leaves.Contains(this))
        //    //{
        //    //    Debug.LogWarning("Node isn't active");
        //    //    return;
        //    //}

        //    //NodePort exitPort = GetOutputPort("output");

        //    //if (!exitPort.IsConnected)
        //    //{
        //    //    Debug.LogWarning("Node isn't connected");
        //    //    return;
        //    //}

        //    //if (!(bool)GetValue(exitPort))
        //    //{
        //    //    Debug.LogWarning("Node evaluate to false");
        //    //    return;
        //    //}

        //    //Debug.Log($"Exiting node '{GetType()}'");

        //    //var nodePorts = exitPort.GetConnections();
        //    //qGraph.leaves.Clear();
        //    //foreach (var port in nodePorts)
        //    //{
        //    //    if (port.node is LogicNode lnode)
        //    //    {
        //    //        lnode.GetValue(port.node.GetOutputPort("output"));
        //    //        if (lnode.output == false)
        //    //        {
        //    //            qGraph.leaves.Add(this);
        //    //            continue;
        //    //        }
        //    //    }

        //    //    var node = port.node as QuestNode;
        //    //    Debug.Log($"Entering node '{node.GetType()}'");
        //    //    qGraph.leaves.Add(node);
        //    //    node.MoveNext();
        //    //}

        //    //qGraph.Continue();

        //    //QuestNode node = exitPort.Connection.node as QuestNode;
        //    //node.OnEnter();
        //}

        //public void OnEnter()
        //{
        //    QuestGraph qGraph = graph as QuestGraph;
        //    qGraph.leaves.Add(this);
        //}

        public abstract bool Evaluate();

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            var inputs = GetInputValues("input", input);

            // default to OR logic
            input = inputs.Any(i => i);

            output = input && Evaluate();

            return output;
        }
    }
}