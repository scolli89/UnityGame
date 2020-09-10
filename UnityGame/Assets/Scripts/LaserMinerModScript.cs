using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMinerModScript : PlayerClass
{
    private int AMMO_REQUIRED = 5; // Energy required. What it costs to use the power.
    [SerializeField]
    private int MAX_MINES = 5; // max number of mines placed? 
    // if we put a limit on the number of mines. 
    // we will need to track each of the mines. 
    // 
    public GameObject laserMinePrefab;
    private List<LaserMineScript> mineList;
    private void Start()
    {
        mineList = new List<LaserMineScript>();

    }

    public override void usePower(Vector2 v)//,GameObject g)
    {
        // called when you press b. 

        // place the mine. 
        // add it to our list. 
        // what if we have too many mines. 


        // where to place the mine
        Transform parentTransform = this.gameObject.transform.parent;
        Vector2 iPosition = new Vector2(parentTransform.position.x, parentTransform.position.y);
        GameObject newMine = Instantiate(laserMinePrefab, iPosition, transform.rotation);
        LaserMineScript newMineScript = newMine.GetComponent<LaserMineScript>();

        newMineScript.SetPlacer(parentTransform.gameObject);

        mineList.Add(newMineScript);
        if (mineList.Count > MAX_MINES)
        {
            Debug.Log("Destroying Mine");
            LaserMineScript destroyMine = mineList[0];
            mineList.RemoveAt(0); 
            Destroy(destroyMine.gameObject);
        }

        // Array[4]: [] [] [] [] 
        // List:  [something 1].  [something 2].  [something 3].  [something 4].  [something 5] .  


    }

    public override int getAmmoReq()
    {
        return AMMO_REQUIRED;
    }

}
