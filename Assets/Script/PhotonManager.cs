using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Photon.Pun.Demo.Cockpit;
using UnityEngine.WSA;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField userNameText;
    public TMP_InputField roomNameText;
    public TMP_InputField MaxPlayer;

    public GameObject PlayerNamePanel;
    public GameObject LobbyPanel;
    public GameObject RoomCreatePanel;
    public GameObject ConnectingPanel;
    public GameObject RoomlistPanel;
   

    #region UnityMehtods
    // Start is called before the first frame update
    void Start()
    {
        ActiiveMyPanel(PlayerNamePanel.name);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Network state :"+PhotonNetwork.NetworkClientState);
    }
    #endregion

    #region UiMethds
    public void OnLoginClick()
    {
        string name = userNameText.text;
        if(!string.IsNullOrEmpty(name))
        {
            PhotonNetwork.LocalPlayer.NickName = name;
            PhotonNetwork.ConnectUsingSettings();
            ActiiveMyPanel(ConnectingPanel.name);
        }
        else
        {
            Debug.Log("name");
        }
    }

    public void OnClickRoomCreate()
    {
       string roomName = roomNameText.text; 
        if (!string.IsNullOrEmpty(roomName)) 
        {
            roomName = roomName + Random.Range(0, 1000);                
        }
        RoomOptions roomOptions = new RoomOptions();
        RoomOptions.MaxPlayer=(byte)int.Parse(MaxPlayer.text);
        PhotonNetwork.CreateRoom(roomName,roomOptions);
    }

    public void OnCancelClick()
    {
        ActiiveMyPanel(LobbyPanel.name);
    }
    public void RoomListbtnClicked()
    {
        if (!PhotonNetwork.InLobby) ;
        {
            PhotonNetwork.JoinLobby();
            
        }
        ActiiveMyPanel(RoomlistPanel.name) ;
    }
    #endregion


    #region PHOTON_CALLBACKS


    public override void OnConnected()
    {
        Debug.Log("conected to internet");

    }
    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        ActiiveMyPanel(LobbyPanel.name);

    }
    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + "Is created");
    }


    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + "Room Joined");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomInfo rooms in roomList)
        {
            Debug.Log("RoomName :" + rooms.Name);
        }
    }
    #endregion



    #region Public_Methods

    public void ActiiveMyPanel(string PanelName)
    {
        LobbyPanel.SetActive(PanelName.Equals(LobbyPanel.name));
        PlayerNamePanel.SetActive(PanelName.Equals(PlayerNamePanel.name));
        RoomCreatePanel.SetActive(PanelName.Equals(RoomCreatePanel.name));
        ConnectingPanel.SetActive(PanelName.Equals(ConnectingPanel.name));
        RoomlistPanel.SetActive(PanelName.Equals(RoomlistPanel.name));
    }

    #endregion

}