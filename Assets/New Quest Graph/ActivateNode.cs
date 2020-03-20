using UnityEngine;

namespace Quester.QuestEditor
{
    public class ActivateNode : QuestNode
    {
        public ObjectID Interactable;

        public override bool Evaluate()
        {
            return complete;
        }
    }
}