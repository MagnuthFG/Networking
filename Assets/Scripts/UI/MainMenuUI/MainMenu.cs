using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{

    private UIDocument _uIDocument;
    private VisualElement _rootElement;

    private Button _hostGameButton;
    private Button _joiGameButton;
    private Button _lobbiesButton;
    private Button _searchGameButton;

    private TextField _joinCodeField;

    //SearchGameButton
    [SerializeField] private GameObject lobbyGameObjectUI;
    private void Awake()
    {
        _uIDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        if (_uIDocument == null) return;
        _rootElement = _uIDocument.rootVisualElement;
        _hostGameButton = _rootElement.Q<Button>("HostGameButton");
        _joiGameButton = _rootElement.Q<Button>("ClientJoinButton");
        _joinCodeField = _rootElement.Q<TextField>("JoinCodeField");
        _lobbiesButton = _rootElement.Q<Button>("LobbiesButton");
        _searchGameButton = _rootElement.Q<Button>("SearchGameButton");

        _hostGameButton.clicked += HostGameAsync;
        _joiGameButton.clicked += JoinGameAsync;
        _lobbiesButton.clicked += OpenLobbiesView;
        _searchGameButton.clicked += MatchMakingSearch;

    }
    private void OnDisable()
    {
        _hostGameButton.clicked -= HostGameAsync;
        _joiGameButton.clicked -= JoinGameAsync;
        _lobbiesButton.clicked -= OpenLobbiesView;
        _searchGameButton.clicked -= MatchMakingSearch;
    }

    private void MatchMakingSearch()
    {
        ClientSingelton.GetInstance().StartMatchMaking();
    }


    private async void JoinGameAsync()
    {
        await ClientSingelton.GetInstance().StartClientAsync(_joinCodeField.text);
    }

    private async void HostGameAsync()
    {
        await HostSingelton.GetInstance().StartHost();
    }

    private void OpenLobbiesView()
    {
        lobbyGameObjectUI.SetActive(true);
        gameObject.SetActive(false);
    }




}
