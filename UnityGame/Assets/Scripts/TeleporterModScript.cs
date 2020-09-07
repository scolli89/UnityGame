using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterModScript : PlayerClass
{
    // Start is called before the first frame update
    public GameObject purplePanel;
    public GameObject greenPanel;
    public GameObject purplePanelPrefab;
    public GameObject greenPanelPrefab;
    public Sprite panelColor;

    private int AMMO_REQUIRED = 5;
    private bool firstPanel = true;
    private int panelCount;
    private bool purplePanelNext;


    private GameObject oldPanel;
    private TeleporterScript oldPanelScript;
    private GameObject newPanel;
    private TeleporterScript newPanelScript;


    void Start()
    {
        panelCount = 0;
        purplePanelNext = true;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void usePower(Vector2 v)//,GameObject g)
    {
        // where to place the panel
        Transform parentTransform = this.gameObject.transform.parent;
        Vector2 iPosition = new Vector2(parentTransform.position.x, parentTransform.position.y);

        if (panelCount == 0)
        {  // no panels have been placed
            if (purplePanelNext)
            {
                // make purple panel
                oldPanel = Instantiate(purplePanelPrefab, iPosition, transform.rotation);
                purplePanelNext = false;

            }
            else
            {
                // make green panel
                oldPanel = Instantiate(greenPanelPrefab, iPosition, transform.rotation);
                purplePanelNext = true;


            }

            panelCount++;
            //oldPanel = Instantiate(purplePanelPrefab, iPosition, transform.rotation);
            SetPanelColor(oldPanel);


            oldPanelScript = oldPanel.GetComponent<TeleporterScript>();
            oldPanelScript.setModScript(this);
            Debug.Log(oldPanel);
            Debug.Log(oldPanelScript);
            purplePanelNext = false;

        }
        else if (panelCount == 1)
        { // one panels have been placed

            panelCount++;

            if (purplePanelNext)
            {
                // make purple panel
                newPanel = Instantiate(purplePanelPrefab, iPosition, transform.rotation);
                purplePanelNext = false;

            }
            else
            {
                // make green panel
                newPanel = Instantiate(greenPanelPrefab, iPosition, transform.rotation);
                purplePanelNext = true;


            }


            SetPanelColor(newPanel);

            newPanelScript = newPanel.GetComponent<TeleporterScript>();



            oldPanelScript.setSisterPanel(newPanel);
            newPanelScript.setSisterPanel(oldPanel);
            newPanelScript.setModScript(this);
            //oldPanelScript.setBothSisters(newPanelScript);


        }
        else
        {

            panelCount++;
            // two panels have been placed.

            // the shuffle.
            Destroy(oldPanel);
            oldPanel = newPanel;
            oldPanelScript = newPanelScript;

            if (purplePanelNext)
            {
                newPanel = Instantiate(purplePanelPrefab, iPosition, transform.rotation);
                SetPanelColor(newPanel);
            }
            else
            {
                newPanel = Instantiate(greenPanelPrefab, iPosition, transform.rotation);
                SetPanelColor(newPanel);
            }
            newPanelScript = newPanel.GetComponent<TeleporterScript>();
            oldPanelScript.setBothSisters(newPanelScript);
            newPanelScript.setModScript(this);

            purplePanelNext = !purplePanelNext;

        }
    }

    public void DestroyThisPanel(TeleporterScript teleporter)
    {
        if (panelCount == 1)
        {
            // the panel that exists is the one to destroy. 
            // should always be the old panel. no need to shuffle.
            Destroy(oldPanel);
            purplePanelNext = true; 

        }
        else if (panelCount == 2)
        {
            // panel remaining could be either.

            if (teleporter == oldPanelScript)
            {
                // the panel being destroyed is the old one.
                // need to shuffle down the new panel.
                Destroy(oldPanel);
                oldPanel = newPanel;
                oldPanelScript = newPanelScript;
            }
            else if (teleporter == newPanelScript)
            {
                // the panel being destroyed is the newest panel.
                // don't need to shuffle down the panels. 
                Destroy(newPanel);
                purplePanelNext = !purplePanelNext;



            }



        }
        // if (teleporter == oldPanelScript)
        // {
        //     // if the one being destroy is the old panel. Don't need to flip the spawning order.
        //     Destroy(oldPanel);
        //     oldPanel = null;
        //     oldPanelScript = null;
        //     newPanelScript.setSisterPanel(newPanel);

        // }
        // else if (teleporter == newPanelScript)
        // {
        //     // if the one being destroyed is the new panel, Need to flip spawning order boolean.
        //     purplePanelNext = !purplePanelNext;
        //     Destroy(newPanel);

        //     newPanel = null;
        //     newPanelScript = null;
        //     oldPanelScript.setSisterPanel(oldPanel);

        // }
        panelCount--;
    }
    private void SetPanelColor(GameObject panel)
    {
        panel.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = panelColor;
    }
    public override int getAmmoReq()
    {
        // this way, the teleporter can use their power
        if (panelCount == 0)
        {
            return 1;
        }
        return AMMO_REQUIRED;
    }

}
