using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAbility : NetworkBehaviour
{
    [SerializeReference, SubclassSelector] private List<Ability> abilities;

    private void Start()
    {
        if (IsOwner)
        {
            Debug.Log("PlayerAbility Initialize");
            foreach (var ability in abilities)
                ability.Initialize();
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        foreach(var ability in abilities)
            ability.OnDestroy();
    }
}
