using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTriggerColliderController : MonoBehaviour
{
    // Start is called before the first frame update
    ///
    /*
    The FEET COLLIDER FOR THE BOTTOM OF THE PLAYER
    */
    //
   // HEAD is for displaying over the wall. 
   // FEET is for displaying under the wall. 
    PlayerController playerController;
    bool allowUnder ;
    void Start()
    {
        playerController = this.transform.parent.GetComponent<PlayerController>(); 
        allowUnder = true; 
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if(other.gameObject.CompareTag("OverWallTrigger") && other.gameObject.CompareTag("UnderWallTrigger")){
        //     displayLevel = DisplayLevel.overWall; 
        //     Debug.Log("BOTH");
        // }
        // else
        if(playerController == null){
            // null catch 
            Start();
        } 
        // if (other.gameObject.CompareTag("OverWallTrigger"))
        // {

        //     playerController.displayLevel = (PlayerController.DisplayLevel)DisplayLevel.overWall;
        //     Debug.Log("Overwalltrigger");
        //     allowUnder = false; 
        // }
        
        //if (playerController.allowUnder && other.gameObject.CompareTag("UnderWallTrigger"))
        if (other.gameObject.CompareTag("UnderWallTrigger"))
        {
            playerController.displayLevel = (PlayerController.DisplayLevel)DisplayLevel.underWall;
            Debug.Log("Underwalltrigger");
        }



        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
       
        if(other.gameObject.CompareTag("OverWallTrigger")){
            allowUnder = false; 
        }
       
    }
    private void OnTriggerExit2D(Collider2D other)
    {
       if(other.gameObject.CompareTag("OverWallTrigger")){
           allowUnder = true; 
       }
        


    }


    
    public enum DisplayLevel
    {
        //let us assume that the default position is being displayed over top of the wall. 
        underWall,
        overWall,
        noWall
    }
}

