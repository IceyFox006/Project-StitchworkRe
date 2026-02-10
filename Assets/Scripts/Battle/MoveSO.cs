using UnityEngine;

public enum EffectTag
{

}

[CreateAssetMenu(fileName = "MoveSO", menuName = "Scriptable Objects/MoveSO")]
public class MoveSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField][TextArea(1,5)] private string _description;

    [SerializeField] private int _energyCost; //!!!
    [SerializeField][Range(0, 1)] private float _accuracy; //!!!
    [SerializeField] private int _power; //!!!
    [SerializeField] private EffectTag[] _moveEffects; //!!!
}
