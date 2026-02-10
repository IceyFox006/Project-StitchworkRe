using AYellowpaper.SerializedCollections;
using UnityEngine;

//=====================================================================================================================
public enum SortTag
{
    Consumable,
    Part,
    Material,
    Key,
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/Item/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Item")]

    [SerializeField] private string _name;
    [SerializeField][TextArea(1, 5)] private string _description;

    [SerializeField] private SortTag _sortTag;
    [SerializeField][Range(0, 99)] private int _stackSize; //!!!
}

//=====================================================================================================================
public enum GrowthRate
{
    Fast = 1, //600,000
    MediumFast = 2, //800,000
    MediumSlow = 3, //1,000,000
    Slow = 4, //1,200,000
}

[CreateAssetMenu(fileName = "PartSO", menuName = "Scriptable Objects/Item/Part")]
public class PartSO : ItemSO
{
    [Header("Part")]

    [SerializeField] private GameObject _model;

    [SerializeField] private PartType _type;
    [SerializeField] private Stats _stats;
    [SerializeField] private GrowthRate _growthRate;
    [SerializeField] private int _baseExp;
    [SerializedDictionary("Level", "Move")][SerializeField] private SerializedDictionary<int, MoveSO[]> _learnMoves = new SerializedDictionary<int, MoveSO[]>();

    #region GS
    public Stats Stats { get => _stats; set => _stats = value; }
    public GrowthRate GrowthRate { get => _growthRate; set => _growthRate = value; }
    #endregion
}
