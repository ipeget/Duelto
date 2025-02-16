using System;
using UnityEngine;

[Serializable]
public class ProjectileAbility : Ability, INetworkAbilityWithInstantiate
{
    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;

    public override void Execute()
    {
        int id = AbilityRequestReceiver.instance.GetPrefabId(this);
        AbilityRequestReceiver.instance.RequestInstantiateRpc(id);
    }
}
