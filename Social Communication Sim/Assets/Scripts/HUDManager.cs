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
    [SerializeField] private ResponseData responseDataObject;

    private void Awake()
    {
        scoreObject.initScoreObject();
    }

    /// <summary>
    /// Function <c>FixedUpdate</c> is called by Unity. The body of this particular
    /// variant sets the text contents of the PlayerHUD based on whether the
    /// conversation is currently underway. If true, the player's current response
    /// time is displayed. If false, feedback is provided based on the player's
    /// conversational score.
    /// </summary>
    void FixedUpdate()
    {
        if (!responseDataObject.isOver)
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
        else if (responseDataObject.isOver)
        {
            if (scoreObject.getScore() >= -1 && scoreObject.getScore() < 0)
                GetComponentInChildren<Text>().text = "You performed poorly in this conversation and demonstrated inadequate social skills.";
            else if (scoreObject.getScore() >= 0 && scoreObject.getScore() < 1)
                GetComponentInChildren<Text>().text = "Your performance was acceptable, but there is much room for improvement!";
            else if (scoreObject.getScore() >= 1 && scoreObject.getScore() < 2)
                GetComponentInChildren<Text>().text = "Your performance was average and you demonstrated basic social skills.";
            else if(scoreObject.getScore() >= 2 && scoreObject.getScore() < 3)
                GetComponentInChildren<Text>().text = "You performed well in this conversation and demonstrated good social skills.";
            else if(scoreObject.getScore() >= 3 && scoreObject.getScore() < 4)
                GetComponentInChildren<Text>().text = "You performed very well in this conversation and demonstrated great social skills";
            else if(scoreObject.getScore() >= 4 && scoreObject.getScore() <= 5)
                GetComponentInChildren<Text>().text = "You performed perfectly in this conversation and demonstrated brilliant social skills.";
            GetComponentInChildren<Text>().color = new Color(255.0f, 255.0f, 255.0f);
        }
    }
}
