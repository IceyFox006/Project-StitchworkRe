using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataMethods : MonoBehaviour
{
    public static string GenerateSeed()
    {
        int tempSeed = (int)System.DateTime.Now.Ticks;
        return tempSeed.ToString();
    }
    //public static string GenerateID(MonoBehaviour mb)
    //{
    //    string ID = mb.GetType().ToString() + "_";

    //    ID += mb.transform.position.x.ToString() + mb.transform.position.y.ToString() + mb.transform.position.z.ToString();

    //    return ID;
    //}
    #region Extension

    public static int NextIndex<T>(int index, List<T> list)
    {
        return ShiftIndex(index, list, 1);//(index + 1 < list.Count)? index : 0;
    }
    public static int PreviousIndex<T>(int index, List<T> list)
    {
        return ShiftIndex(index, list, -1);//(index - 1 > -1)? index : list.Count;
    }
    public static int ShiftIndex<T>(int index, List<T> list, int amount)
    {
        if (amount > 0)
            return (index + amount < list.Count) ? index + amount : list.Count % amount;
        if (amount < 0)
            return (index + amount > -1) ? index + amount : list.Count - 1 + (list.Count % amount);
        return index;
    }
    public static T[] AddToArray<T>(T[] array, T value)
    {
        List<T> list = array.ToList();
        list.Add(value);
        return list.ToArray();
    }

    public static T[] RemoveFirstFromArray<T>(T[] array)
    {
        return RemoveFromArrayAt(array, 0);
    }
    public static T[] RemoveLastFromArray<T>(T[] array)
    {
        return RemoveFromArrayAt(array, array.Length - 1);
    }
    public static T[] RemoveFromArrayAt<T>(T[] array, int index)
    {
        List<T> list = array.ToList();
        list.RemoveAt(index);
        return list.ToArray();
    }

    public static T RemoveAt<T>(ref List<T> list, int index)
    {
        T value = list[index];
        list.RemoveAt(index);
        return value;
    }
    #endregion
}
