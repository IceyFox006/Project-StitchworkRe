using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType
{
    SELF = 0,
    ALL = 10,
    ALL_EXSELF = 11,
    SINGLE_ENEMY = 20,
    ALL_ENEMIES = 30,
    SINGLE_ALLY = 40,
    ALL_ALLIES = 50,
}
[CreateAssetMenu(fileName = "ActionSO", menuName = "Scriptable Objects/Battle/Actions/Action")]
public class ActionSO : ScriptableObject
{
    [SerializeField] 
        protected string _name;
    [SerializeField]
        private AnimatorOverrideController _vfxAc;
    [SerializeField]
        protected TargetType target;
    [SerializeField][MinValue(0)] 
        protected int _power;
    [SerializeField][Range(-1, 3)][Tooltip("Items have priority of 3 (always go first).")]
        protected int _priority;
    [SerializeField] 
        protected EffectData[] _effects;

    #region GS
    public string Name { get => _name; set => _name = value; }
    public TargetType Target { get => target; set => target = value; }
    public AnimatorOverrideController VfxAc { get => _vfxAc; set => _vfxAc = value; }
    public int Priority { get => _priority; set => _priority = value; }
    #endregion
    public virtual void Use(ActiveFighter user, List<ActiveFighter> targets){}

    public virtual string AsString()
    {
        return GetType() + ": " + _name + "\nPriority " + _priority;
    }
}
//=====================================================================================================================
public enum EffectTag
{

}
[System.Serializable]
public class EffectData
{
    [SerializeField] 
        private EffectTag _effect;
    [SerializeField][Range(1, 100)][Tooltip("The chance of the effect occuring.")]
        private int _chance = 10;
    [SerializeField][MinValue(-1)][Tooltip("How many turns the effect lasts.\nPERMANENT = -1")]
        private int _duration;
    [SerializeField][Range(0.01f, 1)][Tooltip("STAT = The percentage boost of the stat.")] 
        private float _intensity = 0.1f;
}
