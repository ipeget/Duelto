using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class TargetingHandler : NetworkBehaviour
{
    private List<Client> registeredPlayers = new List<Client>();

    [SerializeField] private LayerMask targetLayer;

    public void RegisterPlayer(ulong id, PlayerTargeting player)
    {
        player.targetingHandler = this;

        Transform main = player.transform;

        Transform dotTarget = player.transform.Find("DotTarget");
        if (dotTarget == null)
        {
            Debug.LogError("DotTarget child not found. Player is not registered");
            return;
        }

        Transform up = dotTarget.Find("Up");
        Transform down = dotTarget.Find("Down");

        if (!player.IsOwner)
        {
            up.gameObject.layer = 6;
            down.gameObject.layer = 6;
        }

        registeredPlayers.Add(new Client(id, player, main, up, down));
    }

    [Rpc(SendTo.Server)]
    public void RequestTargetRpc(ulong id, Vector3 origin, Vector3 forward, float distance)
    {
        if (!IsServer) return;

        Physics.Raycast(origin, forward, out var hitInfo, distance, targetLayer);

        long targetId = -1;
        int valueIndex = -1;

        foreach (var client in registeredPlayers)
        {
            if (client.id == id) continue;
            for (int i = 0; i < client.targets.Length; i++)
            {
                if (client.targets[i] != hitInfo.transform) continue;

                valueIndex = i;
                targetId = (long)client.id;
                break;
            }
        }

        
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { id }
            }
        };

        PassTargetClientRpc(id, targetId, valueIndex, clientRpcParams);
    }

    [ClientRpc]
    private void PassTargetClientRpc(ulong id, long targetId, int valueIndex, ClientRpcParams clientRpcParams = default)
    {
        PlayerTargeting playerTargeting = (from client in registeredPlayers where client.id == id select client).
            First().playerTargeting;

        if (targetId == -1 && valueIndex == -1)
        {
            playerTargeting.TakeTarget(null);
            return;
        }

        Transform target = (from t in registeredPlayers where t.id == (ulong)targetId select t).First().targets[valueIndex];
        playerTargeting.TakeTarget(target);
    }


    private class Client
    {
        public ulong id { get; private set; }
        public Transform[] targets { get; private set; }
        public PlayerTargeting playerTargeting { get; private set; }

        public Client(ulong id, PlayerTargeting playerTargeting, params Transform[] targets)
        {
            this.id = id;
            this.targets = targets;
            this.playerTargeting = playerTargeting;
        }
    }
}
