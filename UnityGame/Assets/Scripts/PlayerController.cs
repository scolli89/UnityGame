using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int playerId;

    public int press = 0;

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
    const string SHOCK_CLASS_NAME = "SHOCK";

    const string BUILDER_MOD_ONE = "B_MOD_ONE";
    const string BUILDER_MOD_TWO = "B_MOD_TWO";
    const string BUILDER_MOD_THREE = "B_MOD_THREE";


    public bool usingKeyBoard;

    [Space]
    [Header("Character Statistics:")]
    public int arrowsRemaining = 12;
    private int lastArrowsRemaining;
    public int health = 6;
    private int lastHealth;
    public float movementSpeed;
    private Vector2 movementDirection;
    private Vector2 aimDirection;
    public bool endOfAiming = false;
    public bool isAiming = false;
    public bool usingPower = false;

    public bool endUsingPower = false;

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

    // public GameObject classPrefab;

    Vector3 worldPosition;


    // Start is called before the first frame update

    // todos
    // 

    // *****
    /*
     
     The new input system has it that the "OnX()" Functions are where the booleans are set.
    This is to remove the Input.Getters from the ProccessInputs. Nothing more. 
    The code should run like before. 

     
     */
    //***



    void Start()
    {
        Debug.Log("Player Start");
        rb = GetComponent<Rigidbody2D>();
        lastHealth = health;
        //healthBarController = healthBar.GetComponent<HealthBarController>();
        healthBarController = this.gameObject.transform.GetChild(2).GetComponent<HealthBarController>();
        //Debug.Log(healthBarController);
        //setHealthAmount(health);

        //ammoController = ammoBar.GetComponent<AmmoController>();
        ammoController = this.gameObject.transform.GetChild(3).GetComponent<AmmoController>();
        //Debug.Log(ammoController);
        //setAmmoAmount(arrowsRemaining);
        lastArrowsRemaining = arrowsRemaining;

        crosshair.SetActive(false);

        // SET CLASS
        //setClass(HEALER_CLASS_BASIC);
        setClass(BUILDER_CLASS_BASIC);

        setMod(BUILDER_MOD_ONE);

        if (getClass().Equals(BUILDER_CLASS_BASIC))
        {
            playerClass = this.gameObject.transform.GetChild(4).GetComponent<BuilderClassBasic>();
        }
        else if (getClass().Equals(HEALER_CLASS_BASIC))
        {
            playerClass = this.gameObject.transform.GetChild(4).GetComponent<HealerClassBasic>();
        }
        else if (getClass().Equals(SHOCK_CLASS_NAME))
        {
            // playerClass = this.gameObject.transform.GetChild(4).GetComponent<BuilderClassController>();
        }

        // DASH SET UP

        dashTime = startDashTime;

    }

    // Update is called once per frame
    void Update()
    {
        //this is called once a frame. Tied to frame rate. 
        // get the change 
        if (!PauseMenu.GameIsPaused)
        {
            updateUI(); // doesnt depend on input

            ProcessInputs(); // Aim() and AimPower are called within ProcessInputs. 
            Move(); // Move is called in FixedUpdate
            Animate();
            
            usePower();
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

    public void OnMove(CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        movementDirection.Normalize();
    }

    public void OnPower()
    {

    }

    
    // because input manager package is garbage and hold doesn't work
    public void StupidAssFix()
    {
        press++;
        if (press == 2)
        {
            OnAim();
        }
        else if (press > 2)
        {
            OnFire();
            press = -1;
        }
    }

    // called on hold of aim button
    public void OnAim()
    {
        Debug.Log("Button HOLD");
        isAiming = true;
    }

    //called when aim button is released
    public void OnFire()
    {
        Debug.Log("Button release");
        endOfAiming = true;
    }

    public void OnDash()
    {
        isDashing = true;
    }

    void Move()
    {
        if (isDashing)
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
        if (usingKeyBoard)
        {
            Vector3 shootdirection = Mouse.current.position.ReadValue(); 
            shootdirection.z = 0.0f;
            shootdirection = Camera.main.ScreenToWorldPoint(shootdirection);
            shootdirection = shootdirection - transform.position;
            aimDirection = new Vector2(shootdirection.x, shootdirection.y);
        }
        else
        {
            aimDirection = movementDirection;
        }

        aimDirection.Normalize();
        //Debug.Log("AimDirection" + aimDirection);

        crosshair.transform.localPosition = aimDirection * CROSSHAIR_DISTANCE;
        //crosshair.transform.localPosition = movementDirection * CROSSHAIR_DISTANCE;

        if (endOfAiming == true)
        {
            isAiming = false;
            Shoot(); 
        }
    }

    void AimPower()
    {
        if (usingKeyBoard)
        {
            Vector3 shootdirection = Mouse.current.position.ReadValue(); // mousePosition;
            shootdirection.z = 0.0f;
            shootdirection = Camera.main.ScreenToWorldPoint(shootdirection);
            shootdirection = shootdirection - transform.position;
            aimDirection = new Vector2(shootdirection.x, shootdirection.y);
        }
        else
        {
            aimDirection = movementDirection;
        }

        aimDirection.Normalize();
        //Debug.Log("AimDirection" + aimDirection);

        crosshair.transform.localPosition = aimDirection * BUILDER_POWER_DISTANCE;
        //crosshair.transform.localPosition = movementDirection * CROSSHAIR_DISTANCE;
    }

    void Shoot()
    {
        if (endOfAiming)
        {
            Vector2 shootingDirection = crosshair.transform.localPosition;
            shootingDirection.Normalize();
            // need to determine which if the player is shooting down. 
            Vector2 iPosition = transform.position;
            iPosition = iPosition + shootingDirection * ARROW_OFFSET; // this prevents it from hitting the player

            GameObject arrow = Instantiate(arrowPrefab, iPosition, Quaternion.identity);
            arrow.GetComponent<Rigidbody2D>().velocity = shootingDirection * ARROW_BASE_SPEED; // adjust velocity
            arrow.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
            
            // turn everything back to normal walking. 
            crosshair.SetActive(false);
            isAiming = false;
            endOfAiming = false;
            Destroy(arrow, 2.0f);
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
        }
    }


    // collsion box
    private void OnCollisionEnter2D(Collision2D other)
    {

        Debug.Log("Conk");
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
