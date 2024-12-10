using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAbility : NetworkBehaviour
{
    public PlayerAbilityConfig playerAbilityConfig;
    private List<Ability> abilities;
    private void Start()
    {
        if (IsOwner)
        {
            abilities = playerAbilityConfig.GetAbilities();

            Debug.Log("PlayerAbility Initialize");
            foreach (var ability in abilities)
            {
                ability.Initialize();
                ability.inputAction.performed += _ => TryExecuteAbilityRPC(ability.Id);
            }
        }
    }

    [Rpc(SendTo.Server)]
    public void TryExecuteAbilityRPC(int id)
    {
        AbilityRequestReceiver.instance.CheckPredicates(NetworkObjectId, id);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        foreach(var ability in abilities)
            ability.OnDestroy();
    }
}
