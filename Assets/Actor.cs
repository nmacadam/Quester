using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public ActorID ID;
    public Stage stage;
    
    private void Awake()
    {
        stage.actors.Add(ID, this);
    }
}
