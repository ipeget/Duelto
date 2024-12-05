using Unity.Netcode;
using UnityEngine;

public class PlayerParametrs : NetworkBehaviour, IDamageable
{
    [SerializeField] private NetworkVariable<int> health = new(3000, writePerm: NetworkVariableWritePermission.Owner);
    [SerializeField] private NetworkVariable<int> gfenergy = new(1000, writePerm: NetworkVariableWritePermission.Owner);
    public override void OnNetworkSpawn()
    {
        health.OnValueChanged += OnValueChanged;
    }

    public override void OnNetworkDespawn()
    {
        health.OnValueChanged -= OnValueChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            TakeDamage(10);
    }

    private void OnValueChanged(int previous, int current)
    {
        Debug.Log($"Value changed from {previous} to {current}");
    }

    public void TakeDamage(int damage)
    {
        health.Value -= damage;
    }
}


public interface IDamageable
{
    void TakeDamage(int damage);
}
