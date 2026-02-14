using UnityEngine;

[CreateAssetMenu(fileName = "ColorPaletteSO", menuName = "Scriptable Objects/Data/ColorPalette")]
public class ColorPaletteSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Color _highlight;
    [SerializeField] private Color _base;
    [SerializeField] private Color _shade1;
    [SerializeField] private Color _shade2;

    public Color[] ToArray()
    {
        return new Color[] {_highlight, _base, _shade1, _shade2};
    }
}
