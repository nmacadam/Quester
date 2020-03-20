using UnityEngine;

namespace Quester.QuestEditor
{
    public class CollectNode : QuestNode
    {
        public ObjectID Item;

        public override bool Evaluate()
        {
            return complete;
        }
    }
}