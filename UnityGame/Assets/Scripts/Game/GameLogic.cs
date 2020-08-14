using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    // [SerializeField]
    // private GameObject[] spawnPoints;

    //private GameObject spawnPointPlacements;
    [SerializeField]
    private GameObject[] playerPrefabs;
    [SerializeField]
    public GameObject[] trailDots;

    private int numChildren;
    public float respawnDelay = 3.0f;
    ArenaGameDetails gameDetails;
    private bool gameOverMessageShown = false;



    void Start()
    {
        // for(int i = 0; i< spawnPointPlacements.transform.childCount;i++){
        //     spawnPoints[i] = spawnPointPlacements.transform.GetChild(i).gameObject; 
        // }

        // get the gametype
        GameObject tom = GameObject.FindWithTag("ArenaGameDetailsObject");
        gameDetails = tom.GetComponent<ArenaGameDetails>();

        if ((MainMenu.GameTypes)gameDetails.gameType == MainMenu.GameTypes.freeForAll)
        {
            //UnityEngine.Debug.Log(gameDetails.gameType.ToString());
        }
        else if ((MainMenu.GameTypes)gameDetails.gameType == MainMenu.GameTypes.spaceMarbles)
        {
            //UnityEngine.Debug.Log(gameDetails.gameType.ToString());
        }
        else
        {
            UnityEngine.Debug.Log("Yikes");
        }
        // turn on the game details.

        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        GameObject[] players = new GameObject[playerConfigs.Length];
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
        numChildren = this.transform.childCount - players.Length;

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


            // TODO, DISPLAY RESULTS ON A CANVAS SO EVERYBODY CAN SEE THEM
            for (int i = 0; i < gameDetails.players.Length; i++)
            {
                Debug.Log("Player " + i.ToString() + " scored " + gameDetails.players[i].score.ToString() + " points");
            }
        }
    }
    public IEnumerator SpawnArcher(GameObject player)
    {
        PlayerController p = player.GetComponent<PlayerController>();

        if ((MainMenu.GameTypes)gameDetails.gameType == MainMenu.GameTypes.freeForAll)
        {
            // every kill in freefor all needs to be added to the killers score. 

            // player was killed by environment and not another player, 
            // player can die by trail. 
            if (p.killedBy == null || p.killedBy == p)
            {
                gameDetails.addScoreTo(p.getPlayerIndex(), -1);
            }
            // player was killed by another player
            else
            {

                int killerPlayerIndex = p.killedBy.GetComponent<PlayerController>().getPlayerIndex();

                // TODO MAKE A HIT FEED using the below concept

                // Debug.Log(killerPlayerIndex.ToString() + "Killed" + p.getPlayerIndex().ToString());
                gameDetails.addScoreTo(killerPlayerIndex, 1);
            }






        }
        else if ((MainMenu.GameTypes)gameDetails.gameType == MainMenu.GameTypes.spaceMarbles)
        {
            // This SHOULD BE WHERE WE DROP ALL THE MARBLES> 
            // UnityEngine.Debug.Log(gameDetails.gameType.ToString());
        }
        else
        {
            UnityEngine.Debug.Log("Yikes");
        }

        Transform spawnPoint = GetRandomSpawnPoint();
        player.transform.position = spawnPoint.transform.position;

        player.GetComponent<PlayerController>().enable(false);
        yield return new WaitForSeconds(respawnDelay);
        player.GetComponent<PlayerController>().enable(true);
    }

    Transform GetRandomSpawnPoint()
    {
        //return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];


        // SHOULD WORK FOR NOW
        // NOT GREAT
        // SORRY IT SUCKS. 

        // what should we do: other priotities are bigger. O(n).
        // spawn points are always the first x children if we have a count we can change the numChildren to numSpawnPoints. 
        // still have to count the spawn points somehow. 
        GameObject x = this.transform.GetChild(UnityEngine.Random.Range(0, numChildren)).gameObject;
        while (x.gameObject.CompareTag("Player"))
        {
            x = this.transform.GetChild(UnityEngine.Random.Range(0, numChildren)).gameObject;
        }

        return x.transform;
    }

    // GameObject GetSpawnPoint(int i){

    // }


}