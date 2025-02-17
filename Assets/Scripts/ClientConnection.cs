using Unity.Netcode;
using UnityEngine;

public class ClientConnection : NetworkBehaviour
{
    [SerializeField] private MonoBehaviour[] ownerBehaviours;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private CharacterController controller;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        foreach (var behaviour in ownerBehaviours)
        {
            behaviour.enabled = false;
        }

        controller.enabled = false;
        capsuleCollider.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        enabled = IsClient;
        if (!IsOwner)
        {
            enabled = false;
            capsuleCollider.enabled = true;
            gameObject.tag = "Opponent";
            return;
        }

        gameObject.layer = 6;
        controller.enabled = true;

        foreach (var behaviour in ownerBehaviours)
        {
            behaviour.enabled = true;
        }
    }
}
