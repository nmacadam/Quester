using UnityEngine;

namespace Quester.QuestEditor
{
    public class TalkToNode : QuestNode
    {
        public ObjectID Speaker;

        public override void InitializeObjective()
        {
            var speakerGameObject = ObjectIDMap.Instance.Get(Speaker);
            var objective = speakerGameObject.AddComponent<TalkToObjective>();
            objective.SetNode(this);
            if (!Active) objective.enabled = false;
        }

        public override bool Evaluate()
        {
            return complete;
        }
    }
}