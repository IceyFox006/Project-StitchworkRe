using UnityEngine;

[CreateAssetMenu(fileName = "ElementSO", menuName = "Scriptable Objects/Battle/Element")]
public class ElementSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private ElementSO[] _weaknesses;
    [SerializeField] private ElementSO[] _resistances;
    [SerializeField] private ElementSO[] _immunities;
}
