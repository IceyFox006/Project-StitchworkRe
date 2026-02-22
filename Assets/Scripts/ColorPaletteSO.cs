using UnityEngine;

[CreateAssetMenu(fileName = "ColorPaletteSO", menuName = "Scriptable Objects/Data/ColorPalette")]
public class ColorPaletteSO : ScriptableObject
{
    [SerializeField] private string _name;

    [Tooltip("Contains 4 colors: highlight, base, shade1 and shade2.")]
    [SerializeField] private Color[] _colors = new Color[4];

    #region GS
    public Color[] Colors { get => _colors; set => _colors = value; }
    public string Name { get => _name; set => _name = value; }
    #endregion
}
