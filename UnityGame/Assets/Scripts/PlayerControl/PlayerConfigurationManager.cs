using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;
    //private List<int> playerClasses

    public static PlayerConfigurationManager Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null){
            Debug.Log("This is a singleton - Trying to create another instance of singleton");
        }
        else{
            Instance = this;
            DontDestroyOnLoadManager.DontDestroyOnLoad(this.gameObject);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigs()
    {
        return playerConfigs;
    }

    public void SetPlayerClass(int index, int classType)
    {
        // 0 = empty, 1 = builder, 2 = shock, 3 = healer aura, 4 = healer shot
        playerConfigs[index].PlayerClass = classType;
    }

    public int GetPlayerClass(int index)
    {
        // 0 = empty, 1 = builder, 2 = shock, 3 = healer aura, 4 = healer shot
        return playerConfigs[index].PlayerClass;
    }

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
        if (playerConfigs.Count >= 1 && playerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("PVPArena1");
            //SceneManager.LoadScene("Game");
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

    public int PlayerClass {get; set;}
}
