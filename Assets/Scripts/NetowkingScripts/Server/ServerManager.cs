using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using UnityEngine;

public class ServerManager : ScriptableObject, IDisposable
{

    NetworkManager networkManager;
    NetworkServer networkServer;

    public MultiplayAllocationService multiplayAllocationService;

    String ip;
    int port;
    int qPort;

    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();
        return true;
    }

    public void ServerManagerConfiguration(NetworkManager networkManager, String ip, int port, int qPort)
    {
        this.networkManager = networkManager;
        this.networkServer = new NetworkServer(networkManager);
        multiplayAllocationService = new MultiplayAllocationService();
        this.ip = ip;
        this.port = port;
        this.qPort = qPort;
    }


    private bool OpenConnection()
    {

        UnityTransport unityTransport = networkManager.gameObject.GetComponent<UnityTransport>();
        unityTransport.SetConnectionData(ip, (ushort)port);
        return true;
    }

    public async Task StartServerAsync()
    {
        await multiplayAllocationService.BeginServerCheck();
        if (!OpenConnection())
        {
            Debug.LogError("Its is not an error ! Because our code should not work as i did not finish coding the multiplayer pre");
        }

    }

    public void Dispose()
    {
        multiplayAllocationService?.Dispose();
        networkServer?.Dispose();
    }
}
