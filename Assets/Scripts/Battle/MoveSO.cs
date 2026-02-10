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
    [SerializeField][Range(0, 1)] private float _accuracy; //!!!
    [SerializeField] private DamageType _damageType; //!!!
    [SerializeField] private int _power; //!!!
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
    [SerializeField] private float _intensity;
}

