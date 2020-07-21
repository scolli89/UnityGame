using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGameManager : MonoBehaviour
{
    [Space]
    [Header("Enemies:")]
    public GameObject RobotDronePrefab;
    public GameObject[] RobotArray;
    public int droneArraySize = 2;

    public int dronesRemaining;


    public int checkCount = 0;
    public int checkMax = 120;
    public float CLOSEST_DISTANCE = 1.5f;


    [Space]
    [Header("Players:")]
    public GameObject[] playerTargets;
    void Start()
    {
        dronesRemaining = droneArraySize;
        if (dronesRemaining > 0)
        {
            Vector2 iPosition = new Vector2(0, 0);

            for (int i = 0; i < droneArraySize; i++)
            {
                // spawn these bad boys. 
                iPosition.x += i;
                iPosition.y += i;
                RobotArray[i] = Instantiate(RobotDronePrefab, iPosition, Quaternion.identity);
                RobotArray[i].GetComponent<RobotDroneController>().droneId = i;
                RobotArray[i].transform.parent = transform; // sets it as a child. 


            }
            setPlayerTargets(playerTargets);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (checkIfAllDead() && dronesRemaining != -10)
        {
            showWinCondition();
        }


        // if (checkCount <= checkMax)
        // {
        //     checkCount++;
        // }
        // else
        // {
        //     checkCount = 0;
        //     if (checkIfAllDead())
        //     {
        //         showWinCondition();
        //     }
        // }


    }


    bool checkIfAllDead()
    {
        if (dronesRemaining > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
        // foreach (GameObject robot in RobotArray)
        // {
        //     if (robot != null)
        //     {
        //         // is atleast one robot is alive
        //         return false;
        //     }
        // }
        // dronesRemaining = 0; 
        // return true;

    }
    void checkPositionsOfRobots()
    {
        foreach (GameObject thisRobot in RobotArray)
        {
            //if(thisRobot.GetComponent<RobotDroneController>().pus)

            foreach (GameObject otherRobot in RobotArray)
            {

                if (thisRobot != otherRobot)
                {
                    Vector2 thisRobotPos = new Vector2(thisRobot.transform.position.x, thisRobot.transform.position.y);
                    Vector2 otherRobotPos = new Vector2(otherRobot.transform.position.x, otherRobot.transform.position.y);
                    if ((thisRobotPos - otherRobotPos).magnitude <= CLOSEST_DISTANCE)
                    {

                        // somehow adjust the position. 


                    }

                }

            }

        }


    }

    void showWinCondition()
    {
        Debug.Log("WINNER");
        dronesRemaining = -10;
    }

    void setPlayerTargets(GameObject[] playerTargets)
    {
        this.playerTargets = playerTargets; // receive the active players 
        // pass these targets to the drones that are still alive. 
        foreach (GameObject robot in RobotArray)
        {
            if (robot != null)
            {
                robot.GetComponent<RobotDroneController>().setPlayerTargets(this.playerTargets);
            }
        }

    }

    public void subtractFromDronesRemaining(int n)
    {
        dronesRemaining -= n;
    }

}
