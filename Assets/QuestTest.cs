using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Quester.QuestEditor;
using Quester.Utilities;
using UnityEngine;

public class QuestTest : MonoBehaviour
{
    public QuestGraph graph;

    [Button()]
    public void testMapA()
    {
        foreach (var state in graph.GetCompletionMap())
        {
            Debug.Log($"[{state.NodeIterator}, {state.IsComplete}]");
        }

        Debug.Log(JsonHelper.ToJson(graph.GetCompletionMap()));
    }

    [Button()]
    public void testMapB()
    {
        var fakeMap = new QuestGraph.CompletionMarker[]
        {
            new QuestGraph.CompletionMarker(2,true), 
            new QuestGraph.CompletionMarker(3,true), 
            new QuestGraph.CompletionMarker(4,true) 
        };

        graph.ApplyCompletionMap(fakeMap);

        foreach (var state in graph.GetCompletionMap())
        {
            Debug.Log($"[{state.NodeIterator}, {state.IsComplete}]");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
