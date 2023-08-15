using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEditor.Experimental.GraphView;
//using Photon.Pun.Demo.Cockpit;
//using UnityEngine.WSA;

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

    private Dictionary<string, RoomInfo> roomListData;
    public GameObject roomListPrefab;
    public GameObject roomListParent;

    private Dictionary<string,GameObject> roomListGameObject;

    #region UnityMehtods
    // Start is called before the first frame update
    void Start()
    {
        ActiiveMyPanel(PlayerNamePanel.name);
        roomListData = new Dictionary<string, RoomInfo>();
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
            if (string.IsNullOrEmpty(roomName))
            {
              roomName = roomName + Random.Range(0, 1000);
            }
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = (byte)int.Parse(MaxPlayer.text);

           /* if (int.TryParse(MaxPlayer.text, out int maxPlayers))
            {
                roomOptions.MaxPlayers = (byte)maxPlayers;
            }
            else
            {
                Debug.LogError("Invalid max players value!");
                return;
            }*/

            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }


    public void OnCancelClick()

    {
        ActiiveMyPanel(LobbyPanel.name);
    }
    public void RoomListbtnClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
            
        }
        ActiiveMyPanel(RoomlistPanel.name);
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
        // if (PhotonNetwork.CurrentRoom != null)
        // {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + "Is created!");
      //  }
       /* else
        {
            Debug.LogError("PhotonNetwork.CurrentRoom is null!");
        }*/
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
            if(rooms.IsOpen|| rooms.IsVisible||rooms.RemovedFromList)
            {
                if(roomListData.ContainsKey(rooms.Name))
                {
                    roomListData.Remove(rooms.Name);
                }
                else{
                    if (roomListData.ContainsKey(rooms.Name))
                    {
                        roomListData[rooms.Name] = rooms;
                      
                    }
                    else
                    {
                        roomListData.Add(rooms.Name, rooms);
                    }
                }
            }
            // Generate List Item
            foreach(RoomInfo roomItem in roomListData.Values)
            {
                GameObject roomListItemObject = Instantiate(roomListPrefab);
                roomListItemObject.transform.SetParent(roomListParent.transform);
                roomListItemObject.transform.localScale = Vector3.one;
                //room name player number Button room join

                roomListItemObject.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = roomItem.Name;
                roomListItemObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = roomItem.PlayerCount +"/"+roomItem.MaxPlayers;
                roomListItemObject.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => RoomJoinFromList(roomItem.Name));
                roomListGameObject.Add(roomItem.Name,roomListItemObject);

            }
        }
    }
    #endregion



    #region Public_Methods

    public void RoomJoinFromList(string roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(roomName);
    }

    public void ClearRoomList()
    {
        if(roomListGameObject.Count>0)
        foreach(var v in roomListGameObject.Values)
        {
            Destroy(v);
        }
    }

    public void ActiiveMyPanel(string PanelName)
    {
        LobbyPanel.SetActive(PanelName.Equals(LobbyPanel.name));
        PlayerNamePanel.SetActive(PanelName.Equals(PlayerNamePanel.name));
        RoomCreatePanel.SetActive(PanelName.Equals(RoomCreatePanel.name));
        ConnectingPanel.SetActive(PanelName.Equals(ConnectingPanel.name));
        RoomlistPanel.SetActive(PanelName.Equals(RoomlistPanel.name));

        Debug.Log("everthing is called");
    }

    #endregion

}