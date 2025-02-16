using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAbility : NetworkBehaviour
{
    [SerializeField] private PlayerAbilityConfig playerAbilityConfig;
    private List<Ability> abilities;
    public IEnumerable<Ability> Abilities => abilities;

    public void Initialize()
    {
        abilities = playerAbilityConfig.GetAbilities();

        if (IsOwner)
        {
            foreach (var ability in abilities)
            {
                ability.Initialize();
                ability.inputAction.performed += _ => TryExecuteAbilityRPC(ability.Id);
            }
        }

        AbilityRequestReceiver.instance.AddClientAbilityPredicates(OwnerClientId, playerAbilityConfig.GetAbilityPredicates());
    }

    [Rpc(SendTo.Server)]
    public void TryExecuteAbilityRPC(int id)
    {
        AbilityRequestReceiver.instance.TryExecuteAbility(OwnerClientId, id);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (abilities == null) return;
        foreach (var ability in abilities)
            ability.OnDestroy();
    }
}