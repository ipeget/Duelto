using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Rendering;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    //private void Update()
    //{
    //    if (NetworkManager.Singleton.IsListening)
    //    {
    //        if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
    //        {
    //            foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
    //                NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerController>().UpdateRpc();
    //        }
    //        else
    //        {
    //            var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
    //            var player = playerObject.GetComponent<PlayerController>();
    //            player.UpdateRpc();
    //        }
    //    }
    //}

    //private void LateUpdate()
    //{
    //    if (NetworkManager.Singleton.IsListening)
    //    {
    //        if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
    //        {
    //            foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
    //                NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<PlayerController>().LateUpdateRpc();
    //        }
    //        else
    //        {
    //            var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
    //            var player = playerObject.GetComponent<PlayerController>();
    //            player.LateUpdateRpc();
    //        }
    //    }
    //}

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 400));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }

        GUILayout.EndArea();


    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }


    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private CinemachineVirtualCamera vcam;
    //private void OnEnable()
    //{
    //    networkManager.OnClientConnectedCallback += SetCamera;
    //}
    //private void SetCamera(ulong id)
    //{
    //    IEnumerator Delay()
    //    {
    //        yield return new WaitForSeconds(0.06f);
    //        if (networkManager.IsListening)
    //        {
    //            IReadOnlyDictionary<ulong, NetworkClient> temp = networkManager.ConnectedClients;
    //            vcam.Follow = temp[id].PlayerObject.transform.GetChild(0);
    //        }
    //    }

    //    StartCoroutine(Delay());
    //}
}
