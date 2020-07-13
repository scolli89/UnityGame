using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDroneController : MonoBehaviour
{
    // Start is called before the first frame update

    [Space]
    [Header("Drone Statistics:")]

    public int health = 3;
    public int shieldUses = 2;
    public bool shieldActive = false;
    private int scanCount = 0;


    [Space]
    [Header("Shooting Attributes:")]

    public Vector2 shootingDirection;
    public bool shootAtPlayer;
    public float fireRate; // in frames 
    public bool justFired;
    private float fireCount;
    public float LASER_BASE_SPEED = 1.0f;
    public float ARROW_OFFSET = 1.2f;

    [Space]
    [Header("Behaviour Attributes:")]

    /*
    behaviorTypes::
    0 -> Defensive, non aggressive. 
    1 -> Aggressive. 
    2 -> ?
    3 -> ?
    */
    public int behaviourType = 0;

    public float DISTANCE_FROM_PLAYER = 1.0f;

    public float FIRE_WAIT_TIME = 2.0f;


    [Space]
    [Header("References:")]
    private Rigidbody2D rb;
    public GameObject player; // shouldn't need this. 
    public Animator animator;

    [Space]
    [Header("Prefabs:")]
    public GameObject projectilePrefab;
    public GameObject shieldPrefab;

    void Start()
    {
        animator = this.GetComponent<Animator>(); 
        Random rnd = new Random();
        behaviourType = Random.Range(0, 1); // change for different behavior types. 
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            /* robot drone scans for a valid target. 
            passes a valid target to Move
            move based on behaviourType and target
            drone scans for a valid target. 
            passes a valid target to Combat 
            combat based on behaviourType and target
            
            
            */
           
            updateStats();


            Move(ScanForTargets());
            Combat(ScanForTargets());
            updateStats();

        }


    }



    void Move(Vector2 target)
    {



    }
    void Combat(Vector2 target)
    {
        if (behaviourType == 0)
        {
            if ((health == 1 || health == 2) && !shieldActive && shieldUses > 0)
            {
                ActivateShield();
            }
        }
        else if (behaviourType == 1)
        {
            if (justFired)
            {
                if (fireCount < fireRate)
                {
                    fireCount++;
                    // decrease firerate to attack faster
                }
                else
                {
                    fireCount = 0;
                    justFired = false;
                }

            }
            else
            {
                justFired = true;
                shoot(target);

            }
            if (health == 1 && !shieldActive && shieldUses > 0)
            {
                ActivateShield();
            }


        }
    }



    void updateStats()
    {
        /*
            ADD UI FOR ENEMY HEALTH? 
            // MAYBE MAKE THAT A PASSIVE ABILITY FOR PLAYERS to be ableto see. Think vide game: the Surge

        */


        if (health == 0)
        {
            Destroy(this.gameObject);
        }
    }

    Vector2 ScanForTargets()
    {
        if(scanCount == 0) { // the move call
            scanCount ++; 
            
            if(behaviourType == 0){
                // run away
            }
            else if ( behaviourType == 1){
                // move closer

            }
            return new Vector2(0f, 0f);

        }
        else if(scanCount == 1 ){ // the comabt call
            scanCount --; 
            
            return new Vector2(0f, 0f);
        }



        return new Vector2(0f, 0f);
    }

    public void takeDamage(int damage)
    {

        if (damage < 0)
        {
            // if adding health
            health -= damage;
        }
        else if (shieldActive)
        {
            // if hit by a 
            DeactivateShield();


        }
        else
        {
            health -= damage;
        }

    }

    void ActivateShield()
    {
        shieldUses--;
        shieldActive = true;
        animator.SetBool("shieldActive",shieldActive);
        // turn on shield animation
    }
    void DeactivateShield()
    {
        shieldActive = false;
        animator.SetBool("shieldActive",shieldActive);
        // turn off shield animation 
    }
    void shoot(Vector2 target)
    {
        
        target.Normalize();
        GameObject arrow = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        ArrowController arrowController = arrow.GetComponent<ArrowController>();
        arrowController.shooter = gameObject;
        arrowController.velocity = shootingDirection * LASER_BASE_SPEED; // adjust velocity
        arrow.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
        Destroy(arrow, 2.0f);
    }
}

