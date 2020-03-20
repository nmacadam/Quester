using UnityEngine;

namespace Quester.QuestEditor
{
    public class DeliverNode : QuestNode
    {
        public ObjectID Deliverable;
        public ActorID Receiver;

        public override bool Evaluate()
        {
            return complete;
        }
    }
}