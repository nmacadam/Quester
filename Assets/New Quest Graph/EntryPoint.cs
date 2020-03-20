using XNode;

namespace Quester.QuestEditor
{
    public class EntryPoint : Node
    {
        [Output] public bool start = true;

        public override object GetValue(NodePort port)
        {
            return true;
        }
    }
}