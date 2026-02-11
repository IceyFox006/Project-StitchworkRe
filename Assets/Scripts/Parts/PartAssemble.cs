using UnityEngine;

public class PartAssemble : MonoBehaviour
{
    [SerializeField] private EntityParts _parts;

    private GameObject body;
    private GameObject head;
    private GameObject tail;

    private PartBaseScalingPoints baseScalingPoints;
    private void Start()
    {
        Assemble();

        baseScalingPoints = body.GetComponentInChildren<PartBaseScalingPoints>();
        Scale(baseScalingPoints.HeadMesh, head.transform);
        Scale(baseScalingPoints.TailMesh, tail.transform);
    }

    //Instantiates parts
    private void Assemble()
    {
        body = Instantiate(_parts.Body.Model, transform);

        PartConnectionPoint[] connections = GetComponentsInChildren<PartConnectionPoint>();

        foreach (PartConnectionPoint connectionPoint in connections) //Iterates connection points
        {
            switch (connectionPoint.PartType)
            {
                case PartType.Head: head = Instantiate(_parts.Head.Model, connectionPoint.transform); break;
                case PartType.Arm: Instantiate(_parts.Arm.Model, connectionPoint.transform); break;
                case PartType.Leg: Instantiate(_parts.Leg.Model, connectionPoint.transform); break;
                case PartType.Tail: tail = Instantiate(_parts.Tail.Model, connectionPoint.transform); break;
            }
        }
    }

    private void Scale(MeshFilter baseMesh, Transform scalePart)
    {
        PartScalingPoint scalePoint = scalePart.GetComponentInChildren<PartScalingPoint>();
        MeshFilter scaleMesh = scalePoint.GetComponent<MeshFilter>();
        float scaleMultiplier = scalePoint.ScaleMultiplier;

        Debug.Log("Base: " + baseMesh.mesh.bounds.size);
        Debug.Log("Scale: " + scaleMesh.mesh.bounds.size);

        Vector3 baseSize = baseMesh.mesh.bounds.size;
        Vector3 scaleSize = scaleMesh.mesh.bounds.size;
        Vector3 newScale = new Vector3();
        switch (scalePoint.UnscaledAxis)
        {
            case Axis.x: newScale = new Vector3(1, (baseSize.y / scaleSize.y) * scaleMultiplier, (baseSize.z / scaleSize.z) * scaleMultiplier); break;
            case Axis.y: newScale = new Vector3((baseSize.x / scaleSize.x) * scaleMultiplier, 1, (baseSize.z / scaleSize.z) * scaleMultiplier); break;
            case Axis.z: default: newScale = new Vector3((baseSize.x / scaleSize.x) * scaleMultiplier, (baseSize.y / scaleSize.y) * scaleMultiplier, 1); break;
        }

        scalePart.localScale = newScale;
    }
}
