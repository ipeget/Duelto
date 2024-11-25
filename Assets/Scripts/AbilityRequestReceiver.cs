using Unity.Netcode;
using UnityEngine;

public class AbilityRequestReceiver : NetworkBehaviour
{
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
}
