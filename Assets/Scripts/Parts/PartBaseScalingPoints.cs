using UnityEngine;

public class PartBaseScalingPoints : MonoBehaviour
{
    [SerializeField] private MeshFilter _headMesh;

    [SerializeField] private MeshFilter _tailMesh;

    #region GS
    public MeshFilter HeadMesh { get => _headMesh; set => _headMesh = value; }
    public MeshFilter TailMesh { get => _tailMesh; set => _tailMesh = value; }
    #endregion
}
