using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMessageTriggerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int messageNumber;
    private TutorialMessageController tutorialMessageController; 
    void Start()
    {
        tutorialMessageController = this.transform.parent.gameObject.GetComponent<TutorialMessageController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            tutorialMessageController.ActivateMessageText(messageNumber);
        }
    }
}
