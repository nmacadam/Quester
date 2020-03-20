using System.Linq;
using XNode;

namespace Quester.QuestEditor
{
    public class ExitPoint : Node
    {
        [Input] public bool exit;

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            var inputs = GetInputValues("exit", exit);

            // default to OR logic
            exit = inputs.Any(i => i);

            return exit;
        }
    }
}