using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerConfigurationManager : MonoBehaviour
{
    const string PVPArena1 = "PVPArena1";
    const string PVPArena2 = "PVPArena2-Fissure";
    const string PVPArena3 = "PVPArena3-Colosseum";
    const string TestArena = "Game";
    const string CampaignTutorial = "Campaign-Tutorial";
    const string LevelOne = "Campaign-Level-One";
    const string LevelTwo = "Campaign-Level-Two";
    private List<PlayerConfiguration> playerConfigs;
    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("This is a singleton - Trying to create another instance of singleton");
        }
        else
        {
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
        // 5 = BubbleShield
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
            //SceneManager.LoadScene("PVPArena1");
            GameObject gameDetailsObject = GameObject.Find("GameDetails");
            //GameObject g = GameObject.FindWithTag("ArenaGameDetailsObject");
            if (gameDetailsObject.CompareTag("ArenaGameDetailsObject"))
            {
                // arena game
                ArenaGameDetails arenaGameDetailsScript = gameDetailsObject.GetComponent<ArenaGameDetails>();
                if ((MainMenu.Maps)arenaGameDetailsScript.mapName == MainMenu.Maps.Fissure)
                {
                    // if it is the fissure map
                    SceneManager.LoadScene(PVPArena2);

                }
                else if ((MainMenu.Maps)arenaGameDetailsScript.mapName == MainMenu.Maps.arena1)
                {
                    SceneManager.LoadScene(PVPArena1);
                }
                else if ((MainMenu.Maps)arenaGameDetailsScript.mapName == MainMenu.Maps.Colosseum)
                {
                    SceneManager.LoadScene(PVPArena3);
                }
                else
                {
                    SceneManager.LoadScene(PVPArena2);
                }
            }
            else
            {
                //campaign game

                CampaignGameDetails campaignGameDetails = gameDetailsObject.GetComponent<CampaignGameDetails>(); 
                Debug.Log(campaignGameDetails);
                Debug.Log(campaignGameDetails.level.ToString());
                if((MainMenu.Levels)campaignGameDetails.level == MainMenu.Levels.level1){
                    SceneManager.LoadScene(LevelOne);
                }
                else if((MainMenu.Levels)campaignGameDetails.level == MainMenu.Levels.level2){
                    SceneManager.LoadScene(LevelTwo);
                }
                else {
                    SceneManager.LoadScene(CampaignTutorial);
                }
                
            }   
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player " + pi.playerIndex + " has joined the game.");
        if (!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
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
    public PlayerInput Input { get; set; }

    public int PlayerIndex { get; set; }

    public bool IsReady { get; set; }

    public int PlayerClass { get; set; }
}