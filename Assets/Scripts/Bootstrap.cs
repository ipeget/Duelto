using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private AbilityRequestReceiver abilityRequestReceiver;
    [SerializeField] private CinemachineCamera ccam;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
    }

    private void ClientConnected(ulong id)
    {
        if (!NetworkManager.Singleton.IsListening) return;

        var connected = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);

        if (connected.IsOwner)
        {
            ccam.Target.TrackingTarget = connected.transform.Find("PlayerCameraRoot");
            //connected.GetComponent<ClientConnection>()?.Initialize();
        }

        if (NetworkManager.Singleton.IsHost)
        {
            PlayerAbility playerAbility = connected.GetComponent<PlayerAbility>();
            if (playerAbility != null)
            {
                abilityRequestReceiver.AddClientAbilityPredicates(id, playerAbility.playerAbilityConfig.GetAbilityPredicates());
            }
            else
            {
                throw new MissingComponentException("Player object does not have PlayerAbility");
            }
        }
    }
}
