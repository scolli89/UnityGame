using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampaignGameLogic : MonoBehaviour
{
    // [SerializeField]
    // private GameObject[] spawnPoints;

    //private GameObject spawnPointPlacements;
    const string MainMenuScene = "Menu";
    [SerializeField]

    private GameObject[] playerPrefabs;
    [SerializeField]
    public GameObject[] trailDots;

    public GameObject ScoreBoardCanvas;
    public TextMeshProUGUI scoreBoardText;



    private int numSpawnPoints;
    public float respawnDelay = 3.0f;
    CampaignGameDetails gameDetails;
    private bool gameOverMessageShown = false;

    GameObject[] players;
    public float startEndGameTimer = 10f;
    private float endGameTimer;

    void Start()
    {
        Debug.Log("Campaign Game Logic Start");
        ScoreBoardCanvas.SetActive(false);


        GameObject campaignGameDetailsObject = GameObject.FindWithTag("CampaignGameDetailsObject");
        gameDetails = campaignGameDetailsObject.GetComponent<CampaignGameDetails>();
        gameDetails.setEnemyCount();
        numSpawnPoints = this.transform.childCount;




        // turn on the game details.

        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        players = new GameObject[playerConfigs.Length];
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            //var player = Instantiate(playerPrefabs[playerConfigs[i].PlayerClass], spawnPoints[i].transform.position, spawnPoints[i].transform.rotation, gameObject.transform);
            players[i] = Instantiate(playerPrefabs[playerConfigs[i].PlayerClass],
            GetRandomSpawnPoint().position,
            this.transform.GetChild(i).rotation,
            gameObject.transform);


            PlayerController pc = players[i].GetComponent<PlayerController>();
            pc.setPlayerIndex(i);//playerConfigs[i].PlayerIndex); 
            if (pc.getPlayerIndex() >= trailDots.Length)
            {
                pc.dot = trailDots[pc.getPlayerIndex() - trailDots.Length];
            }
            else
            {
                pc.dot = trailDots[pc.getPlayerIndex()];
            }


            // var player = Instantiate(playerPrefabs[playerConfigs[i].PlayerClass], 
            // this.transform.GetChild(i).position, this.transform.GetChild(i).rotation, gameObject.transform);

            players[i].GetComponent<InputHandler>().InitializePlayer(playerConfigs[i]);
        }

        Debug.Log("Game Logic Players assigned");
        gameDetails.gameActive = true;
        gameDetails.initializeGame(players);


    }
    private void Update()
    {
        if (gameDetails.gameActive == false && gameOverMessageShown == false)
        {
            gameOverMessageShown = true;
            // game should be over here. As update will be called after 
            Debug.Log("Game Is Over");
            endGameTimer = startEndGameTimer;
            DisplayScoreBoard();

        }
        else if (gameOverMessageShown)
        {
            //Game has ended, display is being shown
            // count down timer 30 seconds
            // after, 

            if (endGameTimer >= 0)
            {
                endGameTimer -= Time.deltaTime;
            }
            else
            {
                // time to reload the main menu scene

                GameObject gd = GameObject.Find("GameDetails");
                GameObject audioManager = GameObject.Find("AudioManager");
                GameObject playerConfigs = GameObject.Find("PlayerConfigurationManager");
                Destroy(gd);
                Destroy(audioManager);
                Destroy(playerConfigs);
                SceneManager.LoadScene(MainMenuScene);
            }

        }
    }
    public IEnumerator SpawnArcher(GameObject player)
    {
        // just respawn player at the beginning 
        PlayerController p = player.GetComponent<PlayerController>();

        Transform spawnPoint = GetRandomSpawnPoint();
        player.transform.position = spawnPoint.transform.position;

        player.GetComponent<PlayerController>().enable(false);
        yield return new WaitForSeconds(respawnDelay);
        player.GetComponent<PlayerController>().enable(true);
    }

    Transform GetRandomSpawnPoint()
    {
        //return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

        GameObject g = this.transform.GetChild(UnityEngine.Random.Range(0, numSpawnPoints - 1)).gameObject;


        // GameObject x = this.transform.GetChild(UnityEngine.Random.Range(0, numChildren)).gameObject;
        // while (x.gameObject.CompareTag("Player"))
        // {
        //     x = this.transform.GetChild(UnityEngine.Random.Range(0, numChildren)).gameObject;
        // }

        return g.transform; // y.transform;
    }

    /*
    Marbles are still spawning outside of playable areas. 
    */
    public void DroneDied()
    {
        gameDetails.enemyCount--;
    }
    public void DisplayScoreBoard()
    {
        ScoreBoardCanvas.SetActive(true);


        // TODO, DISPLAY RESULTS ON A CANVAS SO EVERYBODY CAN SEE THEM
        scoreBoardText.text += "\n";
        for (int i = 0; i < gameDetails.players.Length; i++)
        {
            scoreBoardText.text += "Player " + i.ToString() + " scored " + gameDetails.players[i].score.ToString() + " points" + "\n";

        }

    }
    public GameObject[] GetPlayerGameObjects()
    {
        Debug.Log("CampaignGameLogic.GetPlyaeGameObjects");
        return players;
    }
}

// 10 robots.
// players have collective score of 9.
/*
    -> one robot remaing, game not over.
Playes have collective score of 10 or more?
    -> zero robots remaining. 

*/