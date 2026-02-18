using UnityEngine;

[CreateAssetMenu(fileName = "ElementSO", menuName = "Scriptable Objects/Battle/Element")]
public class ElementSO : ScriptableObject
{
    [Header("Data")]
    [SerializeField] private string _name;
    [SerializeField] private ElementSO[] _weaknesses;
    [SerializeField] private ElementSO[] _resistances;
    [SerializeField] private ElementSO[] _immunities;

    [Header("Visuals")]
    [SerializeField] private ColorPaletteSO[] _palettes = new ColorPaletteSO[1];
}
