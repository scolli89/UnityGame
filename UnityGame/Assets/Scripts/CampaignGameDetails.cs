using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignGameDetails : MonoBehaviour
{

    /*

    Im thinking that this gameObject can act as the initial decisions by the players for game set up, 
    then it can be used as the score board object. so that it can be used after the game.  

    */



    #region members
    public MainMenu.Levels level;

    public int enemyCount;
    public GameParticipant[] players;
    public bool gameActive;

    //static public SelfColor lastAssignedSelfColor = 0;
    public float gameTime;

    public bool levelCompleted;
    public bool respawnsActive;
    public string endGameMessage;
    private int checkCount;
    public int startCheckCount = 60;

    #endregion

    #region lifeCycleMethods
    private void Start()
    {
        gameActive = false;

        levelCompleted = false;

        gameTime = 0f;
        respawnsActive = false;
        checkCount = startCheckCount;

    }
    public void setEnemyCount(int enemyCount)
    {
        //
        this.enemyCount = enemyCount;
        // enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("EnemyCount: " + enemyCount);
        // if(levelSelected == MainMenu.Levels.tutorial){
        //     enemyCount = 1; 
        // }else{
        //     enemyCount = 0; 
        // }
    }

    private void Update()
    {
        if (gameActive)
        {
            gameTime += Time.deltaTime;
            // because of the loops, reduced checking frequency of win/lose to once every 60 frames
            if (checkCount >= 0)
            {
                checkCount--;
            }
            else
            {
                // check win conditions
                checkCount = startCheckCount;
                if (checkLoseCondition())
                {
                    // game is over?
                    // reload current scene? 
                    gameActive = false;
                    endGameMessage = "Lozar";
                    Debug.Log("Lozar");
                }
                else if (checkWinCondition())
                {
                    // game is over
                    gameActive = false;
                    endGameMessage = "Winnar";
                    Debug.Log("Winnar");
                }

            }

        }
    }
    private bool checkWinCondition()
    {
        int numPlayersToWin = players.Length;
        int numInEndZone = 0;
        foreach (GameParticipant player in players)
        {
            if (player.pc.getIsAlive())
            {
                if (player.pc.getInEndZone())
                {
                    // alive and in the endzone
                    numInEndZone++;
                }

            }
            else
            {
                // the player is dead
                numPlayersToWin--;
            }
        }
        return numInEndZone >= numPlayersToWin; //enemyCount <= 0;
    }
    private bool checkLoseCondition()
    {
        // worst case O(n);
        // worst case O(8);  
        foreach (GameParticipant player in players)
        {

            if (player.pc.getIsAlive())
            {
                return false; // as in atleast one person is alive. 
            }
        }
        return true; // ie everyone is dead, game over
    }

    #endregion
    public void initializeGame(GameObject[] passedPlayers)
    {
        players = new GameParticipant[passedPlayers.Length];

        for (int i = 0; i < passedPlayers.Length; i++)
        {
            PlayerController pc = passedPlayers[i].GetComponent<PlayerController>();
            GameParticipant gp = new GameParticipant(pc);
            players[i] = gp;
            // players[p.getPlayerIndex()] = p;

            //players[i]

        }

        //we now have all the players in the game. 





    }

    // use it to count kills by players
    public void addScoreTo(int pi, int scoreIncrease)
    {
        if (gameActive)
        {
            players[pi].score += scoreIncrease;
            // Debug.Log("Player " + pi + " Score : " + players[pi].score);
        }
    }



}
