using NaughtyAttributes;
using Quester.QuestEditor;
using UnityEngine;

public class TalkToObjective : QuestObjective
{
    //private TalkToNode _node;
    //public override QuestNode AttachedNode => _node;

    public GameObject Speaker;

    //public override void SetNode(QuestNode node)
    //{
    //    if (node is TalkToNode talk) _node = talk;
    //    else Debug.LogError("Node type mismatch");
    //}

    public override void Accept(IQuestVisitor visitor)
    {
        visitor.Visit(this);
    }

    [Button]
    public void Talk()
    {
        Complete();
    }
}