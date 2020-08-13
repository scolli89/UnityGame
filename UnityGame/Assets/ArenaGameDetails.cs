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
    static public SelfColor lastAssignedSelfColor = 0;
    public float gameTime;
    public float startGameTime = 60.0f;
    #endregion

    #region lifeCycleMethods
    private void Start()
    {
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
        fissure,
        end
    }
    #endregion
}
