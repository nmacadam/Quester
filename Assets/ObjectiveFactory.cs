//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public abstract class ObjectiveFactory
//{
//    protected ObjectiveFactory()
//    {
//        CreateObjective();
//    }

//    private List<ObjectiveDetail> details;
//    public List<ObjectiveDetail> Details => details;

//    public abstract void CreateObjective();
//}

//public class TalkToObjective : ObjectiveFactory
//{
//    public override void CreateObjective()
//    {
//        Details.Add(new ObjectiveDetail("Talk to $NAME_1"));
//        Details.Add(new ObjectiveDetail("Talk to $NAME_2"));
//    }
//}

//[System.Serializable]
//public class ObjectiveDetail
//{
//    public ObjectiveDetail(string data)
//    {
//        detail = data;
//    }
//    public string detail;
//}

////}

////public class TalkToObjective : Objective
////{

////}