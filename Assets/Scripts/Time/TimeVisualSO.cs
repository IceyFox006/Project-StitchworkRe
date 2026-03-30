using UnityEngine;

[CreateAssetMenu(fileName = "TimeVisualSO", menuName = "Scriptable Objects/TimeVisual")]
public class TimeVisualSO : ScriptableObject
{
    [SerializeField] private Gradient _directionalColor;
    [SerializeField] private Gradient _ambientColor;
    [SerializeField] private Gradient _fogColor;

    #region GS
    public Gradient DirectionalColor { get => _directionalColor; set => _directionalColor = value; }
    public Gradient AmbientColor { get => _ambientColor; set => _ambientColor = value; }
    public Gradient FogColor { get => _fogColor; set => _fogColor = value; }
    #endregion
}
