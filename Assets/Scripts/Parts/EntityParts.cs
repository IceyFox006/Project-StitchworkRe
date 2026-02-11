using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityParts
{
    [SerializeField] private PartSO _head;
    [SerializeField] private PartSO _arm;
    [SerializeField] private PartSO _body;
    [SerializeField] private PartSO _leg;
    [SerializeField] private PartSO _tail;

    #region GS
    public PartSO Head { get => _head; set => _head = value; }
    public PartSO Arm { get => _arm; set => _arm = value; }
    public PartSO Body { get => _body; set => _body = value; }
    public PartSO Leg { get => _leg; set => _leg = value; }
    public PartSO Tail { get => _tail; set => _tail = value; }
    #endregion

    public PartSO[] ToArray()
    {
        return new PartSO[] { _head, _arm, _body, _leg, _tail };
    }
    public List<PartSO> ToList()
    {
        List<PartSO> parts = new List<PartSO>();

        foreach (PartSO part in ToArray())
            if (part != null) parts.Add(part);

        return parts;
    }
}
