using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Space]
    [Header("Character Attributes:")]
    public float MOVEMENT_BASE_SPEED = 1.0f;
    public float ARROW_BASE_SPEED = 1.0f;
    public float CROSSHAIR_DISTANCE = 5.0f;

    public float BUILDER_POWER_DISTANCE = 2.0f;
    public float AIMING_BASE_PENALTY = 0.1f;
    public float ARROW_OFFSET = 1.2f;
    public float HEALING_WAIT = 2.0f;

    const string BUILDER_CLASS_BASIC = "BUILDER";

    const string HEALER_CLASS_BASIC = "HEALER";
    const string HEALER_CLASS_SHOOT = "HEALER_SHOOT";

    const string SHOCK_CLASS_BASIC = "SHOCK";

    const string BUILDER_MOD_ONE = "B_MOD_ONE";
    const string BUILDER_MOD_TWO = "B_MOD_TWO";
    const string BUILDER_MOD_THREE = "B_MOD_THREE";

    [SerializeField]
    private int playerIndex = 0;

    public bool usingKeyBoard;

    [Space]
    [Header("Character Statistics:")]
    public int arrowsRemaining = 12;
    private int lastArrowsRemaining;
    public int health = 6;
    private int lastHealth;
    public float movementSpeed;
    private Vector2 movementDirection;
    private Vector2 pushBackDirection;
    private Vector2 aimDirection;
    public bool endOfAiming = false;
    public bool isAiming = false;
    public bool usingPower = false;
    public bool endUsingPower = false;
    public bool isShocked = false;
    public float PUSH_BACK_FORCE = 10f;
    public bool healing = false;
    public int healingCount = 0;
    public int healingTime = 50;  //  healingTime/ 50 = seconds. 

    [Space]
    [Header("Dash Variables:")]
    public bool isDashing;// = false;
    public float dashSpeed = 200.0f;
    private float dashTime;
    public float startDashTime = 0.2f;

    [Space]
    [Header("Character Class:")]
    public string className;
    public string modName;

    [Space]
    [Header("References:")]
    private Rigidbody2D rb;
    public Animator animator;
    public GameObject crosshair;
    public GameObject healthBar;
    private HealthBarController healthBarController;
    public GameObject ammoBar;
    private static AmmoController ammoController;

    private static PlayerClass playerClass;

    private bool firstUpdate = true;

    [Space]
    [Header("Prefabs:")]
    public GameObject arrowPrefab;

    Vector3 worldPosition;

    // todos
    // 

    // *****
    /*
    Notes:
    The new input system has it that the "OnX()" Functions are where the booleans are set.
    This is to remove the Input.Getters from the ProccessInputs. Nothing more. 
    The code should run like before. 

    Input manager is set to only join a new player on pause (start or p) button since
    on any button causes an error where a second arrow is shot from the player's spawn
    position every time they fire an arrow

     */
    //***



    void Start()
    {
        Debug.Log("Player Start");
        rb = GetComponent<Rigidbody2D>();
        lastHealth = health;
        //healthBarController = healthBar.GetComponent<HealthBarController>();
        healthBarController = this.gameObject.transform.GetChild(1).GetComponent<HealthBarController>();
        //Debug.Log(healthBarController);
        //setHealthAmount(health);

        //ammoController = ammoBar.GetComponent<AmmoController>();
        ammoController = this.gameObject.transform.GetChild(2).GetComponent<AmmoController>();
        //Debug.Log(ammoController);
        //setAmmoAmount(arrowsRemaining);
        lastArrowsRemaining = arrowsRemaining;

        crosshair.SetActive(false);

        // SET CLASS
        //setClass(HEALER_CLASS_BASIC);
        //setClass(BUILDER_CLASS_BASIC);
        //setClass(HEALER_CLASS_SHOOT);
        //setClass(SHOCK_CLASS_BASIC);

        setMod(BUILDER_MOD_ONE);
        //playerClass.getClass(); // returns Type : PlayerClass

        playerClass = this.gameObject.transform.GetChild(3).GetComponent<PlayerClass>();
        
        // DASH SET UP

        dashTime = startDashTime;

    }

    void Update()
    {
        //this is called once a frame. Tied to frame rate. 
        // get the change 
        if (!PauseMenu.GameIsPaused)
        {
            ProcessInputs(); // Aim() and AimPower are called within ProcessInputs. 
            Move(); // Move is called in FixedUpdate
            Animate();

            updateUI(); // doesnt depend on input
        }
    }
    private void FixedUpdate()
    {
        if (healing)
        {
            if (health >= 6)
            {
                healing = false;
                return;
            }
            else
            {
                healingCount++;
                if (healingCount >= healingTime)
                {
                    health++;
                    healingCount = 0;

                }
            }

        }
    }
    void updateUI()
    {
        
        // this works assuming health is changed somewhere else.
        // this works because reasons. like async calls.  
        if (firstUpdate)
        {
            Debug.Log("Update UI");
            setHealthAmount(health);
            setAmmoAmount(arrowsRemaining);
            firstUpdate = false;
        }
        if (health != lastHealth)
        {
            lastHealth = health;
            //healthBar.GetComponent<HealthBarController>().setHealth(health); 
            setHealthAmount(health);
        }
        if (arrowsRemaining != lastArrowsRemaining)
        {
            lastArrowsRemaining = arrowsRemaining;
            setAmmoAmount(arrowsRemaining);
        }
    }

    void ProcessInputs()
    {
        // with the new system, we will have already gotten the input booleans. Ie isDashing, 
        if (isDashing)
        {
            //DASH() or something 
        }
        else if (isAiming)
        {
            crosshair.SetActive(true);
            Aim();
            movementSpeed *= AIMING_BASE_PENALTY;
        }
        else if (usingPower)
        {
            crosshair.SetActive(true);
            
            AimPower();
            movementSpeed *= AIMING_BASE_PENALTY;
        } 
    }

    public int getPlayerIndex(){
        return playerIndex;
    }

    public void setMovementDirection(Vector2 direction)
    {
        movementDirection = direction;
        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
    }

    // called on hold of aim button
    public void setIsAiming()
    {
        isAiming = true;
    }

    //called when aim button is released
    public void setIsFiring()
    {
        isAiming = false;
        endOfAiming = true;
        Shoot();
    }

    public void setIsAimingPower()
    {
        usingPower = true;
    }

    public void setIsFiringPower()
    {
        usingPower = false;
        endUsingPower = true;
        usePower();
    }    

    public void setIsDashing()
    {
        isDashing = true;
    }

    void Move()
    {
        if (isShocked)
        {
            Debug.Log("SHOCK MOVE");
            rb.velocity = Vector2.zero;
            // need to save the movement direction at the time of push back . 
            rb.velocity = pushBackDirection * PUSH_BACK_FORCE;
            pushBackDirection = Vector2.zero;
            isShocked = false;
        }
        else if (isDashing)
        {
            if (movementDirection.magnitude == 0)
            {

            }
            else
            {
                if (dashTime <= 0)
                {
                    //dashDirection = 0;
                    movementDirection = Vector2.zero;
                    dashTime = startDashTime;
                    rb.velocity = Vector2.zero;
                }
                else
                {
                    dashTime -= Time.deltaTime;
                    rb.velocity = movementDirection * dashSpeed;
                }
            }
        }
        else
        {
            // regular movement. 
            rb.velocity = movementDirection * movementSpeed * MOVEMENT_BASE_SPEED;
        }
    }

    void Animate()
    {
        if (movementDirection != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movementDirection.x);
            animator.SetFloat("Vertical", movementDirection.y);
        }
        animator.SetFloat("Speed", movementSpeed);
    }

    void Aim()
    {
        // if (usingKeyBoard)
        // {
        //     Vector3 shootdirection = Mouse.current.position.ReadValue(); 
        //     shootdirection.z = 0.0f;
        //     shootdirection = Camera.main.ScreenToWorldPoint(shootdirection);
        //     shootdirection = shootdirection - transform.position;
        //     aimDirection = new Vector2(shootdirection.x, shootdirection.y);
        // }
        // else
        // {
        //     aimDirection = movementDirection;
        // }

        aimDirection = movementDirection;
        aimDirection.Normalize();
        //Debug.Log("AimDirection" + aimDirection);

        crosshair.transform.localPosition = aimDirection * CROSSHAIR_DISTANCE;
        //crosshair.transform.localPosition = movementDirection * CROSSHAIR_DISTANCE;
    }

    void AimPower()
    {
        // if (usingKeyBoard)
        // {
        //     Vector3 shootdirection = Mouse.current.position.ReadValue(); // mousePosition;
        //     shootdirection.z = 0.0f;
        //     shootdirection = Camera.main.ScreenToWorldPoint(shootdirection);
        //     shootdirection = shootdirection - transform.position;
        //     aimDirection = new Vector2(shootdirection.x, shootdirection.y);
        // }
        // else
        // {
        //     aimDirection = movementDirection;
        // }
        aimDirection = movementDirection;
        aimDirection.Normalize();
        //Debug.Log("AimDirection" + aimDirection);

        crosshair.transform.localPosition = aimDirection * BUILDER_POWER_DISTANCE;
        //crosshair.transform.localPosition = movementDirection * CROSSHAIR_DISTANCE;
    }

    void Shoot()
    {
        if (endOfAiming)
        {
            if (arrowsRemaining > 0)
            {
                arrowsRemaining--;

                Vector2 shootingDirection = crosshair.transform.localPosition;
                shootingDirection.Normalize();

                Vector2 iPosition = transform.position;
                iPosition = iPosition + shootingDirection * ARROW_OFFSET; // this prevents it from hitting the player


                GameObject arrow = Instantiate(arrowPrefab, iPosition, Quaternion.identity);
                ArrowController arrowController = arrow.GetComponent<ArrowController>();
                arrowController.shooter = gameObject;
                arrowController.velocity = shootingDirection * ARROW_BASE_SPEED; // adjust velocity
                arrow.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
                Destroy(arrow, 2.0f);
            }
            else
            {
                Debug.Log("OUT OF ARROWS");
            }
            crosshair.SetActive(false);
            isAiming = false;
            endOfAiming = false;
            
        }
    }

    void usePower()
    {
        // i don't know if this is the best way to do it. 
        // i think that maybe have a gameobject with a seoerate builder power script. 
        // then maybe call the builder power one that script rather than build it all into the player controller. 
        // not sure what is more optimal. 
        if (endUsingPower)
        {
            // check if there is enough ammo for their power. 
            int ammoReq = playerClass.getAmmoReq();
            if (arrowsRemaining >= ammoReq)
            {
                arrowsRemaining -= ammoReq;
                playerClass.usePower(crosshair.transform.localPosition);//, classPrefab);
            }
            else
            {
                // consider giving an error message to player?
                // flash their ammo red or something to make it noticable that they have nothing
            }

            //playerClass.usePower(crosshair.transform.localPosition, classPrefab); 


            // if (className.Equals(BUILDER_CLASS_NAME))
            // {

            //     if (modName.Equals(BUILDER_MOD_ONE))
            //     {
            //         //TIME TO BUILD 
            //         Vector2 aimDirection = crosshair.transform.localPosition;
            //         Vector2 buildOffset = new Vector2(this.transform.position.x,this.transform.position.y);
            //         Vector2 iPosition = aimDirection + buildOffset;
            //         //iPosition.Normalize();

            //         GameObject wall = Instantiate(classPrefab, iPosition, Quaternion.identity);
            //         //wall.transform.Rotate(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) *Mathf.Rad2Deg + 90 );

            //         //original
            //         wall.transform.Rotate(0,0,Mathf.Atan2(aimDirection.y, aimDirection.x) *Mathf.Rad2Deg );

            //     }
            //     else if (modName.Equals(BUILDER_MOD_TWO))
            //     {

            //     }
            //     else if (modName.Equals(BUILDER_MOD_THREE))
            //     {

            //     }





            // }
            // else if (className.Equals(HEALER_CLASS_NAME))
            // {

            // }
            // else if (className.Equals(SHOCK_CLASS_NAME))
            // {

            // }
            crosshair.SetActive(false);
            usingPower = false;
            endUsingPower = false;
        }
    }


    // collsion box
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Shockwave")
        {// || other.gameObject.tag == "Enemey"){
            Debug.Log("PUSH BACK");

            isShocked = true;
            pushBackDirection = -movementDirection;
            // Vector2 otherPosition = new Vector2(other.gameObject.transform.position.x,  other.gameObject.transform.position.y);
            // Vector2 thisPosition = new Vector2(this.gameObject.transform.position.x,this.gameObject.transform.position.x);
            // Vector2 forceDirection =  thisPosition - otherPosition;
            // forceDirection.Normalize(); 
            // Debug.Log(forceDirection);

            // ShockwaveScript sw =  other.gameObject.GetComponent<ShockwaveScript>();
            // //rb.AddForce(forceDirection *sw.PUSH_BACK_FORCE);

            // rb.velocity = -movementDirection * sw.PUSH_BACK_FORCE * MOVEMENT_BASE_SPEED * Time.deltaTime;

        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {

        //Debug.Log("Conk");
        // if (other.gameObject.tag == "bullet")
        // {
        //     Destroy(other.gameObject);
        //     health--;
        //     //setHealthAmount(--health);
        //     Debug.Log("Donk");
        // }
    }
    //hurt boxs
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "bullet")
        {
            Debug.Log("Bonk");
            Destroy(other.gameObject);
            health--;
            //setHealthAmount(--health);

        }
        else if (other.gameObject.tag == "HealingAura")
        {
            Debug.Log("Entering");
            healing = true;
            //StartCoroutine("HealingPlayer");
        }
        else if (other.gameObject.tag == "HealingBullet")
        {
            Debug.Log("Healing Laser");
            health++;
            //StartCoroutine("HealingPlayer");
        }
        if (other.gameObject.tag == "Shockwave")
        {// || other.gameObject.tag == "Enemey"){
            Debug.Log("PUSH BACK");

            isShocked = true;
            pushBackDirection = -movementDirection;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Shockwave")
        {// || other.gameObject.tag == "Enemey"){
            Debug.Log("PUSH BACK");

            isShocked = true;
            pushBackDirection = -movementDirection;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "HealingAura")
        {
            Debug.Log("Leaving");
            healing = false;
            //StopCoroutine("HealingPlayer");
        }
    }


    public void takeDamage(int damage)
    {
        Debug.Log("Ray Bonk");
        health -= damage;
    }

    public void setAmmoAmount(int n)
    {
        ammoController.setAmmo(n);
    }
    public void setHealthAmount(int n)
    {
        healthBarController.setHealth(n);
    }

    public void setHealth(int health)
    {
        this.health = health;
    }
    public int getHealth()
    {
        return health;
    }
    public void setClass(string className)
    {
        this.className = className;
    }
    public string getClass()
    {
        return className;
    }
    public void setMod(string modName)
    {
        this.modName = modName;
    }
    public string getMod()
    {
        return modName;
    }

}
