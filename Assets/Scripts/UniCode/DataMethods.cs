using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataMethods : MonoBehaviour
{
    #region Extension
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
