using UnityEngine;

public enum PartType
{
    Head,
    Arm,
    Body,
    Leg,
    Tail,
}

public class PartConnectionPoint : MonoBehaviour
{
    [SerializeField] private PartType _partType;

    #region GS
    public PartType PartType { get => _partType; set => _partType = value; }
    #endregion
}
