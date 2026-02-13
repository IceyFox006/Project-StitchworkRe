using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PartScalingPoint : MonoBehaviour
{
    [SerializeField][Range(0.1f, 1)] private float _scaleMultiplier = 0.7f;

    #region
    public float ScaleMultiplier { get => _scaleMultiplier; set => _scaleMultiplier = value; }
    #endregion
}
