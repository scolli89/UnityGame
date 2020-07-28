using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spawnPoints;
    [SerializeField]
    private GameObject[] playerPrefabs;

    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefabs[playerConfigs[i].PlayerClass], spawnPoints[i].transform.position, spawnPoints[i].transform.rotation, gameObject.transform);
            player.GetComponent<InputHandler>().InitializePlayer(playerConfigs[i]);
        }
    }
    public void SpawnArcher(GameObject player)
    {
        GameObject spawnPoint = GetRandomSpawnPoint();
        player.transform.position = spawnPoint.transform.position;
    }
    
    GameObject GetRandomSpawnPoint()
    {
        return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
    }
}