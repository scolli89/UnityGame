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
    private Vector2 aimDirection;
    public bool endOfAiming = false;
    public bool isAiming = false;
    public bool usingPower = false;
    public bool endUsingPower = false;

    public bool healing = false;
    public bool laserBoltHealing = false;
    public int healingCount = 0;
    public int HEALING_TIME = 50;  //  healingTime/ 50 = seconds. 
    public int LASER_BOLT_HEALING_TIME = 6;

    public int energyCount = 0;
    private int energyTime = 200;

    [Space]
    [Header("Dash Variables:")]
    public bool isDashing;// = false;
    public float dashSpeed = 15.0f;
    private float dashTime;
    public float startDashTime = 0.167f;
    private Vector2 dashingDirection;

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
    public GameObject crosshair;

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
    public bool hiddenUI;
    GameObject playerClassGameObject;

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


        energyBarController = this.gameObject.transform.GetChild(1).GetComponent<EnergyBarController>();

        ammoController = this.gameObject.transform.GetChild(2).GetComponent<AmmoController>();
        lastArrowsRemaining = ammoRemaining;
        lastEnergy = energy;
        lastHealth = health;

        playerClass = this.gameObject.transform.GetChild(3).GetComponent<PlayerClass>();
        playerClassGameObject = this.gameObject.transform.GetChild(3).gameObject; 


        crosshair.SetActive(false);

        dashTime = startDashTime;
        shockTime = startShockTime;

        toggleUI = false;
        hiddenUI = false; 

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
        if(toggleUI){ // player wants to toggle the UI
            toggleUI = false; 
            hiddenUI = !hiddenUI; 
            //playerClassGameObject.SetActive(hiddenUI); 
            playerClassGameObject.GetComponent<Animator>().enabled = hiddenUI; 
            playerClassGameObject.GetComponent<SpriteRenderer>().enabled = hiddenUI; 
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

    public int getPlayerIndex()
    {
        return playerIndex;
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

    public void setToggleUI(){
        Debug.Log(toggleUI);
        toggleUI = !toggleUI; 
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

            if (energy > 1)
            {
                energy--;

                isDashing = true;
                Instantiate(dashEffect, transform.position, Quaternion.identity);
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



            }

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
            }
            else
            {
                dashTime -= Time.deltaTime;
                rb.velocity = (movementDirection + dashingDirection) * dashSpeed;
                // if you are walking, you go twice as far. 
                // standing still: ::: (<0,0> + <1,0>) * 10 = <10,0>
                // moving::::: (<1,0>+<1,0>) * 10 = <20,0> 
                //rb.velocity = (dashingDirection) * dashSpeed; 
            }
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
        if (endOfAiming)
        {

            Vector2 shootingDirection = crosshair.transform.localPosition;
            shootingDirection.Normalize();
            // if (shootingDirection.magnitude != 0)
            // {
            Vector2 iPosition = transform.position;
            iPosition = iPosition + shootingDirection * ARROW_OFFSET; // this prevents it from hitting the player


            GameObject arrow = Instantiate(arrowPrefab, iPosition, Quaternion.identity);
            LaserController arrowController = arrow.GetComponent<LaserController>();
            arrowController.shooter = gameObject;
            arrowController.velocity = shootingDirection * ARROW_BASE_SPEED; // adjust velocity
            arrow.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
            Destroy(arrow, 2.0f);
            // }
            // else{

            // }




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
            if (ammoRemaining >= ammoReq)
            {
                ammoRemaining -= ammoReq;
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
        //scenerario: 
        //damage == 1
        //Energy = 0
        // health = 2
        energy -= damage; // e = -1

        if (energy < 0)
        {
            energy = 0;
            health -= damage; //carry-over damage mechanic, going through the shield.  // 2+ -1
            Debug.Log(health);


            if (health <= 0)
            {
                // kill the player,
                GameObject.Find("GameLogic").GetComponent<GameLogic>().SpawnArcher(this.gameObject);

                ammoRemaining = DEFAULT_AMMO;
                energy = DEFAULT_ENERGY;
                health = DEFAULT_HEALTH;

            }


        }





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
