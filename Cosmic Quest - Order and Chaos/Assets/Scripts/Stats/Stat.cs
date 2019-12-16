using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    public int baseValue;
    private List<int> _modifiers = new List<int>();

    public int GetValue()
    {
        int value = baseValue;
        _modifiers.ForEach(mod => value += mod);
        return value;
    }

    public void AddModifier(int modifier)
    {
        if (modifier != 0)
            _modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
            _modifiers.Remove(modifier);
    }
}
