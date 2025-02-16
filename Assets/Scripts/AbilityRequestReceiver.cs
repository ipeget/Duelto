using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class AbilityRequestReceiver : NetworkBehaviour
{
    private Dictionary<ulong, List<IAbility>> clientAbilityPredicates = new();

    private IAbility ability; //current executable ability

    [SerializeField] private AbilityPrefabSet abilityPrefabSet;

    public static AbilityRequestReceiver instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public int GetPrefabId(INetworkAbilityWithInstantiate ability)
    {
        return abilityPrefabSet.GetPrefabId(ability.Prefab);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void RequestInstantiateRpc(int prefabId)
    {
        if (prefabId == -1) throw new ArgumentNullException("Prefab doesn't exist.");
        Instantiate(abilityPrefabSet.GetPrefab(prefabId));
    }

    public void AddClientAbilityPredicates(ulong clientId, List<IAbility> abilitiesPredicates)
    {
        if (!IsHost) return;
        if (clientAbilityPredicates.ContainsKey(clientId)) Debug.LogError("The client has already been added");

        clientAbilityPredicates.Add(clientId, abilitiesPredicates);
    }

    public bool TryExecuteAbility(ulong clientId, int id)
    {
        if (!IsHost) return false;

        List<Func<bool>> predicates;


        if (clientAbilityPredicates.TryGetValue(clientId, out List<IAbility> list))
        {
            ability = list.First(x => x.Id == id);
            predicates = ability.Predicates;
        }
        else return false;

        foreach (Func<bool> predicate in predicates)
        {
            if (predicate.Invoke() == false)
                return false;
        }

        ability.Execute();
        TimerHandler.instance.StartTimer(clientId, ability.CooldownTimer);

        return true;
    }
}
