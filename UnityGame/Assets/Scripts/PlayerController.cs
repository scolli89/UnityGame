using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeController : MonoBehaviour
{

    [Space]
    [Header("Character Attributes:")]
    public float MOVEMENT_BASE_SPEED = 1.0f;
    public float CROSSHAIR_DISTANCE = 10.0f;
    [Space]
    [Header("Character Statistics:")]
    public float movementSpeed;
    private Vector2 movementDirection;
    public bool endOfAiming;
    
    [Space]
    [Header("Character Statistics:")]
    private Rigidbody2D rb;
    public Animator animator;
    public GameObject crosshair;

     [Space]
    [Header("Character Statistics:")]
    public bool yes;

    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //this is called once a frame. Tied to frame rate. 
        // get the change 
        
       

        ProcessInputs();
        // Move is called in FixedUpdate
        //Move();
        Animate();
        Aim();
        Shoot();


    }

    void ProcessInputs(){

        //movementDirection.x = Input.GetAxisRaw("Horizontal");
        //movementDirection.y = Input.GetAxisRaw("Vertical");
        //Debug.Log(movementDirection);

        //Vector3 movement = new Vector3(Input.GetAxis("MoveHorizontal"),Input.GetAxis("MoveVertical"),0.0f);
        movementDirection = new Vector2(Input.GetAxis("MoveHorizontal"),Input.GetAxis("MoveVertical"));
        //movementDirection = new Vector2( Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f,1.0f);
        movementDirection.Normalize();

        endOfAiming = Input.GetButtonUp("Fire1");
        // fire1 is mapped to the left click button. 
        
        



    }
    void FixedUpdate()
    {
        //this is called at a constant rate, 50 times/ second. Not tied to frame rate
        // better to calculate collisions this way because of frame rate drops or something

        //rb.MovePosition(rb.position + movementDirection * movementSpeed * Time.fixedDeltaTime);
        
        Move();

    }

    void Move()
    {
        rb.velocity = movementDirection * movementSpeed * MOVEMENT_BASE_SPEED;
    }

    void Animate()
    {
        // if(movementDirection != Vector2.zero){
        //     animator.SetFloat("Horizontal",movementDirection.x);
        //     animator.SetFloat("vertical",movementDirection.y);
        // }
        // animator.SetFloat("Speed",movementSpeed);
    }


    void Aim()
    {
        if (movementDirection != Vector2.zero){
            crosshair.transform.localPosition = movementDirection * CROSSHAIR_DISTANCE;
        }

    }

    void Shoot(){
        

        if(endOfAiming){

            Vector2 shootingDirection = crosshair.transform.localPosition;
            shootingDirection.Normalize();
            Debug.Log("Fire");
        }

    }
}
