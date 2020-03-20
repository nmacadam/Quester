using XNode;

namespace Quester.QuestEditor
{
    public abstract class LogicNode : Node//QuestNode
    {
        [Input] public bool input;
        [Output] public bool output;
    }
}