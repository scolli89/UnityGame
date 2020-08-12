using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    // [SerializeField]
    // private GameObject[] spawnPoints;
    
    //private GameObject spawnPointPlacements;
    [SerializeField]
    private GameObject[] playerPrefabs;

    private int numChildren;
    public float respawnDelay = 3.0f; 


    void Start()
    {
        // for(int i = 0; i< spawnPointPlacements.transform.childCount;i++){
        //     spawnPoints[i] = spawnPointPlacements.transform.GetChild(i).gameObject; 
        // }

        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            //var player = Instantiate(playerPrefabs[playerConfigs[i].PlayerClass], spawnPoints[i].transform.position, spawnPoints[i].transform.rotation, gameObject.transform);
            var player = Instantiate(playerPrefabs[playerConfigs[i].PlayerClass], this.transform.GetChild(i).position, this.transform.GetChild(i).rotation, gameObject.transform);
            player.GetComponent<InputHandler>().InitializePlayer(playerConfigs[i]);
        }
        numChildren = this.transform.childCount;
    }

    public IEnumerator SpawnArcher(GameObject player)
    {
        GameObject spawnPoint = GetRandomSpawnPoint();
        player.transform.position = spawnPoint.transform.position;
        
        player.GetComponent<PlayerController>().enable(false);
        yield return new WaitForSeconds(respawnDelay);
        player.GetComponent<PlayerController>().enable(true);
    }
    
    GameObject GetRandomSpawnPoint()
    {
        //return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];


        // SHOULD WORK FOR NOW
        // NOT GREAT
        // SORRY IT SUCKS. 

        // what should we do: other priotities are bigger. O(n).
        // spawn points are always the first x children if we have a count we can change the numChildren to numSpawnPoints. 
        // still have to count the spawn points somehow. 
        GameObject x = this.transform.GetChild(UnityEngine.Random.Range(0, numChildren)).gameObject;
        while(x.gameObject.CompareTag("Player")){
            x = this.transform.GetChild(UnityEngine.Random.Range(0, numChildren)).gameObject;
        }
        return x; 
    }
}