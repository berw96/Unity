using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class <c>HUDManager</c> handles the text contents of the
/// Player's Heads Up Display (HUD). The player's response time
/// is displayed the HUD, indicating how long the player is taking
/// to reply to the NPC. If the player takes more than 10 seconds
/// to provide a response to an NPC statement the on-screen text
/// will read "AWKWARD SILENCE" in a red colour, acting as an
/// indicator to player that they are taking a long time to respond.
/// 
/// To accomplish timing the HUDManager uses a <c>Timer</c> object.
/// There is no limit to how long the player may take to respond, as
/// they may require time to read and understand the choices they're
/// given. However, long response times will severely impact their
/// conversational rating.
/// </summary>

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private ScoreObject scoreObject;

    private void Awake()
    {
        scoreObject.initScoreObject(0.0f);
    }

    void FixedUpdate()
    {
        if (timer.time < 10.0f)
        {
            GetComponentInChildren<Text>().text = "RESPONSE TIME: " + ((int)timer.time) + " s";
            GetComponentInChildren<Text>().color = new Color(255.0f, 255.0f, 255.0f);
        }
        else
        {
            GetComponentInChildren<Text>().text = "AWKWARD SILENCE";
            GetComponentInChildren<Text>().color = new Color(200.0f, 0.0f, 0.0f);
        }
    }
}
