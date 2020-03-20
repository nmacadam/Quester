using UnityEngine;

namespace Quester.QuestEditor
{
    public class KillNode : QuestNode
    {
        public ObjectID Target;

        public override bool Evaluate()
        {
            return complete;
        }
    }
}