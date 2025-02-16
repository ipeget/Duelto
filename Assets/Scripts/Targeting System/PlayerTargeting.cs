using UnityEngine;
using Unity.Netcode;

public class PlayerTargeting : NetworkBehaviour
{
    public TargetingHandler targetingHandler;

    private Transform mainCamera;
    private Transform currentTarget;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            mainCamera = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            RequestTarget();
            string name = currentTarget ? currentTarget.name : "null";
            Debug.Log(name);
        }
    }

    private void RequestTarget()
    {
        if (IsOwner)
            targetingHandler.RequestTargetRpc(OwnerClientId, mainCamera.position, mainCamera.forward, 100);
    }

    public void TakeTarget(Transform target)
    {
        currentTarget = target;
    }
}
