using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool[] switchSettings;
    void Start()
    {
        int numChild = this.transform.childCount;
        for (int i = 0; i < numChild; i++)
        {
            GameObject s = this.transform.GetChild(i).gameObject;
            s.GetComponent<SwitchScript>().SetSwitch(switchSettings[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
