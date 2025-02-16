using Unity.Netcode;
using ImprovedTimers;
using System.Collections.Generic;
using UnityEngine;

public class TimerHandler : NetworkBehaviour
{
    public static TimerHandler instance;
    private List<ClientTimer> timers = new();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        if (IsHost)
        {
            foreach (var timer in timers)
                timer.timer.Tick();
        }
    }

    public void Initialize()
    {
        if (!IsServer) return;

        timers.Clear();

        var clientsId = NetworkManager.Singleton.SpawnManager.GetConnectedPlayers();
        foreach (var id in clientsId)
        {
            var connected = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(id);
            if (connected.TryGetComponent<PlayerAbility>(out var playerAbility))
            {
                foreach (var ability in playerAbility.Abilities)
                {
                    RegisterTimer(id, ability.CooldownTimer);
                }
            }
            else throw new MissingComponentException("Player object does not have PlayerAbility");
        }
    }

    public Timer CreateTimer(ulong clientIdm, float time)
    {
        Timer timer = new CountdownTimer(time);
        RegisterTimer(clientIdm, timer);
        return timer;
    }

    public void RegisterTimer(ulong clientId, Timer timer)
    {
        ClientTimer clientTimer = new ClientTimer(clientId, timer);
        timers.Add(clientTimer);
    }

    public void StartTimer(ulong clientId, Timer timer)
    {
        if(!IsServer) return;

        ClientTimer cashedClientTimer = timers.Find(x => x.timerHashCode == timer.GetHashCode() && x.clientId == clientId);
        if (cashedClientTimer == null)
            Debug.LogError("The requested timer is not registered");

        if (cashedClientTimer.timer.IsFinished)
            cashedClientTimer.timer.Start();
        else
            Debug.LogError("Attempt to start a running timer");
    }

    private class ClientTimer
    {
        public ulong clientId { get; private set; }
        public int timerHashCode { get; private set; }
        public Timer timer { get; private set; }

        public ClientTimer(ulong clientId, Timer timer)
        {
            this.clientId = clientId;
            this.timer = timer;
            timerHashCode = timer.GetHashCode();
        }
    }
}
