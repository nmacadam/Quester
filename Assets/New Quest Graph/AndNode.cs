using System.Linq;
using XNode;

namespace Quester.QuestEditor
{
    public class AndNode : LogicNode
    {
        public override object GetValue(NodePort port)
        {
            var inputs = GetInputValues("input", input);

            // default to OR logic
            input = inputs.All(i => i);

            output = input;

            return output;
        }
    }
}