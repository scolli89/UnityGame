using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RobotDroneController : MonoBehaviour
{
    // Start is called before the first frame update

    [Space]
    [Header("Drone Statistics:")]
    public int droneId;
    public int health = 1;
    public int shieldUses = 2;
    public bool shieldActive = false;

    [Space]
    [Header("Shooting Attributes:")]

    public float fireRate; // in frames 
    private float fireCount;
    public float LASER_BASE_SPEED = 1.0f;
    public float LASER_OFFSET = 1.2f;

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

    public float AGGRO_DISTANCE = 10f;

    public float ATTACK_DISTANCE = 7f;
    public float ATTACK_STOP_DISTANCE = 5f;
    public float WANDER_STOP_DISTACNE = 1.5f;




    [Space]
    [Header("Movement:")]
    public float moveCount;
    public float moveRate = 60f;
    public float PATROL_SPEED = 1f;
    public float PURSUIT_SPEED = 3f;
    public float magnitude;

    private bool isEMPed = false;
    public float empLength = 0f;


    [Space]
    [Header("References:")]
    Path path;
    private Seeker seeker;
    private Rigidbody2D rb;
    public Animator animator;
    public GameObject[] players;

    [Space]
    [Header("Prefabs:")]
    public GameObject projectilePrefab;
    public GameObject shieldPrefab;
    private SpriteRenderer spriteRenderer;

    [Space]
    [Header("Drone Script:")]
    public float nextWaypointDistance = 3f;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    private float STOPPING_DISTANCE = 5f;

    // private Vector2 _destination;
    private Vector2 _direction;
    public Transform target = null;
    // //private Drone _target;
    // private PlayerController _playerTarget;
    // private DroneState _currentState;
    private CampaignGameLogic gameLogic;
    private AudioManager audioManager;

    public void SetGameLogic(CampaignGameLogic gameLogic)
    {
        this.gameLogic = gameLogic;
    }
    void Start()
    {
        //Debug.Log("Robot Drone Controller Start");
        animator = this.GetComponent<Animator>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        rb = this.GetComponent<Rigidbody2D>();
        seeker = this.GetComponent<Seeker>();
        
        Random rnd = new Random();
        behaviourType = Random.Range(0, 1); // change for different behavior types. 
        //target = new Vector2(0, 0);
        //hasTarget = false;
        //_currentState = DroneState.Wander;
        // finds the gameLogic gameObject. then gets the script on it. 
        gameLogic = GameObject.Find("CampaignGameLogic").GetComponent<CampaignGameLogic>();

        players = gameLogic.GetPlayerGameObjects();
        audioManager = FindObjectOfType<AudioManager>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }
#region SwitchCase
    // function initialize drones call from gamelogic

    // Update is called once per frame
    // void Update()
    // {
    //     if (PauseMenu.GameIsPaused || path == null){
    //         return;
    //     }

    //     /* robot drone scans for a valid target. 
    //     passes a valid target to Move
    //     move based on behaviourType and target
    //     drone scans for a valid target. 
    //     passes a valid target to Combat 
    //     combat based on behaviourType and target
    //     */
    //     if(currentWaypoint >= path.vectorPath.Count){
    //         reachedEndOfPath = true;
    //         return;
    //     }else{
    //         reachedEndOfPath = false;
    //     }

    //     _direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
    //     Vector2 force  = _direction * LASER_BASE_SPEED * Time.deltaTime;
    //     updateStats();
         
    //     // switch (_currentState)
    //     // {
    //     //     case DroneState.Wander:
    //     //         {
    //     //             if (isEMPed)
    //     //             {
    //     //                 Debug.Log("Robot Zapped");
    //     //             }
    //     //             else
    //     //             {
    //     //                 if (NeedsDestination())
    //     //                 {
    //     //                     GetDestination();
    //     //                 }

    //     //                 rb.velocity = _direction * PATROL_SPEED;// * Time.deltaTime;


    //     //                 int count = 0;
    //     //                 while (IsPathBlocked())
    //     //                 {

    //     //                     //Debug.Log("Path Blocked");
    //     //                     GetDestination();
    //     //                     count++;
    //     //                     if (count >= 100)
    //     //                     {
    //     //                         count = 0;
    //     //                         break;
    //     //                     }
    //     //                 }

    //     //                 var targetToAggro = CheckForAggro();
    //     //                 if (targetToAggro != null)
    //     //                 {
    //     //                     _playerTarget = targetToAggro.gameObject.GetComponent<PlayerController>();

    //     //                     _currentState = DroneState.Chase;
    //     //                     // assign direction 
    //     //                     Vector2 targetPosition = new Vector2(_playerTarget.transform.position.x, _playerTarget.transform.position.y);
    //     //                     Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
    //     //                     _direction = targetPosition - currentPosition;
    //     //                     _direction.Normalize();
    //     //                 }




    //     //             }

    //     //             break;
    //     //         }
    //     //     case DroneState.Chase:
    //     //         {
    //     //             if (_playerTarget == null)
    //     //             {
    //     //                 //target died. 
    //     //                 Debug.Log("Entering Wander from chase");
    //     //                 _currentState = DroneState.Wander;
    //     //                 return;
    //     //             }


    //     //             Vector2 targetPosition = new Vector2(_playerTarget.transform.position.x, _playerTarget.transform.position.y);
    //     //             Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

    //     //             float d = Vector2.Distance(currentPosition, targetPosition);
    //     //             if (d < ATTACK_STOP_DISTANCE)
    //     //             {
    //     //                 // stop chaisng, just attack
    //     //                 //  Debug.Log("ENTERING ATTACK from chase");
    //     //                 rb.velocity = Vector2.zero;
    //     //                 _currentState = DroneState.Attack;
    //     //                 return;
    //     //             }
    //     //             else if (d >= AGGRO_DISTANCE)
    //     //             {
    //     //                 //lost the target. 
    //     //                 //Debug.Log("ENTERING Wander from chase ");
    //     //                 _currentState = DroneState.Wander;
    //     //                 return;
    //     //             }
    //     //             else
    //     //             {
    //     //                 //chase the player 
    //     //                 _direction = targetPosition - currentPosition;
    //     //                 _direction.Normalize();

    //     //                 if (!isEMPed)
    //     //                 {
    //     //                     rb.velocity = _direction * PATROL_SPEED;// * Time.deltaTime;
    //     //                 }


    //     //                 if (fireCount <= fireRate)
    //     //                 {
    //     //                     fireCount++;
    //     //                     // decrease firerate to attack faster
    //     //                     // chase and shoot. 
    //     //                     rb.velocity = _direction * PURSUIT_SPEED;


    //     //                 }
    //     //                 else
    //     //                 {
    //     //                     // this ties the shield generating to the same interval as firing. 
    //     //                     fireCount = 0;
    //     //                     if (health == 1 && !shieldActive && shieldUses > 0)
    //     //                     {
    //     //                         // need to stop first
    //     //                         //rb.velocity = Vector2.zero;
    //     //                         // wait?
    //     //                         ActivateShield();
    //     //                     }
    //     //                     else
    //     //                     {
    //     //                         // shooting, 
    //     //                         // need to stop first
    //     //                         //rb.velocity = Vector2.zero;
    //     //                         //wait? 
    //     //                         Vector2 at = new Vector2(_playerTarget.transform.position.x, _playerTarget.transform.position.y);
    //     //                         shoot(at);
    //     //                     }
    //     //                 }

    //     //             }
    //     //             break;
    //     //         }
    //     //     case DroneState.Attack:
    //     //         {
    //     //             if (_playerTarget == null)
    //     //             { //
    //     //               // should check if the target is still alive.
    //     //               // and if not, should revert back to wandering. 
    //     //                 _currentState = DroneState.Wander;
    //     //                 return;
    //     //             }
    //     //             Vector2 targetPosition = new Vector2(_playerTarget.transform.position.x, _playerTarget.transform.position.y);
    //     //             Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
    //     //             float d = Vector2.Distance(currentPosition, targetPosition);

    //     //             if (d >= ATTACK_STOP_DISTANCE)// && d <= AGGRO_DISTANCE)
    //     //             {
    //     //                 // Debug.Log("ENTERING Wander from attack");
    //     //                 _currentState = DroneState.Wander;
    //     //                 return;

    //     //                 // _currentState = DroneState.Chase;
    //     //                 // return;
    //     //             }
    //     //             // else if (d >= AGGRO_DISTANCE)
    //     //             // {
    //     //             //     _currentState = DroneState.Wander;
    //     //             //     return;

    //     //             // }
    //     //             // the stopped and attack
    //     //             else
    //     //             {
    //     //                 rb.velocity = Vector2.zero;
    //     //                 if (fireCount <= fireRate)
    //     //                 {
    //     //                     fireCount++;
    //     //                     // decrease firerate to attack faster
    //     //                 }
    //     //                 else
    //     //                 {
    //     //                     // this ties the shield generating to the same interval as firing. 
    //     //                     fireCount = 0;
    //     //                     if (health == 1 && !shieldActive && shieldUses > 0)
    //     //                     {
    //     //                         ActivateShield();
    //     //                     }
    //     //                     else
    //     //                     {
    //     //                         Vector2 at = new Vector2(_playerTarget.transform.position.x, _playerTarget.transform.position.y);
    //     //                         shoot(at);
    //     //                     }
    //     //                 }
    //     //             }


    //     //             break;
    //     //         }
    //     // }
         #endregion
    
    // }
    void FixedUpdate()
    {
        if (PauseMenu.GameIsPaused){
            return;
        }

        // look for the player, if we get null, we didn't find them
        target = CheckForAggro();
        if(target == null){
            // find random point in circle around our drone and set that as target
            GameObject temp = new GameObject(); 
            temp.transform.position = (Vector2)this.transform.position + Random.insideUnitCircle * 5;
            target = temp.transform;
        }

        //this.GetComponent<AIDestinationSetter>().target = target;
        // if Emped, don't do anything until timer is up
        if (isEMPed){
            if (empLength >= 0)
            {
                empLength -= Time.deltaTime;
                return;
            }
            else
            {
                Debug.Log("Robo emp over");
                //empLength = 0;
                isEMPed = false;
            }
        }

        // error catch
        if(path==null){
            return;
        }else if(currentWaypoint >= path.vectorPath.Count){
            // if we get to the last waypoint in the path, we reached the end of the path
            reachedEndOfPath = true;
            return;
        }else{
            reachedEndOfPath = false;
        }

        // direction is next waypoint along the path
        _direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        // move the drone towards direction
        rb.AddForce(_direction * moveRate * Time.deltaTime);

        // calculate distance from current position to next waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        // move up waypoint array when close enough
        if(distance < nextWaypointDistance){
            currentWaypoint++;
        }
        updateStats();
    }

    void UpdatePath(){
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }
    #region Old code
    // private bool IsPathBlocked()
    // {
    //     Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
    //     Vector2 newPosition = currentPosition + _direction;
    //     RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition, newPosition);
    //     foreach (RaycastHit2D hit in hits)
    //     {
    //         GameObject other = hit.collider.gameObject;
    //         if (other != this.gameObject)
    //         {
    //             if (other.CompareTag("Player"))
    //             {
    //                 // should set target to player. 
    //                 return false;

    //             }
    //             if (other.CompareTag("Enemy"))
    //             { // right now just the cannon. 
    //                 return true;

    //             }

    //             if (other.gameObject.CompareTag("DeathBox"))
    //             {
    //                 //Debug.Log("Falling");

    //                 return true;
    //             }

    //             if (other.CompareTag("Environment"))
    //             {
    //                 return true;

    //             }
    //             if (other.CompareTag("Shockwave"))
    //             {
    //                 return true;
    //             }
    //             if (other.CompareTag("BuilderWall"))
    //             {

    //                 return true;

    //             }
    //         }

    //     }
    //     return false;
    // }
    // private bool NeedsDestination()
    // {
    //     if (_destination == Vector2.zero)
    //     {
    //         // if the destination is not set, it needs a destinantion. 
    //         return true;
    //     }


    //     var distance = Vector2.Distance(
    //         new Vector2(transform.position.x, transform.position.y),
    //          _destination);

    //     if (distance <= WANDER_STOP_DISTACNE)
    //     {
    //         return true;
    //     }

    //     return false;
    // }
    // private void GetDestination()
    // {
    //     // getting a random destination on the board
    //     Vector2 transformXY = new Vector2(transform.position.x, transform.position.y);
    //     _destination = new Vector2(
    //          transformXY.x + (transform.forward.x * 4f) +
    //          UnityEngine.Random.Range(-4.5f, 4.5f),
    //          transformXY.y + (transform.forward.z * 4f) +
    //          UnityEngine.Random.Range(-4.5f, 4.5f));

    //     _direction = _destination - transformXY;
    //     _direction.Normalize();
    // }
    #endregion

    private Transform CheckForAggro()
    {
        Vector2 mPos = new Vector2(transform.position.x, transform.position.y);

        //Debug.Log("COME BACK TO ME. PLAYERS is Not working");

        foreach (GameObject player in players)
        {
            Vector2 pPos = new Vector2(player.transform.position.x, player.transform.position.y);
            // if (Mathf.Abs(player.transform.position.x - transform.position.x)
            // < DISTANCE_FROM_PLAYER &&
            //     Mathf.Abs(player.transform.position.y - transform.position.y)
            //      < DISTANCE_FROM_PLAYER)



            if (Vector2.Distance(mPos, pPos) <= AGGRO_DISTANCE)
            {
                // comparing between players who are closer. 
                return player.transform;
            }
        }


        return null;
    }

    void updateStats()
    {
        /*
            ADD UI FOR ENEMY HEALTH? 
            // MAYBE MAKE THAT A PASSIVE ABILITY FOR PLAYERS to be ableto see. Think vide game: the Surge

        */



    }

    public void setEmpEffect(float empLength)
    {
        if (isEMPed)
        {
            this.empLength += empLength;

        }
        else
        {
            isEMPed = true;
            this.empLength = empLength;
        }
        //_currentState = DroneState.Wander;
    }
    public void takeDamage(int damage)
    {
        gameLogic.DroneDied();

        Destroy(this.gameObject);

        // if (damage < 0)
        // {
        //     // if adding health
        //     health -= damage;
        // }
        // else if (shieldActive)
        // {
        //     // if shield is hit. 
        //     DeactivateShield();


        // }
        // else
        // {
        //     //regular damange. 
        //     health -= damage;
        // }

        // if (health == 0)
        // {
        //     gameLogic.DroneDied();
        //     //enemyGameManager.subtractFromDronesRemaining(1);
        //     Destroy(this.gameObject);
        // }

    }
    public void takeDamage(int damage, int playerIndex)
    {
        gameLogic.DroneDied(playerIndex);
        Destroy(this.gameObject);
    }
    void ActivateShield()
    {
        shieldUses--;
        shieldActive = true;
        animator.SetBool("shieldActive", shieldActive);
        // turn on shield animation
    }
    void DeactivateShield()
    {
        shieldActive = false;
        animator.SetBool("shieldActive", shieldActive);
        // turn off shield animation 
    }

    public void setPlayerTargets(GameObject[] passed)
    {
        players = passed;
    }
    void shoot(Vector2 shootAt)
    {
        Vector2 iPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 shootingDirection = shootAt - iPosition;
        shootingDirection.Normalize();
        //iPosition += shootingDirection * LASER_OFFSET;
        GameObject laser = Instantiate(projectilePrefab, iPosition, Quaternion.identity);
        //GameObject arrow = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        LaserController arrowController = laser.GetComponent<LaserController>();
        laser.GetComponent<LaserController>().audioManager = audioManager;

        arrowController.shooter = this.gameObject;
        arrowController.velocity = shootingDirection * LASER_BASE_SPEED; // adjust velocity
        laser.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
        Destroy(laser, 2.0f);
    }





    //////////////////////////////////


    void Combat(bool hasTarget)
    {
        /*
        remove later
        */
        behaviourType = 1;
        if (behaviourType == 0)
        {
            if ((health == 1 || health == 2) && !shieldActive && shieldUses > 0)
            {
                ActivateShield();
            }
        }
        else if (behaviourType == 1)
        {
            if (hasTarget)
            {

                if (fireCount <= fireRate)
                {
                    fireCount++;
                    // decrease firerate to attack faster
                }
                else
                {
                    fireCount = 0;
                    // shoot(target);
                }

            }

        }

        if (health == 1 && !shieldActive && shieldUses > 0)
        {
            ActivateShield();
        }


    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // move away from each other. 
            Vector2 movementDirection;// = Vector2.zero;
            movementDirection = new Vector2(this.transform.position.x - other.transform.position.x, this.transform.position.y - other.transform.position.y);
            rb.velocity = Vector2.zero;
            rb.velocity = movementDirection * PURSUIT_SPEED;


        }
        if (other.gameObject.CompareTag("DeathBox"))
        {
            Debug.Log("Robot Falling");
            audioManager.playSound("Fall");
            takeDamage(1);
        }
        if (other.gameObject.CompareTag("OverWallTrigger"))
        {
            spriteRenderer.sortingLayerName = "overWall";
            //PlayerController.DisplayLevel.overWall.ToString();
            //displayLevel.ToString();
        }
        else if (other.gameObject.CompareTag("UnderWallTrigger"))
        {
            spriteRenderer.sortingLayerName = "underWall";
            //displayLevel.ToString();
        }

    }
}






public enum DroneState
{
    Wander,
    Chase,
    Attack

}