using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceMarbleController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameLogic gameLogic;
    
    public void setGameLogic(GameLogic g){
        this.gameLogic = g; 
    }
    
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("PlayerFeet")){//"Player")){
            //player walks into it. 
            GameObject p =  other.transform.parent.gameObject;
            gameLogic.playerPickedUpMarble(p);//other.gameObject);
            //get rid of this guy.
            Destroy(this.gameObject);

        }
        if(other.CompareTag("DeathBox")){
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("DeathBox")){
            Debug.Log("Zoop");
            Destroy(this.gameObject);
        }
    }
}
