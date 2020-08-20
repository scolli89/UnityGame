using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaGameDetails : MonoBehaviour
{

    /*

    Im thinking that this gameObject can act as the initial decisions by the players for game set up, 
    then it can be used as the score board object. so that it can be used after the game.  

    */



    #region members
    public Maps mapName;
    public GameTypes gameType;


    public List<Team> teams;

    public GameParticipant[] players;
    public bool gameActive;
    public int numberOfMarbles = 50; 
    static public SelfColor lastAssignedSelfColor = 0;
    public float gameTime;
    public float startGameTime = 60.0f;
    #endregion

    #region lifeCycleMethods
    private void Start()
    {
        numberOfMarbles = 50; //10;
        gameActive = false;
        
        teams = new List<Team>(); // 

        gameTime = startGameTime;
    }
    private void Update()
    {
        if (gameActive)
        {

            if (gameTime <= 0)
            {
                // game is over
                gameActive = false;
            }
            else
            {
                gameTime -= Time.deltaTime;
            }


        }

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

    public void addScoreTo(int pi, int scoreIncrease)
    {
        if (gameActive)
        {
            players[pi].score += scoreIncrease;
           // Debug.Log("Player " + pi + " Score : " + players[pi].score);
        }
    }
    public void convertMarblesToScore(){
        //This function just sets the score to be that of the num of held marbels
        // intended to be called once per game at the end. 
        // Made to resue the score board section. 

        Debug.Log("Converting");
        for(int i = 0; i < players.Length;i++){
            players[i].score = players[i].numOfHeldMarbles; 
        }
    }
    public int setMarblesTo(int pi,int newMarbles){
        // returns the value held in numOfHeldMarbles before the functions was called. 
        // this is so that I only need to make one function call to be able to instantiate
        // the right number of marbles while also dropping the correct number of marbles from 
        // the player. 
        int x = players[pi].numOfHeldMarbles; 
        players[pi].numOfHeldMarbles = newMarbles;
        return x; 
    }
    #region enums
    public enum SelfColor
    {
        start,
        red,
        blue,
        orange,
        green,
        yellow,
        purple,
        white,
        cyan,
        end
    }
    public enum TeamColor
    {
        start,
        red,
        blue,
        orange,
        green,
        yellow,
        purple,
        white,
        cyan,
        end


    }
    public enum GameTypes
    {
        start,
        freeForAll,
        spaceMarbles,
        end
    }
    public enum Maps
    {
       start,
        arena1,
        Fissure,
        Colosseum,
        end
    }
    #endregion
}
