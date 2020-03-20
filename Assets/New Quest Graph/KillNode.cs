using UnityEngine;

namespace Quester.QuestEditor
{
    public class KillNode : QuestNode
    {
        public ObjectID Target;

        public override void InitializeObjective()
        {
            var killGameObject = ObjectIDMap.Instance.Get(Target);
            var objective = killGameObject.AddComponent<KillObjective>();
            objective.SetNode(this);
            if (!Active) objective.enabled = false;
        }

        public override bool Evaluate()
        {
            return complete;
        }
    }
}