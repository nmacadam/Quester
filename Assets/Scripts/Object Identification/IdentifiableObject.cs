using UnityEngine;

public class IdentifiableObject : MonoBehaviour
{
    [SerializeField] private ObjectID _id;

    private void Awake()
    {
        RegisterSelf();
    }

    protected void RegisterSelf()
    {
        ObjectIDMap.Instance.Add(_id, gameObject);
    }
}