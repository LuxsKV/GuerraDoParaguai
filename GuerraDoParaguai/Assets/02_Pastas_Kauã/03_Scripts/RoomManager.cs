using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;

    public GameObject player;
    [Space]
    public Transform[] spawnPoints;
    public Transform[] spawnPointsTeamA;
    public Transform[] spawnPointsTeamB;

    [Space]
    public GameObject roomCam;

    [Space]
    public GameObject nameUI;
    public GameObject connectingUI;

    private string nickname = "unnamed";

    public string roomNameToJoin = "test";

    [HideInInspector]
    public int kills = 0;
    [HideInInspector]
    public int deaths = 0;

    [Header("Timer")]
    public GameObject timer;

    public enum Team
    {
        None,
        TeamA,
        TeamB
    }

    private Team playerTeam = Team.None;

    void Awake()
    {
        instance = this;
    }

    public void ChangeNickname(string _name)
    {
        nickname = _name;
    }

    public void JoinRoomButtonPressed()
    {
        Debug.Log("Connecting...");

        PhotonNetwork.JoinOrCreateRoom(roomNameToJoin, null, null);

        nameUI.SetActive(false);
        connectingUI.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("We're connected and in a room now");

        roomCam.SetActive(false);

        AssignTeam(); // Assign team to the player
        SpawnPlayer();
    }

    private void AssignTeam()
    {
        // Alternar entre os times A e B
        playerTeam = (Team)(((int)playerTeam + 1) % 2);
    }

    public void SpawnPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            timer.SetActive(true);
        }

        // Selecionar o ponto de spawn com base no time
        Transform spawnPoint = GetSpawnPointForTeam(playerTeam);

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);

        _player.GetComponent<PlayerSetup>().IsLocalPlayer();
        _player.GetComponent<Health>().isLocalPlayer = true;

        _player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, nickname);
        PhotonNetwork.LocalPlayer.NickName = nickname;

        SetHashes();
    }

    
    private Transform GetSpawnPointForTeam(Team team)
    {
        if (team == Team.TeamA)
            return spawnPointsTeamA[Random.Range(0, spawnPointsTeamA.Length)];
        else if (team == Team.TeamB)
            return spawnPointsTeamB[Random.Range(0, spawnPointsTeamB.Length)];
        else
            return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    public void SetHashes()
    {
        try
        {
            Hashtable hash = PhotonNetwork.LocalPlayer.CustomProperties;
            hash["kills"] = kills;
            hash["deaths"] = deaths;
            hash["team"] = (int)playerTeam;

            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        catch
        {
            // Tratar exceções, se necessário
        }
    }
}