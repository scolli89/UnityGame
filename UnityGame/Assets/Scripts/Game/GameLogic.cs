using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void usePower(Vector2 v){//, GameObject g){
        Debug.Log(" PLAYER CLASS Using Power");
    }
    public virtual IEnumerator SpawnArcher(GameObject player){
        return null; 
    }
    
    // public IEnumerator SpawnArcher(GameObject player)
    // {
    //     // just respawn player at the beginning 
    //     PlayerController p = player.GetComponent<PlayerController>();

    //     Transform spawnPoint = GetRandomSpawnPoint();
    //     player.transform.position = spawnPoint.transform.position;

    //     player.GetComponent<PlayerController>().enable(false);
    //     yield return new WaitForSeconds(respawnDelay);
    //     player.GetComponent<PlayerController>().enable(true);
    // }

}
