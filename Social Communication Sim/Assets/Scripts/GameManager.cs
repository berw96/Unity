using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class <c>GameManager</c> is attached to a singleton which
/// manages the instantiation, deletion and spawning of
/// other game objects.
/// </summary>

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoreObject scoreObject;
    [SerializeField] private Timer timer;
    bool toggled;
    const int menuButtonCapacity = 2;
    const int responseCapacity = 4;
    private GameManager gm;
    private GameObject gameManagerObject;
    private GameObject responseCanvas;
    private GameObject theSun;
    private GameObject playerHUD;
    private List<GameObject> menuButtons;
    private List<GameObject> responses;

    private void Awake()
    {
        gm = this;
        toggled = false;
        menuButtons = new List<GameObject>(menuButtonCapacity);
        responses = new List<GameObject>(responseCapacity);
        instantiateObjects();
    }

    /// <summary>
    /// Function <c>instantiateObjects</c> initializes the
    /// <c>GameObject</c> variables of the <c>GameManager</c>
    /// class and sets their positions in the world.
    /// 
    /// The <c>responseCanvas</c> object enables the graphical contents
    /// of each response button to be rendered. It is set as the
    /// <c>GameManager</c> object's parent.
    /// 
    /// The for loop adds any game objects to the <c>GameObject</c>
    /// list which adhere to the following naming convention: Response 'x',
    /// where 'x' is their ordinal number. It sets the <c>GameManager</c>
    /// object as their parent and adjusts their positions accordingly
    /// relative to the <c>responseCanvas</c> object.
    /// </summary>
    private void instantiateObjects()
    {
        gameManagerObject = GameObject.Find("GameManager");
        responseCanvas = GameObject.Find("ResponseCanvas");
        gameManagerObject.transform.SetParent(responseCanvas.transform);
        gameManagerObject.GetComponent<Transform>().position = responseCanvas.transform.position;
        theSun = GameObject.Find("Sun");
        playerHUD = GameObject.Find("PlayerHUD");
        playerHUD.SetActive(toggled);
        playerHUD.transform.SetParent(GameObject.Find("Player").transform);

        for (int i = 0; i < menuButtons.Capacity; i++)
        {
            menuButtons.Add(GameObject.Find("MenuButton " + i));
        }
        if (menuButtons[0] != null)
            menuButtons[0].GetComponentInChildren<Text>().text = "START";
        if (menuButtons[1] != null)
            menuButtons[1].GetComponentInChildren<Text>().text = "QUIT";

        for (int i = 0; i < responses.Capacity; i++)
        {
            responses.Add(GameObject.Find("Response " + i));
            responses[i].GetComponent<Transform>().position = new Vector3(
                responses[i].GetComponent<Transform>().position.x - 0.45f,
                ((float)-i / 6) + 1.2f,
                responses[i].GetComponent<Transform>().position.z + 1);
            responses[i].transform.rotation = Quaternion.Euler(0.0f, -45.0f, 0.0f);
        }
    }

    public void toggleSimulation()
    {
        switch (toggled)
        {
            case true:
                toggled = false;
                for (int i = 0; i < responses.Capacity; i++)
                    responses[i].transform.SetParent(null);
                menuButtons[0].GetComponentInChildren<Text>().text = "START";
                playerHUD.SetActive(toggled);
                break;
            case false:
                toggled = true;
                for (int i = 0; i < responses.Capacity; i++)
                    responses[i].transform.SetParent(GameObject.Find(this.name).transform);
                menuButtons[0].GetComponentInChildren<Text>().text = "BACK";
                playerHUD.SetActive(toggled);
                break;
        }
        timer.resetTimer();
    }

    private void FixedUpdate()
    {
        if (toggled)
            timer.incrementTime();
    }

    public void deleteObject(GameObject obj)
    {
        Destroy(obj);
    }

    public void quitSimulation()
    {
        Application.Quit();
    }
}
