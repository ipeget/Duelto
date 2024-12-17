using ImprovedTimers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public abstract class Ability : IAbility
{
    [SerializeField] private Metadata metadata;
    [Space]
    public InputAction inputAction;
    [SerializeField] private AnimationClip animationClip;
    [SerializeField] private AudioClip audioClip;

    [Space]
    protected List<Func<bool>> predicates;
    public List<Func<bool>> Predicates => predicates;
    [SerializeField] private float cooldown;
    [Space]
    [SerializeField] private int priority;
    public int Id => GetHashCode();

    public Timer CooldownTimer { get; private set; }

    public abstract void Execute();

    public virtual void Initialize()
    {
        CooldownTimer = new CountdownTimer(cooldown);
        predicates = new List<Func<bool>>() { () => !CooldownTimer.IsRunning };
        OnEnable();
    }

    public virtual void OnEnable() => inputAction.Enable();
    public virtual void OnDisable() => inputAction.Disable();
    public virtual void OnDestroy() => CooldownTimer.Dispose();
    public override int GetHashCode() => metadata.Name.GetHashCode();
}

[Serializable]
public class ProjectileAbility : Ability, INetworkAbilityWithInstantiate
{
    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;

    public override void Execute()
    {
        int id = AbilityRequestReceiver.instance.GetAbilityId(this);
        AbilityRequestReceiver.instance.RequestInstantiateRpc(id);
    }
}

public interface INetworkAbilityWithInstantiate
{
    GameObject Prefab { get; }
}

public interface IAbility
{
    List<Func<bool>> Predicates { get; }
    Timer CooldownTimer { get; }
    void Execute();
    int Id { get; }
}
