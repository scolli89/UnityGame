using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialMessageController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject ScoreBoardCanvas;
    public TextMeshProUGUI messageText;
    public float startMessageFadeTimer = 10f;
    private float messageFadeTimer;
    private string[] tutorialMessages = {
        "Welcome To Spacey Spacey Shoot Shoot",
    "You Spawn at the blue bases. Be careful not to fall off the cliff",
    "Use WASD, the d-pad, or the Left stick to move",
    "Use A/Cross on your controller, or Left Click to shoot",
    "Shoot Switches to open stuff.",
    "Use B/Circle or Right Click to use your power",
    "Each Class' power is different! Some might be better for different situations.",
    "Use X/Square, Lb/L1, or Left Shift to dash.",
    "Moving while Dashing can change your trajectory.",
    "Be careful, those trails you leave are explosive! They will blow up when shot!",
    "Stand in the Red Bases with your team to complete a level."
     };

    void Start()
    {
        messageText.text = tutorialMessages[0];
        messageFadeTimer = startMessageFadeTimer;

    }
    void Update()
    {
        if (messageFadeTimer >= 0)
        {
            messageFadeTimer -= Time.deltaTime;
        }
        else
        {
            messageText.text = "";
        }

    }

    public void ActivateMessageText(int messageNumber)
    {
        messageFadeTimer = startMessageFadeTimer;
        messageText.text = tutorialMessages[messageNumber];

    }


}
