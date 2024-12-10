using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Netcode;
using UnityEditor.Playables;
using UnityEngine;

public class AbilityRequestReceiver : NetworkBehaviour
{
    private Dictionary<ulong, List<IAbilityPredicates>> clientAbilityPredicates = new();

    [SerializeField] private AbilityPrefabSet abilityPrefabSet;

    public static AbilityRequestReceiver instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public int GetAbilityId(INetworkAbilityWithInstantiate ability)
    {
        return abilityPrefabSet.GetPrefabId(ability.Prefab);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void RequestInstantiateRpc(int prefabId)
    {
        Instantiate(abilityPrefabSet.GetPrefab(prefabId));
    }

    public void AddClientAbilityPredicates(ulong clientId, List<IAbilityPredicates> abilitiesPredicates)
    {
        if (IsClient) return;
        if (clientAbilityPredicates.ContainsKey(clientId)) Debug.LogError("The client has already been added");

        clientAbilityPredicates.Add(clientId, abilitiesPredicates);
    }

    public bool CheckPredicates(ulong clientId, int id)
    {
        if (IsClient) return false;

        List<Func<bool>> predicates;
        if (clientAbilityPredicates.TryGetValue(clientId, out List<IAbilityPredicates> list))
            predicates = list.First(x => x.Id == id).Predicates;
        else return false;

        foreach (Func<bool> predicate in predicates)
        {
            if(predicate.Invoke() == false)
                return false;
        }

        return true;
    }
}
