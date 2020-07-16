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

    public Vector2 target;
    public bool hasTarget;



    [Space]
    [Header("Movement:")]
    public float moveCount;
    public float moveRate = 60f;
    public float PATROL_SPEED = 3f;
    public float PURSUIT_SPEED = 6f;
    public float magnitude;
    public float STOP_DISTANCE = 1f;


    [Space]
    [Header("References:")]
    private Rigidbody2D rb;
    public GameObject player; // shouldn't need this. 
    public Animator animator;
    public GameObject[] players;

    [Space]
    [Header("Prefabs:")]
    public GameObject projectilePrefab;
    public GameObject shieldPrefab;


    [Space]
    [Header("Drone Script:")]
    private float ATTACK_RANGE = 3f;
    private float _rayDistance = 5.0f;
    private float STOPPING_DISTANCE = 1.5f;

    private Vector2 _destination;
    private Quaternion _desiredRotation;
    private Vector2 _direction;
    //private Drone _target;
    private PlayerController _playerTarget;
    private DroneState _currentState;


    void Start()
    {
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        Random rnd = new Random();
        behaviourType = Random.Range(0, 1); // change for different behavior types. 


        target = new Vector2(0, 0);
        hasTarget = false;
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
            switch (_currentState)
            {
                case DroneState.Wander:
                    {
                        if (NeedsDestination())
                        {
                            GetDestination();
                        }

                        transform.rotation = _desiredRotation;
                        transform.Translate(Vector3.forward * Time.deltaTime * 5f);
                        var rayColor = IsPathBlocked() ? Color.red : Color.green;
                        Debug.DrawRay(transform.position, _direction * _rayDistance, rayColor);

                        while (IsPathBlocked())
                        {
                            Debug.Log("Path Blocked");
                            GetDestination();
                        }

                        var targetToAggro = CheckForAggro();
                        if (targetToAggro != null)
                        {
                            _playerTarget = targetToAggro.gameObject.GetComponenet<PlayerController>();
                            _currentState = DroneState.Chase;
                        }




                        break;
                    }
                case DroneState.Chase:
                    {
                        if (_playerTarget == null)
                        {
                            _currentState = DroneState.Wander;
                            return;
                        }

                        transform.LookAt(_playerTarget.transform);
                        transform.Translate(Vector3.forward * Time.deltaTime * 5f);

                        if (Vector3.Distance(transform.position, _playerTarget.transform.position) < ATTACK_RANGE)
                        {
                            _currentState = DroneState.Attack;
                        }


                        break;
                    }
                case DroneState.Attack:
                    {
                        if (_playerTarget != null)
                        {
                            Destroy(_playerTarget.gameObject);
                        }
                        _currentState = DroneState.Wander;
                        break;
                    }


            }

            //updateStats();
            // GameObject t = ScanForTargets();
            // if (t == null)
            // {
            //     // behaviour without a target. 
            //     //Move();
            //     Combat(Move());
            // }
            // else
            // {
            //     target = new Vector2(t.transform.position.x, t.transform.position.y);
            //     // Debug.Log("Update : " + target);
            //     Combat(Move(target));
            //     //Combat(target);
            // }




        }


    }

    private bool IsPathBlocked()
    {
        Ray ray = new Ray(transform.position, _direction);
        var hitSomething = Physics.RaycastAll(ray, _rayDistance, _layerMask);
        return hitSomething.Any();
    }
    private bool NeedsDestination()
    {
        if (_destination == Vector2.zero){
            // if the destination is not set, it needs a destinantion. 
            return true;
        }
        

        var distance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.y),
             _destination);
             
        if (distance <= STOPPING_DISTANCE)
        {
            return true;
        }

        return false;
    }
    private void GetDestination()
    {
        Vector3 testPosition = (transform.position + (transform.forward * 4f)) +
                               new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), 0f,
                                   UnityEngine.Random.Range(-4.5f, 4.5f));

        _destination = new Vector3(testPosition.x, 1f, testPosition.z);

        _direction = Vector3.Normalize(_destination - transform.position);
        _direction = new Vector3(_direction.x, 0f, _direction.z);
        _desiredRotation = Quaternion.LookRotation(_direction);
    }


    private Transform CheckForAggro()
    {
        float aggroRadius = 5f;

        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;
        var pos = transform.position;
        for (var i = 0; i < 24; i++)
        {
            if (Physics.Raycast(pos, direction, out hit, aggroRadius))
            {
                var drone = hit.collider.GetComponent<Drone>();
                if (drone != null && drone.Team != gameObject.GetComponent<Drone>().Team)
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.red);
                    return drone.transform;
                }
                else
                {
                    Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
                }
            }
            else
            {
                Debug.DrawRay(pos, direction * aggroRadius, Color.white);
            }
            direction = stepAngle * direction;
        }

        return null;
    }















    //////////////////////////////////
    bool Move(Vector2 moveTowards)
    {


        // check if it is close to the player.
        //        Debug.Log("MOVE() : " + moveTowards);
        Vector2 robotPosition = new Vector2(this.transform.position.x, this.transform.position.y);

        if (Mathf.Abs(robotPosition.x - moveTowards.x) <= STOP_DISTANCE &&
                Mathf.Abs(robotPosition.y - moveTowards.y) <= STOP_DISTANCE)
        {
            rb.velocity = Vector2.zero;
            return true;
        }
        else
        {
            // move towards player
            Vector2 movementDirection = new Vector2(0, 0);

            //  Debug.Log("MOVING TOWARDS PLAYER.");

            movementDirection = moveTowards - new Vector2(this.transform.position.x, this.transform.position.y);

            magnitude = movementDirection.magnitude;
            movementDirection.Normalize();

            rb.velocity = movementDirection * PURSUIT_SPEED;
            return false;
        }




    }
    void StopMovement()
    {
        rb.velocity = Vector2.zero;
    }
    bool Move()
    {



        if (moveCount >= moveRate)
        {
            moveCount = 0;

            Vector2 movementDirection = new Vector2(0, 0);
            int i = Random.Range(0, 4);

            if (i == 0)
            {
                // move north
                movementDirection.y = 1f;

            }
            else if (i == 1)
            {
                // move east
                movementDirection.x = 1f;
            }
            else if (i == 2)
            {
                // move south
                movementDirection.y = -1f;
            }
            else //(i == 3)
            {
                // move west

                movementDirection.x = -1f;
            }

            rb.velocity = movementDirection * PATROL_SPEED;
            return false;

        }
        else
        {
            moveCount++;
            return false;
        }



    }
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
                    shoot(target);
                }

            }

        }

        if (health == 1 && !shieldActive && shieldUses > 0)
        {
            ActivateShield();
        }


    }


    GameObject ScanForTargets()
    {
        foreach (GameObject player in players)
        {
            if (Mathf.Abs(player.transform.position.x - transform.position.x) < DISTANCE_FROM_PLAYER &&
                Mathf.Abs(player.transform.position.y - transform.position.y) < DISTANCE_FROM_PLAYER)
            {


                /*
                     // should add raycasting towards player position to make sure that their is not a wall in the way. 

                */
                // comparing between players who are closer. 
                return player; // returns the player that is first in the list and within the certain boundary
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


        if (health == 0)
        {
            Destroy(this.gameObject);
        }
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
        animator.SetBool("shieldActive", shieldActive);
        // turn on shield animation
    }
    void DeactivateShield()
    {
        shieldActive = false;
        animator.SetBool("shieldActive", shieldActive);
        // turn off shield animation 
    }
    void shoot(Vector2 shootAt)
    {
        Vector2 iPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 shootingDirection = shootAt - iPosition;
        shootingDirection.Normalize();
        iPosition += shootingDirection * ARROW_OFFSET;
        GameObject arrow = Instantiate(projectilePrefab, iPosition, Quaternion.identity);
        //GameObject arrow = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        LaserController arrowController = arrow.GetComponent<LaserController>();
        arrowController.shooter = gameObject;
        arrowController.velocity = shootingDirection * LASER_BASE_SPEED; // adjust velocity
        arrow.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
        Destroy(arrow, 2.0f);
    }

    public void AdjustPosition(Vector2 rob1Pos, Vector2 rob2Pos, float magnitude)
    {
        // in this context, rob2Pos referese to the robot attacted to the instance of this script. 
        // this is because the other robot is reposnible for moving away. 




    }

    public void setPlayerTargets(GameObject[] passed)
    {
        players = passed;
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
    }
}

public enum DroneState
{
    Wander,
    Chase,
    Attack

}