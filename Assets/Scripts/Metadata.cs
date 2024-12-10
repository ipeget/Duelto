using System;
using UnityEngine;

[Serializable]
public class Metadata
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private string extendeDescription;

    public Sprite Icon => icon;
    public string Name => name;
    public string Description => description;
    public string ExtensionDescription => extendeDescription;
}
