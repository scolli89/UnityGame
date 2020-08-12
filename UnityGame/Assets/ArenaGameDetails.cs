using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaGameDetails : MonoBehaviour
{

    /*

    Im thinking that this gameObject can act as the initial decisions by the players for game set up, 
    then it can be used as the score board object. so that it can be used after the game.  

    */


    public class Team{
        public List<PlayerController> teamMembers; 
        public Team(){
            teamMembers = new List<PlayerController>(); 
        }
        public void addMember(PlayerController pc)
        {   
            teamMembers.Add(pc); 
        }
    }

    public Maps mapName;
    public GameTypes gameType;
    public List<Team> teams; 
    public bool gameActive; 

    private void Start() {
        teams = new List<Team>(); // 
        gameActive = false; 
    }
    private void Update()
    {
        if(gameActive){

        }
        
    }
    public void initializeGame(){



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
}
