using System.Collections.Generic;
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

    #region GS
    public ElementSO[] Weaknesses { get => _weaknesses; set => _weaknesses = value; }
    public ElementSO[] Resistances { get => _resistances; set => _resistances = value; }
    public ElementSO[] Immunities { get => _immunities; set => _immunities = value; }
    #endregion
}
public class ElementEffectiveness
{
    private ElementSO element;
    private float multiplier;

    public ElementEffectiveness(ElementSO element, float multiplier)
    {
        this.element = element;
        this.multiplier = multiplier;
    }

    #region GS
    public ElementSO Element { get => element; set => element = value; }
    public float Multiplier { get => multiplier; set => multiplier = value; }
    #endregion

    public static int FindElement(ElementSO target, List<ElementEffectiveness> list)
    {
        for (int i = 0; i < list.Count; i++)
            if (list[i].Element == target) return i;

        return -1;
    }
}

