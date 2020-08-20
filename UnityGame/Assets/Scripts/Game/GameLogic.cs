using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    // [SerializeField]
    // private GameObject[] spawnPoints;

    //private GameObject spawnPointPlacements;
    [SerializeField]

    private GameObject[] playerPrefabs;
    [SerializeField]
    public GameObject[] trailDots;
    [SerializeField]
    public GameObject[] marblePrefabs;
    public GameObject[] mapMarkers;

    public GameObject ScoreBoardCanvas;
    public TextMeshProUGUI scoreBoardText;


    private int numSpawnPoints;
    public float respawnDelay = 3.0f;
    ArenaGameDetails gameDetails;
    private bool gameOverMessageShown = false;

    private float xmin;
    private float xmax;
    private float ymin;
    private float ymax;

    void Start()
    {
        ScoreBoardCanvas.SetActive(false);
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
        int x = UnityEngine.Random.Range(0, marblePrefabs.Length - 1);
        Debug.Log("Marble: " + x);
        return marblePrefabs[x];
    }
    private void Update()
    {
        if (gameDetails.gameActive == false && gameOverMessageShown == false)
        {
            gameOverMessageShown = true;
            // game should be over here. As update will be called after 
            Debug.Log("Game Is Over");
            DisplayScoreBoard();
        }
    }
    public IEnumerator SpawnArcher(GameObject player)
    {

        if ((MainMenu.GameTypes)gameDetails.gameType == MainMenu.GameTypes.freeForAll)
        {
            PlayerController p = player.GetComponent<PlayerController>();

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

    Transform GetRandomSpawnPoint()
    {
        //return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];


        // SHOULD WORK FOR NOW
        // NOT GREAT
        // SORRY IT SUCKS. 

        // what should we do: other priotities are bigger. O(n).
        // spawn points are always the first x children if we have a count we can change the numChildren to numSpawnPoints. 
        // still have to count the spawn points somehow. 
        float z = UnityEngine.Random.value;
        z *= numSpawnPoints;
        int c = (int)Mathf.RoundToInt(z) - 1;
        GameObject y = this.transform.GetChild(c).gameObject;


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

    private float[] noiseValues;
    public void randomExample()
    {
        //UnityEngine.Random.InitState(42);
        noiseValues = new float[10];
        for (int i = 0; i < noiseValues.Length; i++)
        {
            noiseValues[i] = UnityEngine.Random.value;
            Debug.Log(noiseValues[i]);
        }
    }
}

// public class CupGame : MonoBehaviour{
//     public int Score;
//     public Text ScoreText;

//     // Update is called once per frame
//     void Update(){
//         if(Score >= 5){
//             YouWin();
//         }

//     }
//     void YouWin(){
//         ScoreText.text = "You Win!";
//         Time.timeScale = 0f;
//     }
//     private void OnTriggerEnter2D(Collider2D other){
//         Destroy(other.gameObject);
//         AddScore();
//     }

//     void AddScore(){
//         Score++;
//         ScoreText.text = Score.ToString();
//     }
// }