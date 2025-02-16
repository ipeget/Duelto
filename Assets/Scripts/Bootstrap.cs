using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private AbilityRequestReceiver abilityRequestReceiver;
    [SerializeField] private TimerHandler timerHandler;
    [SerializeField] private CinemachineCamera ccam;
    [SerializeField] private TargetingHandler targetingHandler;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
    }

    private void Update() //
    {
        if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            StartGame();
        }
    }

    private void ClientConnected(ulong id)
    {
        if (!NetworkManager.Singleton.IsListening) return;

        var connected = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);

        if (connected.IsOwner)
        {
            ccam.Target.TrackingTarget = connected.transform.Find("PlayerCameraRoot");
        }

        if (connected.TryGetComponent<PlayerAbility>(out var playerAbility))
        {
            playerAbility.Initialize();
        }
        else throw new MissingComponentException("Player object does not have PlayerAbility");

        timerHandler.Initialize();
    }

    private void StartGame()
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            var playerTargeting = client.Value.PlayerObject.gameObject.GetComponent<PlayerTargeting>();
            if (playerTargeting == null)
            {
                Debug.Log($"PlayerTargeting not found. Client {client.Key} is not registered in TargetingHandler");
                continue;
            }
            targetingHandler.RegisterPlayer(client.Key, playerTargeting);
        }
        enabled = false;
    }
}
