using UnityEngine;

namespace Quester.QuestEditor
{
    public class TalkToNode : QuestNode
    {
        public ObjectID Speaker;

        public override bool Evaluate()
        {
            return complete;
        }
    }
}