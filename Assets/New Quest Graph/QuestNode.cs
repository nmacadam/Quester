using System.Linq;
using XNode;

namespace Quester.QuestEditor
{
     //[System.Serializable]

    public abstract class QuestNode : Node
    {
        public bool complete;

        public bool Active => (!complete && input);

        [Input] public bool input;
        [Output] public bool output;

        public abstract void InitializeObjective();

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