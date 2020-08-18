using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headTriggerColliderController : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerController playerController;
    void Start()
    {
        playerController = this.transform.parent.GetComponent<PlayerController>();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerController == null)
        {
            // null catch 
            Start();
        }
        if (other.gameObject.CompareTag("OverWallTrigger"))
        {
            playerController.headPos = (PlayerController.DisplayLevel)DisplayLevel.overWall;
        }

        if (other.gameObject.CompareTag("UnderWallTrigger"))
        {
            playerController.headPos = (PlayerController.DisplayLevel)DisplayLevel.underWall;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("OverWallTrigger") || other.gameObject.CompareTag("UnderWallTrigger"))
        {
            playerController.headPos = (PlayerController.DisplayLevel)DisplayLevel.noWall;
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