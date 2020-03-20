using NaughtyAttributes;
using Quester.QuestEditor;
using UnityEngine;

namespace Quester
{
    public class TalkToObjective : QuestObjective
    {
        public GameObject Speaker;

        [Button]
        public void Talk()
        {
            Complete();
        }
    }

    public class ActivateObjective : QuestObjective
    {
        public GameObject Interactable;

        [Button]
        public void Activate()
        {
            Complete();
        }
    }

    public class CollectObjective : QuestObjective
    {
        public GameObject Collectable;

        [Button]
        public void Collect()
        {
            Complete();
        }
    }

    public class DeliverObjective : QuestObjective
    {
        public GameObject Deliverable;
        public GameObject Receiver;

        [Button]
        public void Deliver()
        {
            Complete();
        }
    }

    public class KillObjective : QuestObjective
    {
        public GameObject Target;

        [Button]
        public void Kill()
        {
            Complete();
        }
    }
}