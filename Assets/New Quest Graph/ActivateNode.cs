using UnityEngine;

namespace Quester.QuestEditor
{
    public class ActivateNode : QuestNode
    {
        public ObjectID Interactable;

        public override void InitializeObjective()
        {
            var interactableGameObject = ObjectIDMap.Instance.Get(Interactable);
            var objective = interactableGameObject.AddComponent<ActivateObjective>();
            objective.SetNode(this);
            if (!Active) objective.enabled = false;
        }

        public override bool Evaluate()
        {
            return complete;
        }
    }
}