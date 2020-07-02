using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerId;

    [Space]
    [Header("Character Attributes:")]
    public float MOVEMENT_BASE_SPEED = 1.0f;
    public float ARROW_BASE_SPEED = 1.0f;
    public float CROSSHAIR_DISTANCE = 5.0f;
    public float AIMING_BASE_PENALTY = 0.1f;
    public float ARROW_DOWN_OFFSET = 1.0f;
    public bool usingKeyBoard;

    [Space]
    [Header("Character Statistics:")]
    public int arrowsRemaining = 12;
    public int health = 6; 
    private int lastHealth = 6;
    public float movementSpeed;
    private Vector2 movementDirection;
    private Vector2 aimDirection;
    public bool endOfAiming;
    public bool isAiming;

    [Space]
    [Header("References:")]
    private Rigidbody2D rb;
    public Animator animator;
    public GameObject crosshair;
    public GameObject healthBar;

    [Space]
    [Header("Prefabs:")]
    public GameObject arrowPrefab;

    Vector3 worldPosition;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        crosshair.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //this is called once a frame. Tied to frame rate. 
        // get the change 


        updateHealth();
        ProcessInputs();
        Move(); // Move is called in FixedUpdate
        Animate();
        //Aim();
        Shoot();
        

        


    }


    void updateHealth(){
        // this works assuming health is changed somewhere else. 
        if(health != lastHealth ){
            lastHealth = health;
            healthBar.GetComponent<HealthBarController>().setHealth(health); 

        }
        

    }



    void ProcessInputs()
    {




        if (usingKeyBoard)
        {
            movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
            movementDirection.Normalize();
        }
        else
        {
            movementDirection = new Vector2(Input.GetAxis("MoveHorizontal"), Input.GetAxis("MoveVertical"));
            //movementDirection = new Vector2( Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
            movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
            movementDirection.Normalize();


        }

        endOfAiming = Input.GetButtonUp("Fire1");
        isAiming = Input.GetButton("Fire1");
        // fire1 is mapped to the left click button. 

        if (isAiming)
        {
            Aim();
            crosshair.SetActive(true);
            movementSpeed *= AIMING_BASE_PENALTY;
        }

        //movementDirection.x = Input.GetAxisRaw("Horizontal");
        //movementDirection.y = Input.GetAxisRaw("Vertical");
        //Debug.Log(movementDirection)

        //Vector3 movement = new Vector3(Input.GetAxis("MoveHorizontal"),Input.GetAxis("MoveVertical"),0.0f);


    }

    void Move()
    {
        rb.velocity = movementDirection * movementSpeed * MOVEMENT_BASE_SPEED;
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

            // Vector3 point = new Vector3();
            // Event currentEvent = Event.current;
            // Vector2 mousePos = new Vector2();

            // Camera cam = Camera.main;
            // // Get the mouse position from Event.
            // // Note that the y position from Event is inverted.
            // mousePos.x = Input.mousePosition.x;
            // mousePos.y = cam.pixelHeight - Input.mousePosition.y;
            // point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

            // aimDirection = new Vector2(point.x, point.y);

            Vector3 shootdirection = Input.mousePosition;
            shootdirection.z = 0.0f;
            shootdirection = Camera.main.ScreenToWorldPoint(shootdirection);
            shootdirection = shootdirection - transform.position;
            aimDirection = new Vector2(shootdirection.x, shootdirection.y);

            //aimDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else
        {
            aimDirection = new Vector2(Input.GetAxis("MoveHorizontal"), Input.GetAxis("MoveVertical"));

        }

        aimDirection.Normalize();
        Debug.Log("AimDirection" + aimDirection);

        crosshair.transform.localPosition = aimDirection * CROSSHAIR_DISTANCE;
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
                // need to determine which if the player is shooting down. 
                Vector3 iPosition = transform.position;
                if (shootingDirection.y < 0)
                {
                    iPosition.y = iPosition.y - ARROW_DOWN_OFFSET;
                }


                GameObject arrow = Instantiate(arrowPrefab, iPosition, Quaternion.identity);
                arrow.GetComponent<Rigidbody2D>().velocity = shootingDirection * ARROW_BASE_SPEED; // adjust velocity
                arrow.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
                Destroy(arrow, 2.0f);
            }
            else
            {
                Debug.Log("OUT OF ARROWS");
            }
            crosshair.SetActive(false);
        }

    }


     private void OnCollisionEnter2D(Collision2D other){

         Debug.Log("Bonk");
         if(other.gameObject.tag == "bullet"){
             Destroy(other.gameObject);
             health--; 

         }
     }
        
    
}
