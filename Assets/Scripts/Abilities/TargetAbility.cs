using UnityEngine;

public class TargetAbility : Ability, INetworkAbilityWithInstantiate, IUsageUpdate
{
    [SerializeField] private GameObject prefab;
    public GameObject Prefab => prefab;

    [SerializeField] private ITargetingStrategy targetingStrategy;
    private bool targetFound;

    public override void Execute()
    {
        if (!targetFound) return;

        targetingStrategy.OnUse();
    }

    public void Update()
    {
        targetFound = targetingStrategy.OnTargeting();
    }
}
