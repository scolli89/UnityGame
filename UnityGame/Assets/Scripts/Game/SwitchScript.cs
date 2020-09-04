using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite switchOnSprite;
    public Sprite switchOffSprite;
    public SpriteRenderer spriteRenderer;
    public bool isActivated;
    public GameObject thingToSwitch;

    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        SetSwitch(false);
    }
    public void SetSwitch(bool isActivated)
    {
        // called after instantition;
        this.isActivated = isActivated;
        if (isActivated)
        {
            spriteRenderer.sprite = switchOnSprite;

        }
        else
        {
            spriteRenderer.sprite = switchOffSprite;

        }
        ToggleThingToSwitch(isActivated);

    }

    public void SetThingToSwitch(GameObject thingToSwitch)
    {
        this.thingToSwitch = thingToSwitch;
    }
    // Update is called once per frame

    public void ToggleSwitch()
    {
        isActivated = !isActivated;
        if (isActivated)
        {
            spriteRenderer.sprite = switchOnSprite;
        }
        else
        {
            spriteRenderer.sprite = switchOffSprite;
        }
        ToggleThingToSwitch(isActivated);
    }
    public void ToggleThingToSwitch(bool active)
    {
        if (thingToSwitch != null)
        {
            thingToSwitch.SetActive(active);
        }
        else
        {
            Debug.Log("Nothing to switch");
        }

    }
}
