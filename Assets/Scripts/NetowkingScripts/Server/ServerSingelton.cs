using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

public class ServerSingelton : Singleton<ServerSingelton>
{
    public ServerManager serverManager;
    public bool isAuth = false;


    public async Task InitServerAsync(){
        serverManager = ScriptableObject.CreateInstance<ServerManager>();
        serverManager.ServerManagerConfiguration(NetworkManager.Singleton, ApplicationData.IP(), ApplicationData.Port(), ApplicationData.QPort());
        isAuth = await serverManager.InitAsync();
    }

    public async Task StartServerAsync(){
        await serverManager.StartServerAsync();
    }
    
}
