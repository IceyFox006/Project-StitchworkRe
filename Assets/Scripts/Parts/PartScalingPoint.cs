using UnityEngine;
public enum Axis
{
    x,
    y,
    z,
}

[RequireComponent(typeof(MeshFilter))]
public class PartScalingPoint : MonoBehaviour
{
    [SerializeField][Range(0.1f, 1)] private float _scaleMultiplier = 0.7f;
    [SerializeField] private Axis _unscaledAxis;

    #region
    public float ScaleMultiplier { get => _scaleMultiplier; set => _scaleMultiplier = value; }
    public Axis UnscaledAxis { get => _unscaledAxis; set => _unscaledAxis = value; }
    #endregion
}
