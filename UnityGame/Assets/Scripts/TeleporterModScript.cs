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

        Debug.Log("Panel: " + panelCount);
        if (panelCount == 0)
        {  // no panels have been placed


            panelCount++;
            oldPanel = Instantiate(purplePanelPrefab, iPosition, transform.rotation);
            SetPanelColor(oldPanel);

            oldPanelScript = oldPanel.GetComponent<TeleporterScript>();
            Debug.Log(oldPanel);
            Debug.Log(oldPanelScript);
            purplePanelNext = false;

        }
        else if (panelCount == 1)
        { // one panels have been placed

            panelCount++;
            newPanel = Instantiate(greenPanelPrefab, iPosition, transform.rotation);
            SetPanelColor(newPanel);
            newPanelScript = newPanel.GetComponent<TeleporterScript>();


            oldPanelScript.setSisterPanel(newPanel);
            newPanelScript.setSisterPanel(oldPanel);
            //oldPanelScript.setBothSisters(newPanelScript);
            purplePanelNext = true;

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

            purplePanelNext = !purplePanelNext;

        }
    }

    private void SetPanelColor(GameObject panel)
    {
        panel.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = panelColor;
    }
    public override int getAmmoReq()
    {
        // this way, the teleporter can use their power
        if (firstPanel)
        {
            firstPanel = false;
            return 1;
        }
        return AMMO_REQUIRED;
    }

}
