using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private ScoreObject scoreObject;

    private void Awake()
    {
        timer.initTime(0.0f);
        scoreObject.initScoreObject(0.0f);
    }

    void FixedUpdate()
    {
        if (timer.time < 10.0f)
        {
            timer.time += Time.deltaTime;
            GetComponentInChildren<Text>().text = "RESPONSE TIME: " + ((int)timer.time) + "s";
            GetComponentInChildren<Text>().color = new Color(255.0f, 255.0f, 255.0f);
        }
        else
        {
            GetComponentInChildren<Text>().text = "AWKWARD SILENCE";
            GetComponentInChildren<Text>().color = new Color(200.0f, 0.0f, 0.0f);
        }
    }
}
