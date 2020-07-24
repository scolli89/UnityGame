using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

    public static PlayerConfigurationManager Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null){
            Debug.Log("This is a singleton - Trying to create another instance of singleton");
        }
        else{
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void SetPlayerClass(int index, GameObject classType)
    {
        playerConfigs[index].PlayerClass = classType;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
        if (playerConfigs.Count >= 1 && playerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player " + pi.playerIndex + " has joined the game.");
        if(!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex)){
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi));
        }
    }
}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
    }
    public PlayerInput Input {get; set;}

    public int PlayerIndex {get; set;}

    public bool IsReady {get; set;}

    public GameObject PlayerClass {get; set;}
}
