using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArenaGameLogic : GameLogic
{
    // [SerializeField]
    // private GameObject[] spawnPoints;

    //private GameObject spawnPointPlacements;
    const string MainMenuScene = "Menu";
    [SerializeField]

    private GameObject[] playerPrefabs;
    [SerializeField]
    public GameObject[] trailDots;
    [SerializeField]
    public GameObject[] marblePrefabs;
    public GameObject[] mapMarkers;
    
    [SerializeField]
    public Sprite[] playerShadowSprites; 

    public GameObject ScoreBoardCanvas;
    public TextMeshProUGUI scoreBoardText;
    public GameObject killLogCanvas;
    public TextMeshProUGUI killLogText;
    private List<string> killLog;
    private int killLogMaxSize = 3;


    private int numSpawnPoints;
    public float respawnDelay = 3.0f;
    ArenaGameDetails gameDetails;
    private bool gameOverMessageShown = false;

    private float xmin;
    private float xmax;
    private float ymin;
    private float ymax;
    public float startEndGameTimer = 10f;
    private float endGameTimer;

    void Start()
    {
        ScoreBoardCanvas.SetActive(false);

        killLogCanvas.SetActive(true);
        killLog = new List<string>();
        //scoreBoardText.SetActive(false); 
        //randomExample();
        //getting map boundaries

        /*
        THINGS TO FIX:
        SPACE MARBLES:
        1. Marbles spawn in walls and outside of map.
        2. Marbles are dropped outside of map
        3. Aaron mentioned that picking up the marbles should be sooner when moving up to them.
            -> It used to be on the players hitbox. 
            -> It is now on the players feet trigger box. 
        4. UI Scoreboard. 
        */



        GameObject tom = GameObject.FindWithTag("ArenaGameDetailsObject");
        gameDetails = tom.GetComponent<ArenaGameDetails>();
        numSpawnPoints = this.transform.childCount;




        if ((MainMenu.GameTypes)gameDetails.gameType == MainMenu.GameTypes.freeForAll)
        {
            //UnityEngine.Debug.Log(gameDetails.gameType.ToString());
        }
        else if ((MainMenu.GameTypes)gameDetails.gameType == MainMenu.GameTypes.spaceMarbles)
        {
            /*
            Spawning the marblesdepending on the map . 
            */
            if ((MainMenu.Maps)gameDetails.mapName == MainMenu.Maps.Fissure)
            {

                xmin = Mathf.Min(mapMarkers[0].transform.position.x, mapMarkers[1].transform.position.x);
                xmax = Mathf.Max(mapMarkers[0].transform.position.x, mapMarkers[1].transform.position.x);
                ymin = Mathf.Min(mapMarkers[0].transform.position.y, mapMarkers[1].transform.position.y);
                ymax = Mathf.Max(mapMarkers[0].transform.position.y, mapMarkers[1].transform.position.y);



                for (int i = 0; i < gameDetails.numberOfMarbles; i++)
                {
                    // create marbles

                    var marble = Instantiate(RandomMarble(), RandomVector3(), Quaternion.identity, gameObject.transform);
                    marble.GetComponent<SpaceMarbleController>().setGameLogic(this);

                }
            }

            if ((MainMenu.Maps)gameDetails.mapName == MainMenu.Maps.Colosseum)
            {
                xmin = Mathf.Min(mapMarkers[0].transform.position.x, mapMarkers[1].transform.position.x, mapMarkers[2].transform.position.x);
                xmax = Mathf.Max(mapMarkers[0].transform.position.x, mapMarkers[1].transform.position.x, mapMarkers[2].transform.position.x);
                float radius = (xmax - xmin) / 2;

                for (int i = 0; i < gameDetails.numberOfMarbles; i++)
                {
                    // Vector3 ipos = new Vector3(0,0,0); 
                    // //ipos += UnityEngine.Random.insideUnitCircle * diff; 
                    // Vector2 p = UnityEngine.Random.insideUnitCircle * diff; 
                    Vector2 center = new Vector2(mapMarkers[2].transform.position.x, mapMarkers[2].transform.position.y);

                    var marble = Instantiate(RandomMarble(), center + UnityEngine.Random.insideUnitCircle * radius, Quaternion.identity, gameObject.transform);
                    marble.GetComponent<SpaceMarbleController>().setGameLogic(this);
                }
            }
            else
            {
                
                xmin = Mathf.Min(mapMarkers[0].transform.position.x, mapMarkers[1].transform.position.x);
                xmax = Mathf.Max(mapMarkers[0].transform.position.x, mapMarkers[1].transform.position.x);
                ymin = Mathf.Min(mapMarkers[0].transform.position.y, mapMarkers[1].transform.position.y);
                ymax = Mathf.Max(mapMarkers[0].transform.position.y, mapMarkers[1].transform.position.y);


                int temp = gameDetails.numberOfMarbles;
                Debug.Log("Number of marbles: "+ temp);
                for (int i = 0; i < gameDetails.numberOfMarbles; i++)
                {
                    // create marbles

                    var marble = Instantiate(RandomMarble(), RandomVector3(), Quaternion.identity, gameObject.transform);
                    marble.GetComponent<SpaceMarbleController>().setGameLogic(this);

                }
            }



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
                pc.dot = trailDots[pc.getPlayerIndex() % trailDots.Length];
            }
            else
            {
                pc.dot = trailDots[pc.getPlayerIndex()];
            }

            if(pc.getPlayerIndex() >= playerShadowSprites.Length){
                // modulus operator to loop around, just in case. 
                pc.setShadowSprite(playerShadowSprites[pc.getPlayerIndex() % playerShadowSprites.Length]); 
            }
            else{
                pc.setShadowSprite(playerShadowSprites[pc.getPlayerIndex()]); 
            }
            

            pc.SetGameLogicObject(this.gameObject);

            // var player = Instantiate(playerPrefabs[playerConfigs[i].PlayerClass], 
            // this.transform.GetChild(i).position, this.transform.GetChild(i).rotation, gameObject.transform);

            players[i].GetComponent<InputHandler>().InitializePlayer(playerConfigs[i]);
        }


        gameDetails.gameActive = true;
        gameDetails.initializeGame(players);


    }
    public Vector3 RandomVector3()
    {
        //float x = UnityEngine.Random.Range(xmin, xmax);

        float randX = (xmax - xmin);
        randX *= UnityEngine.Random.value;

        float randY = (ymax - ymin);
        randY *= UnityEngine.Random.value;
        float x = UnityEngine.Random.Range(xmin, xmax);
        float y = UnityEngine.Random.Range(ymin, ymax);

        Vector3 v = new Vector3(x, y, 0f);
        //Debug.Log(v);
        return v;
    }
    public GameObject RandomMarble()
    {
        int x = UnityEngine.Random.Range(0, marblePrefabs.Length);// - 1);
        //        Debug.Log("Marble: " + x);
        return marblePrefabs[x];
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
    public override IEnumerator SpawnArcher(GameObject player)
    {
        // called when a player dies.
        PlayerController p = player.GetComponent<PlayerController>();



        int killerid = WriteToKillLog(p);




        if ((MainMenu.GameTypes)gameDetails.gameType == MainMenu.GameTypes.freeForAll)
        {

            // Debug.Log(p.killedBy.name);
            // Debug.Log(p.killedBy == p);

            // every kill in freefor all needs to be added to the killers score. 

            // player was killed by environment and not another player, 
            // player can die by trail. 
            if (killerid == -1 || killerid == p.getPlayerIndex())
            {

                gameDetails.addScoreTo(p.getPlayerIndex(), -1);
            }
            else
            {
                gameDetails.addScoreTo(killerid, 1);
            }
            /*
            if (p.killedBy == null)// || p.killedBy == p)
            {
                //player has no killer/ killed themselves
                gameDetails.addScoreTo(p.getPlayerIndex(), -1);
            }
            else
            {
                // player was killed by something
                PlayerController killerPlayerController = p.killedBy.GetComponent<PlayerController>();
                int killerPlayerIndex = killerPlayerController.getPlayerIndex();



                if (killerPlayerIndex != p.getPlayerIndex())
                {
                    // some one else killed him
                    // Debug.Log(killerPlayerIndex.ToString() + "Killed" + p.getPlayerIndex().ToString());
                    gameDetails.addScoreTo(killerPlayerIndex, 1);
                }
                else
                {
                    // he killed himself
                    gameDetails.addScoreTo(p.getPlayerIndex(), -1);
                }

            }
            */


        }
        else if ((MainMenu.GameTypes)gameDetails.gameType == MainMenu.GameTypes.spaceMarbles)
        {
            // not holding any more marbles
            DropMarbles(player);

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
    public int WriteToKillLog(PlayerController p)
    {


        if (killLog.Count >= killLogMaxSize)
        {
            killLog.RemoveAt(0);
        }


        int deadId = p.getPlayerIndex();
        string tempKillString = "Player  " + deadId.ToString();
        int returnValue = 0;
        bool explosiveDeath = false;

        //Process the death
        if (p.killedBy == null)
        {
            tempKillString += " jumped off a cliff.";
            returnValue = -1;

        }
        else
        {
            int killerPlayerIndex;

            if (p.killedBy.CompareTag("TrailDot"))
            {
                // death by trail dot. 
                explosiveDeath = true;
                killerPlayerIndex = p.killedBy.GetComponent<TrailDotController>().sploder.GetComponent<PlayerController>().getPlayerIndex();
                // -> gets the controller of the exploded trail dot, 
                //-> gets the one who exploded him
                // -> gets the controller of the one who exploded him.
                // -> returns the player index
            }
            else
            {
                // regular death by player
                PlayerController killerPlayerController = p.killedBy.GetComponent<PlayerController>();

                killerPlayerIndex = killerPlayerController.getPlayerIndex();
            }


            if (killerPlayerIndex != deadId)
            {
                if (explosiveDeath)
                {
                    tempKillString += " was 'sploded by Player ";
                }
                else
                {
                    tempKillString += " was killed by Player ";
                }
                tempKillString += killerPlayerIndex.ToString();

            }
            else
            {
                if(explosiveDeath){
                    tempKillString += " 'sploded themselves.";
                } else {
                    tempKillString += "died.";
                }
                
            }
            returnValue = killerPlayerIndex;

        }
        Debug.Log(tempKillString);
        //Add and display
        killLog.Add(tempKillString);
        killLogText.text = "";
        for (int i = 0; i < killLog.Count; i++)
        {
            killLogText.text += killLog[i] + "\n";
        }



        return returnValue;

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

    public void playerPickedUpMarble(GameObject p)
    {

        // PlayerController pc = p.GetComponent<PlayerController>();
        // int x = pc.getPlayerIndex();
        // Debug.Log(x);
        // gameDetails.players[x].numOfHeldMarbles += 1; 
        gameDetails.players[p.GetComponent<PlayerController>().getPlayerIndex()].numOfHeldMarbles += 1;

    }
    public void DropMarbles(GameObject player)
    {
        PlayerController p = player.GetComponent<PlayerController>();
        int marblesToDrop = gameDetails.setMarblesTo(p.getPlayerIndex(), 0);
        Vector3 deathSpot = player.transform.position;
        // consider

        /*
        1. Spawn all at the spot of players death.
        2. maker them move 
        */

        // instantiate the marbles at the spot of death.
        for (int i = 0; i < marblesToDrop; i++)
        {

            // create marbles
            Vector3 randomCirle = UnityEngine.Random.insideUnitCircle * 3;
            Vector3 dropSpot = deathSpot + randomCirle;

            var marble = Instantiate(RandomMarble(), dropSpot, Quaternion.identity, gameObject.transform);

            marble.GetComponent<SpaceMarbleController>().setGameLogic(this);
        }

    }
    public void DisplayScoreBoard()
    {
        ScoreBoardCanvas.SetActive(true);
        killLogCanvas.SetActive(false);



        if ((MainMenu.GameTypes)gameDetails.gameType == MainMenu.GameTypes.spaceMarbles)
        {
            gameDetails.convertMarblesToScore();
        }

        // TODO, DISPLAY RESULTS ON A CANVAS SO EVERYBODY CAN SEE THEM
        scoreBoardText.text += "\n";
        for (int i = 0; i < gameDetails.players.Length; i++)
        {
            scoreBoardText.text += "Player " + i.ToString() + " scored " + gameDetails.players[i].score.ToString() + " points" + "\n";

        }





    }


}

