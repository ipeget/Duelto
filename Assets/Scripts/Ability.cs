using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public abstract class Ability
{
    [SerializeField] private Metadata metadata;
    [Space]
    [SerializeField] private InputAction inputAction;
    [SerializeField] private AnimationClip animationClip;
    [SerializeField] private AudioClip audioClip;
    [Space]
    public List<Func<bool>> Predicates;
    [SerializeField] private float cooldown;
    [Space]
    [SerializeField] private int priority;

    public abstract void Execute();

    public virtual void Initialize() 
    {
        inputAction.performed += _ => Execute();
        OnEnable();
    }

    public virtual void OnEnable() => inputAction.Enable();
    public virtual void OnDisable() => inputAction.Disable();
}

[Serializable]
public class ProjectileAbility : Ability, INetworkAbilityWithInstantiate
{
    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;

    public override void Execute()
    {
        int id = AbilityRequestReceiver.instance.GetAbilityId(this);
        if(id != -1)
            AbilityRequestReceiver.instance.RequestInstantiateRpc(id);
    }
}

public interface INetworkAbilityWithInstantiate
{
    GameObject Prefab { get; }
}
