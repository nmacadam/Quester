using UnityEngine;

namespace Quester.QuestEditor
{
    public class CollectNode : QuestNode
    {
        public ObjectID Collectable;

        public override void InitializeObjective()
        {
            var collectGameObject = ObjectIDMap.Instance.Get(Collectable);
            var objective = collectGameObject.AddComponent<CollectObjective>();
            objective.SetNode(this);
            if (!Active) objective.enabled = false;
        }

        public override bool Evaluate()
        {
            return complete;
        }
    }
}