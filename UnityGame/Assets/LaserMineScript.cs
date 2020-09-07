using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMineScript : MonoBehaviour
{
    private GameObject placer;
    [SerializeField]
    private GameObject trapPrefab;

    // Start is called before the first frame update
    void Start()
    {

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
                Instantiate(trapPrefab, iPosition, transform.rotation);



            }
        }
    }
}
