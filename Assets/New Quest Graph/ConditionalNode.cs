using System.Linq;
using XNode;

namespace Quester.QuestEditor
{
    public class ConditionalNode : LogicNode
    {
        [Input] public bool condition;
        [Output] public bool elseOutput;

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            var inputs = GetInputValues("input", input);
            input = inputs.Any(i => i);

            var conditions = GetInputValues("condition", condition);
            condition = conditions.Any(i => i);

            if (condition)
            {
                output = input;
                elseOutput = false;
            }
            else
            {
                output = false;
                elseOutput = input;
            }

            return output;
        }
    }
}