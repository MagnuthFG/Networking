using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        bool isGraphicCardDoesntExist = (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
        StartInMode(isGraphicCardDoesntExist);
    }

    async void StartInMode(bool isGraphicsDoesntExist){
        if(isGraphicsDoesntExist){
            await ServerSingelton.GetInstance().InitServerAsync();
            await ServerSingelton.GetInstance().StartServerAsync();
        }else{
            await HostSingelton.GetInstance().InitSeverAsync();
            await ClientSingelton.GetInstance().InitClientAsync();

            if(ClientSingelton.GetInstance().isAuth){
                SceneManager.LoadScene("MainMenuScene");
            }
                    // If everything init then go to main menu 
        }
    }
}
