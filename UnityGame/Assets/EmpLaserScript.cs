using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpLaserScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject empPrefab;
    public Vector2 velocity = new Vector2(0.0f, 0.0f);
    public GameObject shooter;
    public Vector2 offset = new Vector2(0.0f, 0.0f);

    // Update is called once per frame
    void Update()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPosition = currentPosition + velocity * Time.deltaTime;

        RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition + offset, newPosition + offset);
        foreach (RaycastHit2D hit in hits)
        {
            GameObject other = hit.collider.gameObject;
            // do the interaction here. 
            if (other.CompareTag("TrailDot"))
            {

                TrailDotController t = other.GetComponent<TrailDotController>();
                t.sploder = shooter;

                t.setExplode();

                break;

            }
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().setEmpEffect(10f);
                Destroy(gameObject);
                GameObject emp = Instantiate(empPrefab, transform.position, Quaternion.identity);
                EmpScript empScript = emp.GetComponent<EmpScript>();
                empScript.shooter = shooter;

                break;
            }
            if (other.CompareTag("Enemy"))
            { // right now just the cannon. 
                Destroy(gameObject);
                GameObject emp = Instantiate(empPrefab, transform.position, Quaternion.identity);
                EmpScript empScript = emp.GetComponent<EmpScript>();
                empScript.shooter = shooter;

                break;
            }

            if (other.CompareTag("Environment"))
            {
                Destroy(gameObject);
                GameObject emp = Instantiate(empPrefab, transform.position, Quaternion.identity);
                EmpScript empScript = emp.GetComponent<EmpScript>();
                empScript.shooter = shooter;
                break;
            }
            if (other.CompareTag("Shockwave"))
            { // if we don't want the shock wave to block things, remove this if tree
                Destroy(gameObject);
                break;
            }
            if (other.CompareTag("BuilderWall"))
            {
                Destroy(gameObject);
                GameObject emp = Instantiate(empPrefab, transform.position, Quaternion.identity);
                EmpScript empScript = emp.GetComponent<EmpScript>();
                empScript.shooter = shooter;
                // consider doing more damage against builder walls. 
                other.gameObject.GetComponent<BuilderWallController>().takeDamage(-3);
                break;
            }
        }
        transform.position = newPosition;
    }
}
