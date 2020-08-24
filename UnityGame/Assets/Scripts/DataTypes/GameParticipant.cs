using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameParticipant
    {
        
        public PlayerController pc;
        public ArenaGameDetails.SelfColor selfColor;
        public int score;
        public int numOfHeldMarbles; 
        public GameParticipant(PlayerController pc){
            selfColor = ++ ArenaGameDetails.lastAssignedSelfColor; 
            score = 0;
            numOfHeldMarbles =0; 
            this.pc = pc;
        }
        
    }