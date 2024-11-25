using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbiltyPrefabSet", menuName = "Scriptable Objects/AbiltyPrefabSet")]
public class AbilityPrefabSet : ScriptableObject
{
    [SerializeField] private List<GameObject> prefabs;

    public int GetPrefabId(GameObject prefab)
    {
        return prefabs.IndexOf(prefab);
    }

    public GameObject GetPrefab(int id)
    {
        return prefabs[id];
    }
}
