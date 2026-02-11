using UnityEngine;

public class EntityAssemble : MonoBehaviour
{
    [SerializeField] private EntityParts _parts;
    private void Start()
    {
        Assemble();
    }
    private void Assemble()
    {
        GameObject body = Instantiate(_parts.Body.Model, transform);

        PartConnectionPoint[] connections = GetComponentsInChildren<PartConnectionPoint>();

        foreach (PartConnectionPoint connectionPoint in connections)
        {
            switch (connectionPoint.PartType)
            {
                case PartType.Head: Instantiate(_parts.Head.Model, connectionPoint.transform); break;
                case PartType.Arm: Instantiate(_parts.Arm.Model, connectionPoint.transform); break;
                case PartType.Leg: Instantiate(_parts.Leg.Model, connectionPoint.transform); break;
                case PartType.Tail: Instantiate(_parts.Tail.Model, connectionPoint.transform); break;
            }
        }
    }
}
