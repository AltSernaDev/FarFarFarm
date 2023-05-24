using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerilizableDictionary<Tkey, Tvalue> : Dictionary<Tkey, Tvalue>, ISerializationCallbackReceiver
{
    List<Tkey> keys = new List<Tkey>();
    List<Tvalue> values = new List<Tvalue>();

    public void OnAfterDeserialize()
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<Tkey, Tvalue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnBeforeSerialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
        {
            Debug.LogError("The size of the Keys("+ keys.Count + ") and Values("+ values.Count + ") ??does not match,");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}
