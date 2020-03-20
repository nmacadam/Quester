using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Object ID", menuName = "Object ID")]
public class ObjectID : ScriptableObject
{
    private Guid _guid = Guid.NewGuid();

    public Guid GUID => _guid;
}