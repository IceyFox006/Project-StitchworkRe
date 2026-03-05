using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Fighter
{
    //Data
    [SerializeField] private string _name;
    [SerializeField] protected EntityParts _parts;
    [SerializeField] private ElementSO[] _elements;

    [SerializeField] protected int _level;
    [SerializeField] protected List<MoveSO> _moves;

    private int maxHP;
    private int maxEnergy;
    private Stats totalStats; 
    private Stats baseStats;
    [SerializeField] private Stats _madeStats = new Stats(15);
    [SerializeField] private Stats _trainedStats = new Stats(0);
    [SerializeField] private Stats _personalityStats = new Stats();

    //Item

    private float currentHP;
    private float currentEnergy;

    //Visuals
    [Tooltip("Max of 2.")]
    [SerializeField] private ColorPaletteSO[] _palettes;

    #region GS
    public int MaxHP { get => maxHP; set => maxHP = value; }
    public float CurrentHP { get => currentHP; set => currentHP = value; }
    public EntityParts Parts { get => _parts; set => _parts = value; }
    public ElementSO[] Elements { get => _elements; set => _elements = value; }
    public ColorPaletteSO[] Palettes { get => _palettes; set => _palettes = value; }
    #endregion

    public virtual void Initialize()
    {
        CalculateBaseStats();
        CalculateTotalStats();

        currentHP = maxHP; //TempRemove
        currentEnergy = maxEnergy; //TempRemove
    }

    public float GetNormalizedHP()
    {
        return currentHP / maxHP;
    }

    public float GetNormalizedEnergy()
    {
        return currentEnergy / maxEnergy;
    }

    #region Stats
    protected void CalculateTotalStats()
    {
        totalStats = new Stats();
        totalStats.Health = Mathf.FloorToInt(((2 * baseStats.Health + _madeStats.Health + (_trainedStats.Health) * _level) / 100) + _level + 10);
        totalStats.Endurance = Mathf.FloorToInt(((2 * baseStats.Endurance + _madeStats.Endurance + (_trainedStats.Endurance) * _level) / 100) + 5);
        totalStats.Strength = Mathf.FloorToInt(((2 * baseStats.Strength + _madeStats.Strength + (_trainedStats.Strength) * _level) / 100) + 5);
        totalStats.Magic = Mathf.FloorToInt(((2 * baseStats.Magic + _madeStats.Magic + (_trainedStats.Magic) * _level) / 100) + 5);
        totalStats.Agility = Mathf.FloorToInt(((2 * baseStats.Agility + _madeStats.Agility + (_trainedStats.Agility) * _level) / 100) + 5);

        maxHP = (int)totalStats.Health;
        maxEnergy = Mathf.FloorToInt((_level * 3) + (totalStats.GetTotal() / 10));
        totalStats = totalStats.Multiply(_personalityStats);
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
    #endregion
}

//=====================================================================================================================
[System.Serializable]
public class PlayerFighter : Fighter
{
    private GrowthRate growthRate;
    private int expToNextLvl;
    private float currentExp;
    private Dictionary<int, List<MoveSO>> levelMoves = new Dictionary<int, List<MoveSO>>();

    public override void Initialize()
    {
        base.Initialize();
        CalculateGrowthRate();
        expToNextLvl = GetExpInLevel(_level);
        GenerateLevelMoves();
    }

    #region Exp
    private void CalculateGrowthRate()
    {
        int grID = 0;
        foreach (PartSO part in _parts.ToList())
            grID += (int)part.GrowthRate;

        growthRate = (GrowthRate)Mathf.RoundToInt(grID / _parts.ToList().Count);
    }

    private int GetExpInLevel(int level)
    {
        switch (growthRate)
        {
            case GrowthRate.Fast: return (int)(4 * Mathf.Pow(level, 3) / 5);
            case GrowthRate.MediumFast: return (int)(Mathf.Pow(level, 3));
            case GrowthRate.MediumSlow: return (int)(((6 / 5) * Mathf.Pow(level, 3)) - (15 * level) + (100 * level) - 140);
            case GrowthRate.Slow: return (int)((5 * Mathf.Pow(level, 3)) / 4);
            default: return -1;
        }
    }
    private void LevelUp()
    {
        if (currentExp < expToNextLvl) return;

        _level++;
        expToNextLvl = GetExpInLevel(_level + 1);
        CalculateTotalStats();
        
        if (levelMoves.ContainsKey(_level))
            foreach (MoveSO move in levelMoves[_level]) LearnMove(move);

        LevelUp();
    }
    #endregion

    #region Move
    //Generates level moves with no duplicates.
    private void GenerateLevelMoves()
    {
        levelMoves.Clear();
        foreach (PartSO part in _parts.ToList()) //Iterate parts
        {
            for (int lvlID = 0; lvlID < levelMoves.Count; lvlID++) //Iterate levels in part level moves
            {
                foreach (MoveSO move in levelMoves[lvlID]) //Iterate moves in level
                {
                    if (IsDuplicateInLevelMoves(move)) continue;

                    if (!levelMoves.ContainsKey(lvlID)) levelMoves.Add(lvlID, new List<MoveSO>());
                    levelMoves[lvlID].Add(move);
                }
            }
        }
    }

    //Returns true if move is already in level moves.
    private bool IsDuplicateInLevelMoves(MoveSO move)
    {
        int count = 0;
        for (int lvlID = 0; lvlID < levelMoves.Count; lvlID++) //Iterate levels in level moves
        {
            foreach (MoveSO moveC in levelMoves.ElementAt(lvlID).Value) //Iterate moves in level
            {
                if (moveC == move) count++;
                if (count > 1) return true;
            }
        }
        return false;
    }

    private void LearnMove(MoveSO move)
    {
        //!!!CLAMP AMOUNT OF MOVES
        _moves.Add(move);
    }
    #endregion
}

//=====================================================================================================================
[System.Serializable]
public class EnemyFighter : Fighter
{

    private void GenerateWildMoves()
    {

    }
}


