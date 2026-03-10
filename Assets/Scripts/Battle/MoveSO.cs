using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Physical,
    Special,
    Status,
}
public enum TargetType
{
    SELF,
    ALL,
    SINGLE_ENEMY,
    ALL_ENEMIES,
    SINGLE_ALLY,
    ALL_ALLIES,
}

[CreateAssetMenu(fileName = "MoveSO", menuName = "Scriptable Objects/Battle/Move")]
public class MoveSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _icon;
    [SerializeField][TextArea(1,5)] private string _description;

    [SerializeField] private ElementSO _element; 
    [SerializeField] private int _energyCost; //!!!

    [SerializeField][Range(1, 101)] private int _accuracy;
    [SerializeField] private TargetType _targetType;

    [SerializeField] private DamageType _damageType; 
    [SerializeField][MinValue(0)] private int _power;

    [SerializeField] private EffectData[] _effects; //!!!

    #region GS
    public string Name { get => _name; set => _name = value; }
    public Sprite Icon { get => _icon; set => _icon = value; }
    public TargetType TargetType { get => _targetType; set => _targetType = value; }
    #endregion

    public void Use(ActiveFighter user, List<ActiveFighter> targets)
    {
        //Accuracy Check
        if (Random.Range(0, 101) > _accuracy)
        {
            Debug.Log("MISSED " + _name + "!");
            return;
        }    

        //Damage Calculate
        float attack = 1;
        switch (_damageType)
        {
            case DamageType.Physical: attack = user.FluxStats.Strength; break;
            case DamageType.Special: attack = user.FluxStats.Magic; break;
            case DamageType.Status: attack = 1; break;
        }

        float power;
        foreach (ActiveFighter target in  targets)
        {
            power = ((((2 * user.Data.Level) / 5) + 2) * _power * (attack / target.FluxStats.Endurance)/ 50) + 2; //status effect * weather * terrain
            power *= target.Data.GetEffectivenessMultiplier(_element); //Weak, resistant, immune.
            power *= user.Data.GetSTABMultiplier(_element); //User element = move element.

            power = (int)power;
        }

    }
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

