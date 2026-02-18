using UnityEngine;

[System.Serializable]
public class Stats
{
    [SerializeField] private float _health; //hit points
    [SerializeField] private float _endurance; //defence
    [SerializeField] private float _strength; //physical attack
    [SerializeField] private float _magic; //special attack
    [SerializeField] private float _agility; //speed, evasiveness

    [SerializeField] private float _luck = 1f; //critical rate, drop quality
    [SerializeField] private float _regen = 1f; //healing, energy regen

    public Stats()
    {
        _health = 0;
        _endurance = 0;
        _strength = 0;
        _magic = 0;
        _agility = 0;
    }
    public Stats(float health, float endurance, float strength, float magic, float agility)
    {
        _health = health;
        _endurance = endurance;
        _strength = strength;
        _magic = magic;
        _agility = agility;
    }

    #region GS
    public float Health { get => _health; set => _health = value; }
    public float Endurance { get => _endurance; set => _endurance = value; }
    public float Strength { get => _strength; set => _strength = value; }
    public float Magic { get => _magic; set => _magic = value; }
    public float Agility { get => _agility; set => _agility = value; }
    #endregion

    public float GetTotal()
    {
        return _health + _endurance + _strength + _magic + _agility;
    }

    public Stats Multiply(Stats stats)
    {
        return new Stats(_health * stats.Health, _endurance * stats.Endurance, _strength * stats.Strength, _magic * stats.Magic, _agility * stats.Agility);
    }
}
