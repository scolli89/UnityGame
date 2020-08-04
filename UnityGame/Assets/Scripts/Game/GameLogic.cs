using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    // [SerializeField]
    // private GameObject[] spawnPoints;
    
    //private GameObject spawnPointPlacements;
    [SerializeField]
    private GameObject[] playerPrefabs;

    private int numChildren;
    public float respawnDelay = 5f; 


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
    public void SpawnArcher(GameObject player)
    {
        new WaitForSeconds(respawnDelay); 
        GameObject spawnPoint = GetRandomSpawnPoint();
        player.transform.position = spawnPoint.transform.position;
    }
    
    GameObject GetRandomSpawnPoint()
    {
        //return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        return this.transform.GetChild(UnityEngine.Random.Range(0, numChildren)).gameObject;
        
    }
}