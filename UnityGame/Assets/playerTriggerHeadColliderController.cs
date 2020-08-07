using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTriggerHeadColliderController : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerController playerController;
    void Start()
    {
        playerController = this.transform.parent.GetComponent<PlayerController>();

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(playerController == null){
            // null catch 
            Start();
        } 
        if (other.gameObject.CompareTag("OverWallTrigger"))
        {

            playerController.displayLevel = (PlayerController.DisplayLevel)DisplayLevel.overWall;
            Debug.Log("Overwalltrigger");
            playerController.allowUnder = false;
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
