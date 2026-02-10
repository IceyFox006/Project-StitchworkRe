using UnityEngine;

[System.Serializable]
public class Fighter
{
    [SerializeField] private string _name;
    [SerializeField] protected EntityParts _parts;
    [SerializeField] private int _level;

    private int maxHP; //!!!
    private int maxEnergy; //!!!
    private Stats totalStats; 
    private Stats baseStats;
    [SerializeField] private Stats _madeStats;
    [SerializeField] private Stats _trainedStats;
    //Personality

    public virtual void Initialize()
    {
        CalculateBaseStats();
        CalculateTotalStats();
    }

    private void CalculateTotalStats()
    {
        totalStats = new Stats();
        totalStats.Health = Mathf.FloorToInt(((2 * baseStats.Health + _madeStats.Health + (_trainedStats.Health) * _level) / 100) + _level + 10);
        totalStats.Endurance = Mathf.FloorToInt(((2 * baseStats.Endurance + _madeStats.Endurance + (_trainedStats.Endurance) * _level) / 100) + 5);
        totalStats.Strength = Mathf.FloorToInt(((2 * baseStats.Strength + _madeStats.Strength + (_trainedStats.Strength) * _level) / 100) + 5);
        totalStats.Magic = Mathf.FloorToInt(((2 * baseStats.Magic + _madeStats.Magic + (_trainedStats.Magic) * _level) / 100) + 5);
        totalStats.Agility = Mathf.FloorToInt(((2 * baseStats.Agility + _madeStats.Agility + (_trainedStats.Agility) * _level) / 100) + 5);

        maxHP = (int)totalStats.Health;
        maxEnergy = Mathf.FloorToInt((_level * 3) + (totalStats.GetTotal() / 10));
    }

    private void CalculateBaseStats()
    {
        baseStats = new Stats();
        foreach (PartSO part in _parts.ToList())
        {
            baseStats.Health += part.Stats.Health;
            baseStats.Endurance += part.Stats.Endurance;
            baseStats.Strength += part.Stats.Strength;
            baseStats.Magic += part.Stats.Magic;
            baseStats.Agility += part.Stats.Agility;
        }
    }
}

public class PlayerFighter : Fighter
{
    private GrowthRate growthRate;
    private int maxExp;
    private float currentExp;

    public override void Initialize()
    {
        base.Initialize();
        CalculateGrowthRate();
        GenerateLearnMoves();
    }
    private void CalculateGrowthRate()
    {
        int grID = 0;
        foreach (PartSO part in _parts.ToList())
            grID += (int)part.GrowthRate;

        growthRate = (GrowthRate)Mathf.RoundToInt(grID / _parts.ToList().Count);
    }

    private void GenerateLearnMoves()
    {
        
    }

    private void LevelUp()
    {
        //Calculate exp to next level
        //Update stats
        //Learn new moves
        //Level-Up
    }

}

public class EnemyFighter : Fighter
{

}

