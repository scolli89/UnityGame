using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject[] spawnPoints;

    public void SpawnArcher(GameObject archer)
    {
        GameObject spawnPoint = GetRandomSpawnPoint();
        // youtu.be/o6I2HdGxhME?t=397
        // youtu.be/o6I2HdGxhME?t=582
        // got to 10:43
        archer.transform.position = spawnPoint.transform.position;
    }

    GameObject GetRandomSpawnPoint()
    {
        return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
    }
}