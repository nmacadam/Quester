using System.Linq;
using XNode;

namespace Quester.QuestEditor
{
    public class NotNode : LogicNode
    {
        public override object GetValue(NodePort port)
        {
            var inputs = GetInputValues("input", input);

            // default to OR logic
            input = inputs.Any(i => i);

            // ignore Evaluate function
            output = !input;

            return output;
        }
    }
}