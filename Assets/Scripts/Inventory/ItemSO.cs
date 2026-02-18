using UnityEngine;

public enum SortTag
{
    Consumable,
    Part,
    Material,
    Key,
    Miscellaneous,
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/Item/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Item")]

    [SerializeField] private string _name;
    [SerializeField][TextArea(1, 5)] private string _description;

    [SerializeField] private SortTag _sortTag;
    [SerializeField][Range(1, 99)] private int _stackSize; //!!!
}


