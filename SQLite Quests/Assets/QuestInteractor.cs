using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class QuestInteractor : MonoBehaviour
{
    [SerializeField] protected QuestManager Manager;
    [SerializeField] protected Quest Quest;
    [SerializeField] protected Objective Objective;
    
    public void Initialize(QuestManager manager, Quest quest, Objective objective)
    {
        Manager = manager;
        Quest = quest;
        Objective = objective;
    }

    public void CompleteObjective()
    {
        Quest.CompleteObjective(Objective);
    }
}

public class QuestTalkToInterator : QuestInteractor
{
    private void Awake()
    {
        // get component or something for checking when talked to
    }

    [Button]
    public void Talk()
    {
        CompleteObjective();
    }
}