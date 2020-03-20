using UnityEngine;

namespace Quester.QuestEditor
{
    public class DeliverNode : QuestNode
    {
        public ObjectID Deliverable;
        public ActorID Receiver;

        public override void InitializeObjective()
        {
            // hmm...
            var deliverGameObject = ObjectIDMap.Instance.Get(Receiver);
            var objective = deliverGameObject.AddComponent<DeliverObjective>();
            objective.SetNode(this);
            if (!Active) objective.enabled = false;
        }

        public override bool Evaluate()
        {
            return complete;
        }
    }
}