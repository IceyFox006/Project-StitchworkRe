using NaughtyAttributes;
using UnityEngine;

public enum DamageType
{
    Physical,
    Special,
    Status,
}

[CreateAssetMenu(fileName = "MoveSO", menuName = "Scriptable Objects/Battle/Move")]
public class MoveSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField][TextArea(1,5)] private string _description;

    [SerializeField] private ElementSO _element; //!!!
    [SerializeField] private int _energyCost; //!!!
    [SerializeField][Range(1, 100)] private int _accuracy; //!!!
    [SerializeField] private DamageType _damageType; //!!!
    [SerializeField][MinValue(0)] private int _power; //!!!
    [SerializeField] private EffectData[] _effects; //!!!
}

//=====================================================================================================================
public enum EffectTag
{

}
[System.Serializable]
public class EffectData
{
    [SerializeField] private EffectTag _effect;

    [Tooltip("The chance of the effect occuring.")]
    [SerializeField][Range(1, 100)] private int _chance = 100;

    [Tooltip("How many turns the effect lasts.\nPERMANENT = -1")]
    [SerializeField][MinValue(-1)] private int _duration;

    [Tooltip("STAT = The percentage boost of the stat.")]
    [SerializeField][Range(1, 100)] private float _intensity;
}

