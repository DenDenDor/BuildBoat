using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class ListUtility
{
    private static readonly Dictionary<object, bool> s_reverseDictionary = new();
    private static readonly Dictionary<object, int> s_randomDictionary = new();

    private static void ThrowIndexOutOfRangeException<T>(List<T> collection, T element)
    {
        if (collection.Contains(element) == false)
            throw new IndexOutOfRangeException("No such element in collection!");
    }

    private static int GetIndex<T>(List<T> collection, T element)
    {
        if (s_reverseDictionary.ContainsKey(collection) == false)
            s_reverseDictionary.Add(collection, true);

        return collection.IndexOf(element);
    }

    private static int GetNextIndex<T>(List<T> collection, int index)
    {
        int positiveStep = 1;
        int negativeStep = -1;

        int step = s_reverseDictionary[collection] ? positiveStep : negativeStep;

        return index + step;
    }

    private static int GetRandomIndex<T>(List<T> collection) => Random.Range(0, collection.Count);

    public static T GetNextElementInLoop<T>(this List<T> collection, T element)
    {
        ThrowIndexOutOfRangeException(collection, element);

        int index = collection.IndexOf(element) + 1;

        if (index >= collection.Count)
            return collection[0];

        return collection[index];
    }

    public static T GetNextElementInReverse<T>(this List<T> collection, T element)
    {
        ThrowIndexOutOfRangeException(collection, element);

        int index = GetIndex(collection, element);

        if (index == 0)
            s_reverseDictionary[collection] = true;
        else if (index == collection.Count - 1)
            s_reverseDictionary[collection] = false;

        return collection[GetNextIndex(collection, index)];
    }

    public static T GetNextElementInRepeatReverse<T>(this List<T> collection, T element)
    {
        ThrowIndexOutOfRangeException(collection, element);

        int index = GetIndex(collection, element);

        if (index == 0 && s_reverseDictionary[collection] == false)
        {
            s_reverseDictionary[collection] = true;
            return collection[index];
        }

        if (index == collection.Count - 1 && s_reverseDictionary[collection])
        {
            s_reverseDictionary[collection] = false;
            return collection[index];
        }

        return collection[GetNextIndex(collection, index)];
    }

    public static T GetRandomRepeatElement<T>(this List<T> collection)
    {
        int index = GetRandomIndex(collection);

        return collection[index];
    }

    public static T GetRandomElement<T>(this List<T> collection)
    {
        int index = GetRandomIndex(collection);

        if (s_randomDictionary.ContainsKey(collection) == false)
        {
            s_randomDictionary.Add(collection, -1);
        }

        if (s_randomDictionary[collection] == index)
        {
            bool isWorking = true;

            while (isWorking)
            {
                int number = Random.Range(0, collection.Count);

                if (index != number)
                {
                    index = number;
                    isWorking = false;
                }
            }
        }

        s_randomDictionary[collection] = index;

        return collection[index];
    }
    
    public static void DestroyAllMonoBehaviours<T, U>(this Dictionary<T, U> dictionary) where T : MonoBehaviour
    {
        foreach (var kvp in dictionary)
        {
            if (kvp.Key != null)
            {
                Object.Destroy(kvp.Key.gameObject);
            }
        }
    }   
    
    public static void DestroyAllTransforms<T, U>(this Dictionary<T, U> dictionary) where T : Transform
    {
        foreach (var kvp in dictionary)
        {
            if (kvp.Key != null)
            {
                Object.Destroy(kvp.Key.gameObject);
            }
        }
    }
}