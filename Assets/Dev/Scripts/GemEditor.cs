using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gem", menuName = "ScriptableObjects/GemEditorScriptableObject", order = 2)]

public class GemEditor : ScriptableObject
{
    public List<Gem> GemModels;
}
[Serializable]
public class Gem : ICloneable
{   
    public GameObject gemPrefab;
    public string gemName;
    public Colors.Color gemColor;
    public float gemStartPrice;
    public Sprite gemSprite;
    public GameObject collectedGemPrefab;    
    public int collectedGemCount;

    public object Clone()
    {
        var clone = MemberwiseClone() as Gem;

        return clone;
    }
}
