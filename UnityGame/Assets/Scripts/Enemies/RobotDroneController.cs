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
    [Header("Movement:")]
    public float AGGRO_DISTANCE = 7f;
    public float moveRate = 60f;
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
    private bool cancelFire = false;
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
        
        // finds the gameLogic gameObject. then gets the script on it. 
        gameLogic = GameObject.Find("CampaignGameLogic").GetComponent<CampaignGameLogic>();
        players = gameLogic.GetPlayerGameObjects();

        audioManager = FindObjectOfType<AudioManager>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void FixedUpdate()
    {
        if (PauseMenu.GameIsPaused){
            return;
        }

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

        // look for the player, if we get null, we didn't find them
        target = CheckForAggro();
        if(target == null){
            // find random point in circle around our drone and set that as target
            GameObject temp = new GameObject(); 
            temp.transform.position = (Vector2)this.transform.position + Random.insideUnitCircle * 5;
            target = temp.transform;
            if(cancelFire == true){
                CancelInvoke("fireAtPlayer");
                cancelFire = false;
            }
        }
        else{
            if(cancelFire != true){
                InvokeRepeating("fireAtPlayer", 0, fireRate);
            }
            cancelFire = true;
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

    void fireAtPlayer(){
        Vector2 targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        float d = Vector2.Distance(currentPosition, targetPosition);

        shoot(targetPosition);
    }

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


            PlayerController pc = player.GetComponent<PlayerController>();
            if (Vector2.Distance(mPos, pPos) <= AGGRO_DISTANCE && pc.getIsAlive())
            {
                // comparing between players who are closer. 
                return player.transform;
            }
        }

        return null;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // move away from each other. 
            Vector2 movementDirection;// = Vector2.zero;
            movementDirection = new Vector2(this.transform.position.x - other.transform.position.x, this.transform.position.y - other.transform.position.y);
            rb.velocity = Vector2.zero;
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