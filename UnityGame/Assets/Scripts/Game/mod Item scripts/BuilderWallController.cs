using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderWallController : MonoBehaviour
{

    public Sprite wallLow;
    public Sprite wallMed;
    public Sprite wallFull;
    public int health;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        health = 3;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = wallFull;
    }


    void Update()
    {

    }


    void updateWallSprite()
    {
        // do i want to make this function respond to hits or like what?
        // should just move everything into an On CollisionEnter2d. This runs when it is hit by a bullet. 
        // when hit by a bullet it losses health.
        // when depleated. it gets destroyed. 


        if (health >= 3)
        {
            spriteRenderer.sprite = wallFull;
            health = 3;
        }
        else if (health == 2)
        {
            spriteRenderer.sprite = wallMed;
        }
        else if (health == 1)
        {
            spriteRenderer.sprite = wallLow;
        }
        else 
        {
            spriteRenderer.sprite = null;
            wallDestroyed();
        }




    }

    void wallDestroyed()
    {
        //todo destroy the wall. 
        Destroy(this.gameObject);
    }

    public void takeDamage(int damage)
    {

        health -= damage; //we subtract a health point
        updateWallSprite(); // then we update the sprite image. 
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "bullet")
        {
            Destroy(other.gameObject);
            health--; //we subtract a health point
            updateWallSprite(); // then we update the sprite image. 
        }
    }
}
