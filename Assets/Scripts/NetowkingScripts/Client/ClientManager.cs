using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

using UnityEngine;
using static FGAuthentication;

public class ClientManager : ScriptableObject
{
    JoinAllocation allocation;
    private MatchplayMatchmaker matchplayMatchmaker;

    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();
        AuthState currentState = await FGAuthentication.FGAuthen(5);
        bool isAuth = (currentState == AuthState.Authorized);
        matchplayMatchmaker = new MatchplayMatchmaker();
        ClientDisconnect clientDisconnect = new ClientDisconnect(NetworkManager.Singleton);
        // Im gonna call client disconnect over here 
        return isAuth;
    }


private async Task StartClientAsync(String ip, int port)
    {
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        if (transport == null) return;
        transport.SetConnectionData(ip, (ushort)port);
        NetworkManager.Singleton.StartClient();
        await Task.Delay(0);
    }

public async void StartMatchMaking(){
    NetworkManager.Singleton.NetworkConfig.ConnectionData = UserDataWrapper.PayLoadInBytes();
    MatchmakingResult matchmakingResult = await matchplayMatchmaker.Matchmake(UserDataWrapper.GetUserData());
    if(matchmakingResult.result == MatchmakerPollingResult.Success){
        await StartClientAsync(matchmakingResult.ip, matchmakingResult.port);
    }

}



    public async Task StartClientAsync(String joinCode)
    {
        allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        if (transport == null) return;
        RelayServerData relayServerData = new RelayServerData(allocation, "udp");   // dtls
        transport.SetRelayServerData(relayServerData);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = UserDataWrapper.PayLoadInBytes();

        NetworkManager.Singleton.StartClient();
    }


}
