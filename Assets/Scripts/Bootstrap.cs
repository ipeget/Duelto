using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
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
    }
}
