using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feetTriggerColliderController : MonoBehaviour
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
    bool allowUnder;
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
        if (playerController == null)
        {
            // null catch 
            Start();
        }
        #region Gameplay modifiers
        if (other.gameObject.CompareTag("DeathBox"))
        {
            playerController.audioManager.playSound("Fall");
            playerController.respawn();
        }

        else if (other.gameObject.CompareTag("OverWallTrigger"))
        {
            playerController.feetPos = (PlayerController.DisplayLevel)DisplayLevel.overWall;
        }

        else if (other.gameObject.CompareTag("UnderWallTrigger"))
        {
            playerController.feetPos = (PlayerController.DisplayLevel)DisplayLevel.underWall;
        }
        #endregion

        #region Sound effects
        if (other.gameObject.CompareTag("Base"))
        {
            Debug.Log("TRANSITION: " + playerController.groundSound + "to Base");
            playerController.groundSound = "Base";
        }

        else if (other.gameObject.CompareTag("Bridge"))
        {
            Debug.Log("TRANSITION: " + playerController.groundSound + "to Bridge");
            playerController.groundSound = "Bridge";
        }
        #endregion
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("OverWallTrigger") || other.gameObject.CompareTag("UnderWallTrigger"))
        {
            playerController.feetPos = (PlayerController.DisplayLevel)DisplayLevel.noWall;
        }

        if (other.gameObject.CompareTag("Base") || other.gameObject.CompareTag("Bridge"))
        {
            playerController.groundSound = "Ground";
        }
    }

    public enum DisplayLevel
    {
        //let us assume that the default position is being displayed over top of the wall. 
        underWall,
        overWall,
        noWall
    }
    void toggle()
    {
        if (playerController.displayLevel == (PlayerController.DisplayLevel)DisplayLevel.underWall)
        {
            playerController.displayLevel = (PlayerController.DisplayLevel)DisplayLevel.overWall;
        }
        else if (playerController.displayLevel == (PlayerController.DisplayLevel)DisplayLevel.overWall)
        {
            playerController.displayLevel = (PlayerController.DisplayLevel)DisplayLevel.underWall;
        }
    }
}