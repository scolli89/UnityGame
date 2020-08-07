using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Space]
    [Header("Character Attributes:")]
    public float MOVEMENT_BASE_SPEED = 7f;
    public float ARROW_BASE_SPEED = 1.0f;
    public float CROSSHAIR_DISTANCE = 5.0f;

    public float BUILDER_POWER_DISTANCE = 2.0f;
    public float AIMING_BASE_PENALTY = 0.1f;
    public float ARROW_OFFSET = 1.2f;
    public float HEALING_WAIT = 2.0f;

    [SerializeField]
    private int playerIndex = 0;

    private bool usingMouse = false;

    [Space]
    [Header("Character Statistics:")]
    public int ammoRemaining = 12;
    public int DEFAULT_AMMO = 10;
    public int DEFAULT_HEALTH = 3;
    public int DEFAULT_ENERGY = 7;
    private int lastArrowsRemaining;
    public int health = 3;
    private int lastHealth;

    public int energy = 7;
    private int lastEnergy;

    public float movementSpeed;
    private Vector2 movementDirection;
    private Vector2 lastMovementDirection = new Vector2(0, -1);

    public bool usingPower = false;
    public bool endUsingPower = false;

    public bool healing = false;
    public bool laserBoltHealing = false;
    public int healingCount = 0;
    public int HEALING_TIME = 50;  //  healingTime/ 50 = seconds. 
    public int LASER_BOLT_HEALING_TIME = 6;

    public int energyCount = 0;
    private int energyTime = 100;
    [Space]
    [Header("Shooting Varaibles:")]
    private Vector2 aimDirection;
    public bool endOfAiming = false;
    public bool isAiming = false;

    public float startAimTime = 0.35f;
    private float aimTime;
    public bool shootFlag = false;
    public int LASER_DAMAGE = 3;

    [Space]
    [Header("Dash Variables:")]
    public bool isDashing;// = false;
    public float dashSpeed = 8f;//15.0f;
    private float dashTime;
    public float startDashTime = 0.6f;//0.167f;
    private Vector2 dashingDirection;
    public float DASH_INFLUCE_FACTOR = 1f;
    public Vector2 lastDashMovementDirection;

    [Space]
    [Header("Shocked Variables:")]
    public bool isShocked = false;
    private Vector2 shockDirection;
    public float PUSH_BACK_FORCE = 100f;
    public float startShockTime = 0.5f;
    private float shockTime;

    [Space]
    [Header("Character Class:")]
    public string className;
    public string modName;

    [Space]
    [Header("References:")]
    private Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public DisplayLevel displayLevel;
    public DisplayLevel lastDisplayLevel;
    public bool allowUnder; 
    public GameObject crosshair;
    public AudioManager audioManager;

    public Animator crosshairAnimator;
    public GameObject dot;
    public GameObject previousDot;
    public float DESTROY_DOT_TIME = 7f;


    private AmmoController ammoController;

    private EnergyBarController energyBarController;

    private PlayerClass playerClass;

    private bool firstUpdate = true;

    [Space]
    [Header("Prefabs:")]
    public GameObject arrowPrefab;
    public GameObject dashEffect;

    Vector3 worldPosition;
    [Space]
    [Header("Character UI:")]
    public bool toggleUI;// 
    public bool UIisVisible;
    GameObject playerClassGameObject;

    private bool animateCrosshair;

    // todos
    // 
    //decouple dash and energy
    // make powers use energy not ammo. 
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
        //        Debug.Log("Player Start");
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        displayLevel = DisplayLevel.overWall;
        allowUnder = true; 
        //lastDisplayLevel = displayLevel; 


        energyBarController = this.gameObject.transform.GetChild(1).GetComponent<EnergyBarController>();

        ammoController = this.gameObject.transform.GetChild(2).GetComponent<AmmoController>();
        lastArrowsRemaining = ammoRemaining;
        lastEnergy = energy;
        lastHealth = health;

        playerClass = this.gameObject.transform.GetChild(3).GetComponent<PlayerClass>();
        playerClassGameObject = this.gameObject.transform.GetChild(3).gameObject;

        //crossHairScript = this.gameObject.transform.GetChild(0).GetComponent<CrossHairScript>(); 
        //crossHairScript = crosshair.GetComponent<CrossHairScript>(); 
        crosshairAnimator = crosshair.GetComponent<Animator>();


        audioManager = FindObjectOfType<AudioManager>();

        crosshair.SetActive(false);
        animateCrosshair = false;

        dashTime = startDashTime;
        shockTime = startShockTime;
        aimTime = startAimTime;

        toggleUI = false;
        UIisVisible = true;

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
        // /*

        // what happens when you are hit with a healing bolt?
        // 1: Do you heal to full then stop?
        // 2: Do you heal six energy? 
        // */

        // // lets talk healing multipliers!!!!
        // //balancing will affect the relatinship between the multipliers. 

        // // not healing because dashing: 0 
        // // regular healing rate: 1 --> how fast is it actually? 1 every 4 seconds? 
        // // effects of healing aura: 4 --> 


        // // determine how fast;
        // float healingFactor = 1; // set to base line // implied !isDashing
        // if(isDashing){
        //     healingFactor *= 0; 
        // } 
        // if(laserBoltHealing){
        //     healingFactor *= 2; 
        // }
        // if(healing){

        // }



        // if (energy >= 7)
        // {
        //     return;
        // }
        // else
        // {
        //     healingCount += healingFactor;
        //     if (healingCount >= LASER_BOLT_HEALING_TIME)
        //     {
        //         energy++;
        //         healingCount = 0;
        //     }
        // }


        if (laserBoltHealing)
        {
            if (energy >= 7)
            {
                laserBoltHealing = false;
                return;
            }
            else
            {
                healingCount++;
                if (healingCount >= LASER_BOLT_HEALING_TIME)
                {
                    energy++;
                    healingCount = 0;
                }
            }
        }

        else if (healing)
        {
            if (energy >= 7)
            {
                healing = false;
                return;
            }
            else
            {
                healingCount++;
                if (healingCount >= HEALING_TIME) // healing time is 50 
                {
                    energy++;
                    healingCount = 0;

                }
            }
        }
        else if (isDashing)
        {
            // multiplier is 0; 

        }

        else if (!isDashing)
        {
            //regerenate dash energy 
            //multiplier is regular 100% 
            if (energy >= 7)
            {
                healing = false;
                return;
            }
            else
            {
                energyCount++;
                if (energyCount >= energyTime)
                {
                    energy++;
                    energyCount = 0;
                }
            }
        }
        else if (isDashing)
        {
            energyCount = 0; // maybe reset it when the player is dashing. 
        }


    }
    void updateUI()
    {
        if (firstUpdate)
        {
            setHealthAmount(health);
            setEnergyAmount(energy);
            setAmmoAmount(ammoRemaining);
            firstUpdate = false;
        }

        if (health != lastHealth)
        {
            lastHealth = health;
            setHealthAmount(health);

        }
        if (energy != lastEnergy)
        {
            lastEnergy = energy;
            setEnergyAmount(energy);
        }
        if (ammoRemaining != lastArrowsRemaining)
        {
            lastArrowsRemaining = ammoRemaining;
            setAmmoAmount(ammoRemaining);
        }
        if (toggleUI)
        { // player wants to toggle the UI
            toggleUI = false;
            UIisVisible = !UIisVisible;
            //playerClassGameObject.SetActive(hiddenUI); 
            playerClassGameObject.GetComponent<Animator>().enabled = UIisVisible;
            playerClassGameObject.GetComponent<SpriteRenderer>().enabled = UIisVisible;
            // if(hiddenUI){
            //     // if the Ui is on
            //     playerClassGameObject.SetActive(false);
            //     hiddenUI = false; 
            // }
            // else if(!hiddenUI){
            //     // if the ui is off

            // }
        }
    }
    void ProcessInputs()
    {
        // with the new system, we will have already gotten the input booleans. Ie isDashing, 
        // if (isDashing)
        // {
        //     //DASH() or something 

        // }
        // else 
        if (isAiming)
        {
            crosshair.SetActive(true);
            //crossHairScript.startAiming(); 

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

    public void setMovementDirection(Vector2 direction)
    {
        movementDirection = direction;
        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
    }

    public void setAimDirection(Vector3 mouse)
    {
        usingMouse = true;
        mouse.z = 0.0f;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        mouse = mouse - transform.position;
        aimDirection = new Vector2(mouse.x, mouse.y);
    }

    // called on hold of aim button
    public void setIsAiming()
    {
        isAiming = true;
        animateCrosshair = true;
        lastDashMovementDirection = movementDirection;
    }

    //called when aim button is released
    public void setIsFiring()
    {
        animateCrosshair = false;
        isAiming = false;
        endOfAiming = true;
        aimTime = startAimTime;
        Shoot();
    }

    public void setIsAimingPower()
    {
        usingPower = true;
    }

    public void setToggleUI()
    {

        toggleUI = true;//!toggleUI; 
    }

    public void setIsFiringPower()
    {
        usingPower = false;
        endUsingPower = true;
        usePower();
    }
    public void setIsDashing()
    {
        if (isDashing == false)
        {
            // if (energy > 1)
            // {
            //energy--;

            isDashing = true;
            GameObject dashParticles = Instantiate(dashEffect, transform.position, Quaternion.identity);
            //garbage collection on dash particles
            Destroy(dashParticles, startDashTime);
            // if not moving dash in the last direction you were moving. default (0,-1)
            if (movementDirection.magnitude == 0)
            {
                // do the backstep/ regular jump 
                // could make it a back step. 
                // 
                // dashingDirection = -lastMovementDirection; 
                //
                // can't aim behind you, so that is why you dash backwards? 

                dashingDirection = lastMovementDirection;
            }
            else
            {
                // regular roll// flip jump 
                dashingDirection = movementDirection;
            }
            //}
        }
    }
    public void setDualFire()
    {
        if (shootFlag == true)
        {
            isAiming = false;
            endOfAiming = true;
            aimTime = startAimTime;
            Shoot();
        }

    }

    void Move()
    {
        if (isShocked)
        {


            if (shockTime <= 0)
            {
                shockDirection = Vector2.zero;
                isShocked = false;
                shockTime = startShockTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                shockTime -= Time.deltaTime;
                rb.velocity = shockDirection * PUSH_BACK_FORCE;
            }


        }
        else if (isDashing)
        {

            if (dashTime <= 0)
            {
                dashingDirection = Vector2.zero;
                isDashing = false;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
                previousDot = null;

            }
            else
            {
                dashTime -= Time.deltaTime;

                float d = 1 - dashTime / startDashTime;
                d *= DASH_INFLUCE_FACTOR;
                //dashingDirection += movementDirection; 
                //dashingDirection.Normalize(); 
                // dashingDirection += d*movementDirection; 
                // dashingDirection.Normalize();
                // rb.velocity = dashingDirection * dashSpeed;
                if (isAiming)
                {
                    //rb.velocity = ((d * lastDashMovementDirection) + dashingDirection) * dashSpeed;
                    rb.velocity = (d * rejection(lastDashMovementDirection, dashingDirection) + dashingDirection) * dashSpeed;
                }
                else
                {

                    //rb.velocity = ((d * movementDirection) + dashingDirection) * dashSpeed;
                    rb.velocity = (d * rejection(movementDirection, dashingDirection) + dashingDirection) * dashSpeed;

                }
                GameObject thisDot = Instantiate(dot, this.transform.position, Quaternion.identity);
                //thisDot.transform.parent = this.gameObject.transform;

                if (previousDot != null)
                {
                    // if there  is a previous dot, ie this is not the first dot. 
                    //set previous dot's nextDot field to be thisDot. 
                    Vector2 pos = new Vector2((this.transform.position.x + previousDot.transform.position.x) / 2,
                    (this.transform.position.y + previousDot.transform.position.y) / 2);
                    Instantiate(dot, pos, Quaternion.identity);
                }
                previousDot = thisDot;









                // if you are walking, you go twice as far. 
                // standing still: ::: (<0,0> + <1,0>) * 10 = <10,0>
                // moving::::: (<1,0>+<1,0>) * 10 = <20,0> 
                //rb.velocity = (dashingDirection) * dashSpeed; 
            }
            //this is where you modify the dash direction. 
            //dashingDirection += movementDirection; 
            //make the projection here, it will change over time.
            //

        }
        else
        {
            // regular movement. 
            rb.velocity = movementDirection * movementSpeed * MOVEMENT_BASE_SPEED;
            if (movementDirection.magnitude != 0)
            {
                lastMovementDirection = movementDirection;
            }
        }
    }

    private Vector2 rejection(Vector2 a, Vector2 b)
    {
        // vector rejection formula
        // https://en.wikipedia.org/wiki/Vector_projection#Vector_rejection_2
        Vector2 c = a - (
                        ((Vector2.Dot(a, b) / (Vector2.Dot(b, b))
                       ) * (b)));
        return c;
    }

    void Animate()
    {
        if (movementDirection != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movementDirection.x);
            animator.SetFloat("Vertical", movementDirection.y);
        }
        if (animateCrosshair)
        {

            float x = 1f / startAimTime;
            //crosshair.SetActive(true);
            crosshairAnimator.SetFloat("SpeedMultiplier", x);

            crosshairAnimator.SetBool("AimFinished", shootFlag);


        }
        animator.SetFloat("Speed", movementSpeed);
        // display level. 
        if (displayLevel != lastDisplayLevel)
        {
            Debug.Log("Level Change");
            if (displayLevel == DisplayLevel.underWall)
            {
                Debug.Log("Level Down");
                spriteRenderer.sortingLayerName = "UnderWall";
            }
            else if (displayLevel == DisplayLevel.overWall)
            {
                Debug.Log("Level up");
                spriteRenderer.sortingLayerName = "OverWall";
            }

            lastDisplayLevel = displayLevel;
        }


    }

    void Aim()
    {
        if (!usingMouse)
        {
            if (movementDirection.magnitude == 0)
            {
                aimDirection = lastMovementDirection;
            }
            else
            {
                aimDirection = movementDirection;
            }
        }

        aimDirection.Normalize();
        //Debug.Log("AimDirection" + aimDirection);

        crosshair.transform.localPosition = aimDirection * CROSSHAIR_DISTANCE;
        //crosshair.transform.localPosition = movementDirection * CROSSHAIR_DISTANCE;
        if (aimTime < 0 && shootFlag == false)
        {
            audioManager.playSound("Charged");
            //FindObjectOfType<AudioManager>().playSound("Charged");
            shootFlag = true;
        }
        else if (aimTime > 0)
        {
            aimTime -= Time.deltaTime;
        }
    }


    public void AimDual(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            animateCrosshair = true;
            crosshair.SetActive(true);
            //crossHairScript.startAiming(); 
            //movementSpeed *= AIMING_BASE_PENALTY;

            aimDirection = direction;

            aimDirection.Normalize();
            //Debug.Log("AimDirection" + aimDirection);

            crosshair.transform.localPosition = aimDirection * CROSSHAIR_DISTANCE;
            //crosshair.transform.localPosition = movementDirection * CROSSHAIR_DISTANCE;
            if (aimTime < 0 && shootFlag == false)
            {
                audioManager.playSound("Charged");
                //FindObjectOfType<AudioManager>().playSound("Charged");
                shootFlag = true;
            }
            else if (aimTime > 0)
            {
                aimTime -= Time.deltaTime;
            }
        }
        else
        {
            crosshair.SetActive(false);
        }
    }

    void AimPower()
    {
        if (!usingMouse)
        {
            if (movementDirection.magnitude == 0)
            {
                aimDirection = lastMovementDirection;
            }
            else
            {
                aimDirection = movementDirection;
            }
        }

        aimDirection.Normalize();
        //Debug.Log("AimDirection" + aimDirection);

        crosshair.transform.localPosition = aimDirection * BUILDER_POWER_DISTANCE;
        //crosshair.transform.localPosition = movementDirection * CROSSHAIR_DISTANCE;
    }

    void Shoot()
    {
        if (endOfAiming && shootFlag)
        {
            Vector2 shootingDirection = crosshair.transform.localPosition;
            shootingDirection.Normalize();
            Vector2 iPosition = transform.position;
            iPosition = iPosition + shootingDirection * ARROW_OFFSET; // this prevents it from hitting the player

            GameObject arrow = Instantiate(arrowPrefab, iPosition, Quaternion.identity);
            LaserController arrowController = arrow.GetComponent<LaserController>();
            arrowController.shooter = gameObject;
            arrowController.velocity = shootingDirection * ARROW_BASE_SPEED; // adjust velocity
            arrow.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
            Destroy(arrow, 2.0f);
        }

        shootFlag = false;
        aimTime = startAimTime;
        crosshair.SetActive(false);
        isAiming = false;
        animateCrosshair = false;
        endOfAiming = false;
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
            int powerCost = playerClass.getAmmoReq();
            if (energy >= powerCost)
            {
                //ammoRemianing -= powerCost; // what it was before
                energy -= powerCost;
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

    private void OnCollisionEnter2D(Collision2D other)
    {

    }
    //hurt boxs
    private void OnTriggerEnter2D(Collider2D other)
    {
     

        if (other.gameObject.CompareTag("DeathBox"))
        {
            Debug.Log("oh no");
            takeDamage(1);
        }
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
            shockDirection = -movementDirection;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Shockwave")
        {// || other.gameObject.tag == "Enemey"){



            isShocked = true;
            Vector2 shockWavePosition = new Vector2(other.transform.position.x, other.transform.position.y);
            Vector2 myPosition = new Vector2(transform.position.x, transform.position.y);
            /*
            right - up; 
            <1,0> - <0,1>
            = <1,-1>; 

            */



            Vector2 difference = myPosition - shockWavePosition;

            shockDirection = difference;//-lastMovementDirection;
            Debug.Log(shockDirection);
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
        // switch to one hit kills
        ammoRemaining = DEFAULT_AMMO;
        energy = DEFAULT_ENERGY;
        health = DEFAULT_HEALTH;
        GameObject.Find("GameLogic").GetComponent<GameLogic>().SpawnArcher(this.gameObject);



        // energy -= damage * LASER_DAMAGE; // e = -1

        // if (energy < 0)
        // {
        //     energy = 0;
        //     health -= damage; //carry-over damage mechanic, going through the shield.  // 2+ -1
        //     Debug.Log(health);


        //     if (health <= 0)
        //     {
        //         // kill the player,
        //         GameObject.Find("GameLogic").GetComponent<GameLogic>().SpawnArcher(this.gameObject);

        //         ammoRemaining = DEFAULT_AMMO;
        //         energy = DEFAULT_ENERGY;
        //         health = DEFAULT_HEALTH;

        //     }


        // }





        /*
            energy represents shield/ suit level. 
            we want a health bar attached, like in halo, where health is seperate from recharageable shields. 

        */



    }
    public void rechargeEnergyFull()
    {
        laserBoltHealing = true;

    }
    public void setAmmoAmount(int n)
    {
        ammoController.setAmmo(n);
    }
    public void setEnergyAmount(int n)
    {
        energyBarController.setEnergy(n);
    }
    public void setHealthAmount(int n)
    {
        energyBarController.setHealth(n);
    }

    public void setHealth(int health)
    {
        this.health = health;
    }
    public int getHealth()
    {
        return health;
    }
    public enum DisplayLevel
    {
        //let us assume that the default position is being displayed over top of the wall. 
        underWall,
        overWall,
        noWall
    }
}
