using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderTripleWallController : MonoBehaviour
{
    // Start is called before the first frame update
    
    public int numberOfWalls;

    // Update is called once per frame
    public void setBuilderOnWalls(GameObject builder){
        for(int i = 0; i < numberOfWalls;i++){
            GameObject childWall = this.transform.GetChild(i).gameObject;
            childWall.GetComponent<BuilderWallController>().builder = builder;
        }
    }
}
