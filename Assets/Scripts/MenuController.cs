using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private string VersionName = "0.1";

    [SerializeField] private GameObject UsernameMenu;
    [SerializeField] private GameObject ConnectPanel;

    [SerializeField] private InputField UsernameInput;
    [SerializeField] private TextMeshProUGUI CreateGameInput;
    [SerializeField] private TextMeshProUGUI JoinGameInput;

    public GameObject LobbyDisplay;
    public int connections;
    public TextMeshProUGUI count;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

    public void CreateGame()
    {
        PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, roomOptions, TypedLobby.Default);
    }

    private void OnJoinedRoom()
    {
        LobbyDisplay.SetActive(true);

        connections = PhotonNetwork.CountOfPlayers;
        count.text = connections.ToString() + "/4";

        if (connections > 0) { LobbyDisplay.transform.GetChild(2).gameObject.SetActive(true); }
        if (connections > 1) { LobbyDisplay.transform.GetChild(3).gameObject.SetActive(true); }
        if (connections > 2) { LobbyDisplay.transform.GetChild(4).gameObject.SetActive(true); }
        if (connections > 3) { LobbyDisplay.transform.GetChild(5).gameObject.SetActive(true); }
    }

    private void OnConnected()
    {
    }
}
