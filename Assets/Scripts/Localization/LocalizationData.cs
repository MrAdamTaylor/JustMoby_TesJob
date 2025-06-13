using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LocalizationData 
{
    public Dictionary<string, string> en;
    public Dictionary<string, string> ru;
    
    private Dictionary<string, string> _currentDictionary;

    public void SetRus()
    {
        _currentDictionary = ru;
    }

    public void SetEng()
    {
        _currentDictionary = en;
    }

    public string GetText(string key)
    {
        Dictionary<string, string> dict = _currentDictionary;
        
        if (dict != null && dict.TryGetValue(key, out string value))
        {
            return value;
        }
        return $"MISSING: {key}";
    }
}
