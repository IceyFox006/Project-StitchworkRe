using UnityEngine;

[RequireComponent(typeof(ApplyPartColorRegions))]
public class PartAssemble : MonoBehaviour
{
    [SerializeField] private EntityParts _parts;
    private ApplyPartColorRegions regions;

    private GameObject body;
    private GameObject head;
    private GameObject tail;

    private PartBaseScalingPoints baseScalingPoints;

    #region
    public ApplyPartColorRegions Regions { get => regions; set => regions = value; }
    #endregion

    private void Start()
    {
        //regions = GetComponent<ApplyPartColorRegions>();

        //Initialize();
    }

    [HideInInspector]
    public void Initialize()
    {
        Initialize(_parts);
    }

    [HideInInspector]
    public void Initialize(EntityParts parts)
    {
        _parts = parts;
        regions = GetComponent<ApplyPartColorRegions>();

        Assemble();
        regions.ApplyColorRegions();
        ScaleAll();
    }

    [HideInInspector]
    public void Initialize(EntityParts parts, ColorPaletteSO[] palettes)
    {
        _parts = parts;
        regions = GetComponent<ApplyPartColorRegions>();

        Assemble();
        regions.ApplyColorRegions(palettes);
        ScaleAll();
    }

    #region Assemble
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
    #endregion

    #region Scale
    private void ScaleAll()
    {
        baseScalingPoints = body.GetComponentInChildren<PartBaseScalingPoints>();
        Scale(baseScalingPoints.HeadMesh, head.transform);
        Scale(baseScalingPoints.TailMesh, tail.transform);
    }

    //Scales connecting parts to match the largest value (x or y) of the base part.
    private void Scale(MeshFilter baseMesh, Transform scalePart)
    {
        PartScalingPoint scalePoint = scalePart.GetComponentInChildren<PartScalingPoint>();
        MeshFilter scaleMesh = scalePoint.GetComponent<MeshFilter>();
        float scaleMultiplier = scalePoint.ScaleMultiplier;

        Vector3 baseSize = baseMesh.mesh.bounds.size;
        Vector3 scaleSize = scaleMesh.mesh.bounds.size;
        Vector3 newScale = new Vector3();

        if (baseSize.x < baseSize.y)
            newScale = new Vector3((baseSize.x / scaleSize.x) * scaleMultiplier, (baseSize.x / scaleSize.x) * scaleMultiplier, (baseSize.x / scaleSize.x) * scaleMultiplier);
        else
            newScale = new Vector3((baseSize.y / scaleSize.y) * scaleMultiplier, (baseSize.y / scaleSize.y) * scaleMultiplier, (baseSize.y / scaleSize.y) * scaleMultiplier);

        scalePart.localScale = newScale;
    }
    #endregion
}
