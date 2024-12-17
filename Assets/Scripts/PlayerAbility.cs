using System.Collections.Generic;
using Unity.Netcode;
using ImprovedTimers;
using UnityEngine;

public class PlayerAbility : NetworkBehaviour, IUsingTimer
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

    public void Use(Timer timer) // ??? Завтра доделать обязательно, а то у меня уже плывет мозг
    {
        Timer sameTimer = abilities.Find(x => x.CooldownTimer == timer).CooldownTimer;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        if (abilities == null) return;
        foreach (var ability in abilities)
            ability.OnDestroy();
    }
}


public interface IUsingTimer
{
    void Use(Timer timer);
}