using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Unity.Entities;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerSample;

    public List<Transform> SpawnPoint;
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = false
        };
        PhotonNetwork.JoinOrCreateRoom("Test", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        var id = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.Log("Joiner Room with " + PhotonNetwork.CurrentRoom.PlayerCount + " player " + id);

        if (id > (SpawnPoint.Count + 1))
        {
            Debug.LogError("NO SPAWN POINT");
        }
        else
        {
            var newNetworkPlayerPrefab = PhotonNetwork.Instantiate(PlayerSample.name, new Vector3(0, 0.5f, 0), Quaternion.identity);
            var networkManagerSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<NetworkManagerSystem>();
            networkManagerSystem.ObjectPlayerScene = newNetworkPlayerPrefab;
        }
    }
}
