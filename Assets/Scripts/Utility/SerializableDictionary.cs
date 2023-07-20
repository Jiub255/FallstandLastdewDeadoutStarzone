using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<T, U>
{
    [SerializeField]
    private List<SKVP<T, U>> _skvps;

    public List<SKVP<T, U>> SKVPS { get { return _skvps; } set { _skvps = value; } }

    public SerializableDictionary()
    {
        _skvps = new();
    }


    public U this[T key]
    {
        get
        {
            foreach (SKVP<T, U> skvp in SKVPS)
            {
                if (EqualityComparer<T>.Default.Equals(skvp.Key, key))
                {
                    return skvp.Value;
                }
            }

            Debug.LogWarning($"No key {key} found in dictionary.");
            return default(U);
        }
    }

    public SKVP<T, U> this[int index]
    {
        get
        {
            return _skvps[index];
        }
    }

    public void Add(T key, U value)
    {
        _skvps.Add(new SKVP<T, U>(key, value));
    }

    public void Remove(T key)
    {
        foreach (SKVP<T, U> skvp in SKVPS)
        {
            if (EqualityComparer<T>.Default.Equals(skvp.Key, key))
            {
                _skvps.Remove(skvp);
                return;
            }
        }

        Debug.LogWarning($"No key {key} found in dictionary.");
    }

    public int Count()
    {
        return _skvps.Count;
    }
}

[System.Serializable]
public class SKVP<T, U>
{
    [SerializeField]
    private T _key;
    [SerializeField]
    private U _value;

    public T Key { get { return _key; } }
    public U Value { get { return _value; } }

    public SKVP(T key, U value)
    {
        _key = key;
        _value = value;
    }
}