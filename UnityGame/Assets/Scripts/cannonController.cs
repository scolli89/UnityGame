using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonController : MonoBehaviour
{
    // Start is called before the first frame update
    [Space]
    [Header("Cannon Statistics:")]
    public Vector2 shootingDirection;
    public bool shootAtPlayer; 
    public float fireRate; // in frames 
    public bool justFired;
    private float count;

    [Space]
    [Header("Enemy Attributes:")]
    
    public float ARROW_BASE_SPEED = 1.0f;
    public float ARROW_DOWN_OFFSET = 1.0f;
    public float DISTANCE_FROM_PLAYER = 1.0f;
    


    [Space]
    [Header("References:")]
    private Rigidbody2D rb;
    public GameObject player;

    [Space]
    [Header("Prefabs:")]
    public GameObject projectilePrefab;


    void Start()
    {
    
    }

    // Update is called once per frame, but it is tied to frame rate. 
    void Update()
    {
        if(justFired){

            if( count < fireRate){
                count++; 
            } else {
                count = 0;
                justFired = false;
            }

        } else {
            if(checkDistance()){
                fire();
                justFired = true; 
            }
        }
    }

    bool checkDistance(){
        
        //if player is too close, fire arrow down. 
        if( Mathf.Abs(player.transform.position.x - transform.position.x) < DISTANCE_FROM_PLAYER &&
                Mathf.Abs(player.transform.position.y - transform.position.y) < DISTANCE_FROM_PLAYER ){
            return true; 
        }
        // otherwise 
        return false;   
    }


    void fire(){

        if(shootAtPlayer){

        }
        else{
            
        }



        // fires arrow Vector3 iPosition = transform.position; 
            Vector3 iPosition = transform.position;
            if(shootingDirection.y < 0){ //down
                iPosition.y = iPosition.y - ARROW_DOWN_OFFSET;  
            } else if (shootingDirection.y > 0){ // up
                iPosition.y = iPosition.y + ARROW_DOWN_OFFSET; 
            } 
            if(shootingDirection.x < 0){
                iPosition.x = iPosition.x - ARROW_DOWN_OFFSET;  
            } else if(shootingDirection.x > 0){
                iPosition.x = iPosition.x - ARROW_DOWN_OFFSET;  
            }

        
        GameObject arrow = Instantiate(projectilePrefab,iPosition, Quaternion.identity);
        arrow.GetComponent<Rigidbody2D>().velocity = shootingDirection * ARROW_BASE_SPEED; // adjust velocity
        arrow.transform.Rotate(0,0,Mathf.Atan2(shootingDirection.y,shootingDirection.x) * Mathf.Rad2Deg);
        Destroy(arrow,2.0f);
        Debug.Log("Shoot");
       
    }

}
