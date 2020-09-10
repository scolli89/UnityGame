using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMineScript : MonoBehaviour
{
    private GameObject placer;
    [SerializeField]
    private GameObject trapPrefab;
    public float trapTime;
    // Start is called before the first frame update
    void Start()
    {
        trapTime = 5f;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetPlacer(GameObject placer)
    {
        this.placer = placer;
    }
    public GameObject GetPlacer()
    {
        return placer;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // if a player steps on it. 

            if (other.gameObject != placer)
            {
                // if player who stepped on it was not the one who placed it. 

                Vector2 iPosition = new Vector2(transform.position.x, transform.position.y);
                GameObject t = Instantiate(trapPrefab, iPosition, transform.rotation);
                Destroy(t, trapTime);
                // set the shock on the player.
                PlayerController trappedPlayer = other.gameObject.GetComponent<PlayerController>();
                trappedPlayer.setIsShocked(trapTime, Vector2.zero);
                Destroy(this.gameObject);
                // moving the transform of the playerto the center of the mine. 

            }
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            // do something elseto the enemy, not sure yet what though. 
            //RobotDroneController rd = other.gameObject.GetComponent<RobotDroneController>();

            Destroy(other.gameObject);
        }
    }
}
