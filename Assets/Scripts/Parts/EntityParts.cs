using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum PartType
{
    Head,
    Arm,
    Body,
    Leg,
    Tail,
}

[System.Serializable]
public class EntityParts
{
    [SerializeField] private PartSO _head;
    [SerializeField] private PartSO _arm;
    [SerializeField] private PartSO _body;
    [SerializeField] private PartSO _leg;
    [SerializeField] private PartSO _tail;

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
