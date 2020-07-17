using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDroneController : MonoBehaviour
{
    // Start is called before the first frame update

    [Space]
    [Header("Drone Statistics:")]
    public int droneId;
    public int health = 3;
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
    private float STOPPING_DISTANCE = 5f;

    private Vector2 _destination;
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


        //target = new Vector2(0, 0);
        //hasTarget = false;
        _currentState = DroneState.Wander;
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

                        rb.velocity = _direction * PATROL_SPEED;// * Time.deltaTime;
                        int count = 0;
                        while (IsPathBlocked())
                        {

                            //Debug.Log("Path Blocked");
                            GetDestination();
                            count++;
                            if (count >= 100)
                            {
                                count = 0;
                                break;
                            }
                        }

                        var targetToAggro = CheckForAggro();
                        if (targetToAggro != null)
                        {
                            _playerTarget = targetToAggro.gameObject.GetComponent<PlayerController>();

                            _currentState = DroneState.Chase;
                            // assign direction 
                            Vector2 targetPosition = new Vector2(_playerTarget.transform.position.x, _playerTarget.transform.position.y);
                            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
                            _direction = targetPosition - currentPosition;
                            _direction.Normalize();
                        }




                        break;
                    }
                case DroneState.Chase:
                    {
                        if (_playerTarget == null)
                        {
                            //target died. 
                            Debug.Log("Entering Wander from chase");
                            _currentState = DroneState.Wander;
                            return;
                        }


                        Vector2 targetPosition = new Vector2(_playerTarget.transform.position.x, _playerTarget.transform.position.y);
                        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

                        float d = Vector2.Distance(currentPosition, targetPosition);
                        if (d < ATTACK_STOP_DISTANCE)
                        {
                            // stop chaisng, just attack
                          //  Debug.Log("ENTERING ATTACK from chase");
                            rb.velocity = Vector2.zero;
                            _currentState = DroneState.Attack;
                            return;
                        }
                        else if (d >= AGGRO_DISTANCE)
                        {
                            //lost the target. 
                            //Debug.Log("ENTERING Wander from chase ");
                            _currentState = DroneState.Wander;
                            return;
                        }
                        else
                        {
                            //chase the player 
                            _direction = targetPosition - currentPosition;
                            _direction.Normalize();


                            if (fireCount <= fireRate)
                            {
                                fireCount++;
                                // decrease firerate to attack faster
                                // chase and shoot. 
                                rb.velocity = _direction * PURSUIT_SPEED;
                                

                            }
                            else
                            {
                                // this ties the shield generating to the same interval as firing. 
                                fireCount = 0;
                                if (health == 1 && !shieldActive && shieldUses > 0)
                                {
                                    // need to stop first
                                    //rb.velocity = Vector2.zero;
                                    // wait?
                                    ActivateShield();
                                }
                                else
                                {
                                    // shooting, 
                                    // need to stop first
                                    //rb.velocity = Vector2.zero;
                                    //wait? 
                                    Vector2 at = new Vector2(_playerTarget.transform.position.x, _playerTarget.transform.position.y);
                                    shoot(at);
                                }
                            }

                        }
                        break;
                    }
                case DroneState.Attack:
                    {
                        if (_playerTarget == null)
                        { //
                          // should check if the target is still alive.
                          // and if not, should revert back to wandering. 
                            _currentState = DroneState.Wander;
                            return;
                        }
                        Vector2 targetPosition = new Vector2(_playerTarget.transform.position.x, _playerTarget.transform.position.y);
                        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
                        float d = Vector2.Distance(currentPosition, targetPosition);

                        if (d >= ATTACK_STOP_DISTANCE)// && d <= AGGRO_DISTANCE)
                        {
                           // Debug.Log("ENTERING Wander from attack");
                            _currentState = DroneState.Wander;
                            return;

                            // _currentState = DroneState.Chase;
                            // return;
                        }
                        // else if (d >= AGGRO_DISTANCE)
                        // {
                        //     _currentState = DroneState.Wander;
                        //     return;

                        // }
                        // the stopped and attack
                        else 
                        {
                            rb.velocity = Vector2.zero;
                            if (fireCount <= fireRate)
                            {
                                fireCount++;
                                // decrease firerate to attack faster
                            }
                            else
                            {
                                // this ties the shield generating to the same interval as firing. 
                                fireCount = 0;
                                if (health == 1 && !shieldActive && shieldUses > 0)
                                {
                                    ActivateShield();
                                }
                                else
                                {
                                    Vector2 at = new Vector2(_playerTarget.transform.position.x, _playerTarget.transform.position.y);
                                    shoot(at);
                                }
                            }
                        }


                        break;
                    }
            }
        }
    }

    private bool IsPathBlocked()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPosition = currentPosition + _direction;
        RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition, newPosition);
        foreach (RaycastHit2D hit in hits)
        {
            GameObject other = hit.collider.gameObject;
            if (other != this.gameObject)
            {
                if (other.CompareTag("Player"))
                {
                    // should set target to player. 
                    return false;

                }
                if (other.CompareTag("Enemy"))
                { // right now just the cannon. 
                    return true;

                }

                if (other.CompareTag("Environment"))
                {
                    return true;

                }
                if (other.CompareTag("Shockwave"))
                {
                    return true;
                }
                if (other.CompareTag("BuilderWall"))
                {

                    return true;

                }
            }

        }
        return false;

    }
    private bool NeedsDestination()
    {
        if (_destination == Vector2.zero)
        {
            // if the destination is not set, it needs a destinantion. 
            return true;
        }


        var distance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.y),
             _destination);

        if (distance <= WANDER_STOP_DISTACNE)
        {
            return true;
        }

        return false;
    }
    private void GetDestination()
    {
        // getting a random destination on the board
        Vector2 transformXY = new Vector2(transform.position.x, transform.position.y);
        _destination = new Vector2(
             transformXY.x + (transform.forward.x * 4f) +
             UnityEngine.Random.Range(-4.5f, 4.5f),
             transformXY.y + (transform.forward.z * 4f) +
             UnityEngine.Random.Range(-4.5f, 4.5f));

        _direction = _destination - transformXY;
        _direction.Normalize();
    }

    private Transform CheckForAggro()
    {
        Vector2 mPos = new Vector2(transform.position.x, transform.position.y);

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

    public void setPlayerTargets(GameObject[] passed)
    {
        players = passed;
    }
    void shoot(Vector2 shootAt)
    {
        Vector2 iPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 shootingDirection = shootAt - iPosition;
        shootingDirection.Normalize();
        iPosition += shootingDirection * LASER_OFFSET;
        GameObject arrow = Instantiate(projectilePrefab, iPosition, Quaternion.identity);
        //GameObject arrow = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        LaserController arrowController = arrow.GetComponent<LaserController>();
        arrowController.shooter = this.gameObject;
        arrowController.velocity = shootingDirection * LASER_BASE_SPEED; // adjust velocity
        arrow.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
        Destroy(arrow, 2.0f);
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
            Debug.Log("Bonk with enemey");
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