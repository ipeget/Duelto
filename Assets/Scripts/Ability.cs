using ImprovedTimers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public abstract class Ability : IAbilityPredicates
{
    [SerializeField] private Metadata metadata;
    [Space]
    public InputAction inputAction;
    [SerializeField] private AnimationClip animationClip;
    [SerializeField] private AudioClip audioClip;

    [Space]
    private List<Func<bool>> predicates;
    public List<Func<bool>> Predicates => predicates;
    [SerializeField] private float cooldown;
    [Space]
    [SerializeField] private int priority;
    public int Id => GetHashCode();

    protected Timer cooldownTimer;
    
    public abstract void Execute();

    public virtual void Initialize() 
    {
        cooldownTimer = new CountdownTimer(cooldown);
        OnEnable();
    }

    public virtual void OnEnable() => inputAction.Enable();
    public virtual void OnDisable() => inputAction.Disable();
    public virtual void OnDestroy() => cooldownTimer.Dispose();

    public override int GetHashCode()
    {
        return metadata.Name.GetHashCode();
    }
}

[Serializable]
public class ProjectileAbility : Ability, INetworkAbilityWithInstantiate
{
    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;

    public override void Execute()
    {
        int id = AbilityRequestReceiver.instance.GetAbilityId(this);

        if(id == -1) return;

        if (cooldownTimer.IsRunning) return;

        AbilityRequestReceiver.instance.RequestInstantiateRpc(id);
        cooldownTimer.Start();
    }
}

public interface INetworkAbilityWithInstantiate
{
    GameObject Prefab { get; }
}

public interface IAbilityPredicates
{
    List<Func<bool>> Predicates { get; }

    int Id { get; }
}
