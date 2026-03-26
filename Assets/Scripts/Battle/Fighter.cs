using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Fighter
{
    //Data
    [SerializeField] private string _name;
    private string id;
    [SerializeField] 
        protected EntityParts _parts;

    [SerializeField]
        private ElementSO[] _elements;
    private List<ElementEffectiveness> effectiveness;

    [SerializeField] 
        protected int _level;
    [SerializeField] 
        protected List<MoveSO> _moves;
    protected Dictionary<int, List<MoveSO>> levelMoves = new Dictionary<int, List<MoveSO>>();

    private int maxHP;
    private int maxEnergy;
    private Stats totalStats; 
    private Stats baseStats; //Average of all part stats. 
    [SerializeField] 
        private Stats _madeStats = new Stats(15); //Determined on creation. (IVs)
    [SerializeField] 
        private Stats _trainedStats = new Stats(0); //Determined by experience. (EVs)
    [SerializeField] 
        private Stats _personalityStats = new Stats();

    //Item

    private float curHP;
    private float curEnergy;

    //Visuals
    [Tooltip("Max of 2.")]
    [SerializeField] private ColorPaletteSO[] _palettes;

    #region GS
    public int MaxHP { get => maxHP; set => maxHP = value; }
    public float CurHP { get => curHP; set => curHP = value; }
    public EntityParts Parts { get => _parts; set => _parts = value; }
    public ElementSO[] Elements { get => _elements; set => _elements = value; }
    public ColorPaletteSO[] Palettes { get => _palettes; set => _palettes = value; }
    public List<MoveSO> Moves { get => _moves; set => _moves = value; }
    public int Level { get => _level; set => _level = value; }
    public Stats TotalStats { get => totalStats; set => totalStats = value; }
    public string Name { get => _name; set => _name = value; }
    public string Id { get => id; set => id = value; }
    public float CurEnergy { get => curEnergy; set => curEnergy = value; }
    public int MaxEnergy { get => maxEnergy; set => maxEnergy = value; }
    #endregion

    public virtual void Initialize()
    {
        id = DataMethods.GenerateSeed();
        CalculateBaseStats();
        CalculateTotalStats();
        CalculateElementEffectiveness();

        curHP = maxHP; //TempRemove
        curEnergy = maxEnergy; //TempRemove
    }

    #region Get
    public float GetNormalizedHP() => 
        curHP / maxHP;

    public float GetNormalizedEnergy() => 
        curEnergy / maxEnergy;

    //Returns the effectivenessMultiplier of the attacking element vs the fighterElements.
    public float GetEffectivenessMultiplier(ElementSO attackingElement)
    {
        int index = ElementEffectiveness.FindElement(attackingElement, effectiveness);

        if (index < 0) return 1;
        return effectiveness[index].Multiplier;
    }

    public float GetSTABMultiplier(ElementSO usingElement)
    {
        foreach (ElementSO element in _elements)
            if (element == usingElement) return BattleManager.STABMultiplier;
        return 1;
    }
    #endregion
    #region Set
    //Sets currentHP at amount and clamps between 0 and maxHP.
    public void SetHP(float amount)
    {
        curHP = amount;
        ClampHP();
    }

    public void SetEnergy(float amount)
    {
        curEnergy = amount;
        ClampEnergy();
    }
    #endregion

    #region Calculate
    protected void CalculateTotalStats()
    {
        totalStats = new Stats();
        totalStats.Health = (((2 * baseStats.Health + _madeStats.Health + (_trainedStats.Health / 4)) * _level) / 100) + _level + 10;
        
        totalStats.Endurance = (((2 * baseStats.Endurance + _madeStats.Endurance + (_trainedStats.Endurance / 4)) * _level) / 100) + 5;
        totalStats.Strength =(((2 * baseStats.Strength + _madeStats.Strength + (_trainedStats.Strength / 4)) * _level) / 100) + 5;
        totalStats.Magic = (((2 * baseStats.Magic + _madeStats.Magic + (_trainedStats.Magic / 4)) * _level) / 100) + 5;
        totalStats.Agility = (((2 * baseStats.Agility + _madeStats.Agility + (_trainedStats.Agility / 4)) * _level) / 100) + 5;

        maxHP = (int)totalStats.Health;
        maxEnergy = (int)((_level * 3) + (totalStats.GetTotal() / 10));
        totalStats.Multiply(_personalityStats);
    }

    private void CalculateBaseStats()
    {
        baseStats = new Stats(0);
        PartSO[] parts = _parts.ToArray();

        foreach (PartSO part in parts)
            baseStats.Add(part.Stats);
        baseStats.Divide(parts.Length);
    }

    //Sets the effectiveness that each attacking element has when hitting the fighter.
    private void CalculateElementEffectiveness()
    {
        effectiveness = new List<ElementEffectiveness>();

        int index;
        foreach (ElementSO fElement in _elements)
        {
            //Weaknesses
            foreach (ElementSO wElement in fElement.Weaknesses)
                effectiveness.Add(new ElementEffectiveness(wElement, 1 + BattleManager.effectivenessMultiplier));

            //Resistances
            foreach (ElementSO rElement in fElement.Resistances)
            {
                index = ElementEffectiveness.FindElement(rElement, effectiveness);
                if (index < 0)
                    effectiveness.Add(new ElementEffectiveness(rElement, 1 - BattleManager.effectivenessMultiplier));
                else
                    effectiveness[index].Multiplier -= BattleManager.effectivenessMultiplier;
            }

            //Immunities
            foreach (ElementSO iElement in fElement.Immunities)
            {
                index = ElementEffectiveness.FindElement(iElement, effectiveness);
                if (index < 0)
                    effectiveness.Add(new ElementEffectiveness(iElement, 1 - BattleManager.effectivenessMultiplier));
                else
                    effectiveness[index].Multiplier = 0;
            }
        }
    }

    #endregion
    #region Utility
    public string AsString()
    {
        return GetType() + ": " + _name + "\n" + totalStats.AsString();
    }
    public bool EqualTo(Fighter fighter)
    {
        return (id.Equals(fighter.Id));
    }

    //Clamps curHP between 0 and maxHP.
    private void ClampHP() =>
        curHP = Mathf.Clamp(curHP, 0, maxHP);
    
    //Clamps curEnergy between 0 and maxEnergy.
    private void ClampEnergy() =>
        curEnergy = Mathf.Clamp(curEnergy, 0, maxEnergy);
    #endregion

    #region Moves
    //Generates level moves with no duplicates.
    protected void GenerateLevelMoves()
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
    #endregion
}

//---------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class PlayerFighter : Fighter
{
    private GrowthRate growthRate;
    private int expToNextLvl;
    private float currentExp;


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
    private void LearnMove(MoveSO move)
    {
        //!!!CLAMP AMOUNT OF MOVES
        _moves.Add(move);
    }
    #endregion
}

//---------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class EnemyFighter : Fighter
{
}

//---------------------------------------------------------------------------------------------------------------------
[System.Serializable]
public class WildFighter : EnemyFighter
{
    public override void Initialize()
    {
        base.Initialize();
        GenerateLevelMoves();
        GenerateWildMoves();
    }
    private void GenerateWildMoves()
    {

    }
}



