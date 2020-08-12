﻿using System.Collections;
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
        if (other.gameObject.CompareTag("DeathBox"))
        {
            Debug.Log("oh no");
            playerController.respawn();
        }

        if (other.gameObject.CompareTag("OverWallTrigger"))
        {
            playerController.feetPos = (PlayerController.DisplayLevel)DisplayLevel.overWall; 
        }

        if (other.gameObject.CompareTag("UnderWallTrigger"))
        {
            playerController.feetPos = (PlayerController.DisplayLevel)DisplayLevel.underWall; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("OverWallTrigger") || other.gameObject.CompareTag("UnderWallTrigger"))
        {
            playerController.feetPos = (PlayerController.DisplayLevel)DisplayLevel.noWall;
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