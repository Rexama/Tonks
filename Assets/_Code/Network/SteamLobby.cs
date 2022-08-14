using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using TMPro;
using UnityEngine.UI;

public class SteamLobby : MonoBehaviour
{
    //Callbacks
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequested;
    protected Callback<LobbyEnter_t> LobbyEntered;

    //Variables
    public ulong CurrentLobbyID;
    private const string HostAddressKey = "HostAddress";
    private CustomNetworkManager manager;

    //Gameobject
    public GameObject HostButton;
    public TextMeshProUGUI LobbyTextName;

    private void Start()
    {
        if(!SteamManager.Initialized) return;
        
        manager = GetComponent<CustomNetworkManager>();
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequested);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, manager.maxConnections);
        
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.LogError("Lobby creation failed: " + callback.m_eResult);
            return;
        }

        Debug.Log("Lobby created, ID: " + callback.m_ulSteamIDLobby);

        manager.StartHost();
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey,
            SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name",
            SteamFriends.GetPersonaName() + "'s Lobby");
    }

    private void OnJoinRequested(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Requested to join lobby: " + callback.m_steamIDLobby);
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        //Everyone
        HostButton.SetActive(false);
        CurrentLobbyID = callback.m_ulSteamIDLobby;
        LobbyTextName.gameObject.SetActive(true);
        LobbyTextName.text = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "name");

        //Clients
        if (NetworkServer.active) return;

        manager.networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), HostAddressKey);
        manager.StartClient();
    }
}