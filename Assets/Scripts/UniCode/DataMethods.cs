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

    #region Extension
    //Adds amount to index. If it is out of range, cycles around.
    public static int NextIndex<T>(int index, List<T> list)
    {
        return ShiftIndex(index, list, 1);
    }

    public static int PreviousIndex<T>(int index, List<T> list)
    {
        return ShiftIndex(index, list, -1);
    }

    public static int ShiftIndex<T>(int index, List<T> list, int amount)
    {
        if (amount > 0)
            return (index + amount < list.Count) ? index + amount : list.Count % amount;
        if (amount < 0)
            return (index + amount > -1) ? index + amount : list.Count - 1 + (list.Count % amount);
        return index;
    }

    //Adds value to the array and returns the array.
    public static T[] AddToArray<T>(T[] array, T value)
    {
        List<T> list = array.ToList();
        list.Add(value);
        return list.ToArray();
    }

    //Removes the value at index from the array and returns the array.
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

    //Removes the value at index from list and returns the removed value.
    public static T RemoveAt<T>(List<T> list, int index)
    {
        T value = list[index];
        list.RemoveAt(index);
        return value;
    }

    public static void Combine<T>(ref List<T> l1, List<T> l2)
    {
        for (int i = 0; i < l2.Count; i++)
            l1.Add(l2[i]);
    }

    public static List<T> Clone<T>(List<T> list)
    {
        List<T> clone = new List<T>();
        foreach(T var in list)
            clone.Add(var);
        return clone;
    }
    #endregion
}
