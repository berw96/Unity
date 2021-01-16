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
    const int capacity = 4;
    private GameManager gm;
    private GameObject gameManagerObject;
    private GameObject responseCanvas;
    private GameObject theSun;
    private GameObject playerHUD;
    private List<GameObject> responses;

    private void Awake()
    {
        gm = this;
        responses = new List<GameObject>(capacity);
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
    public void instantiateObjects()
    {
        gameManagerObject = GameObject.Find("GameManager");
        responseCanvas = GameObject.Find("ResponseCanvas");
        gameManagerObject.transform.SetParent(responseCanvas.transform);
        gameManagerObject.GetComponent<Transform>().position = responseCanvas.transform.position;

        for (int i = 0; i < responses.Capacity; i++)
        {
            responses.Add(GameObject.Find("Response " + i));
            responses[i].transform.SetParent(GameObject.Find(this.name).transform);
            responses[i].GetComponent<Transform>().position = new Vector3(
                responses[i].GetComponent<Transform>().position.x, 
                ((float)-i/6) + 1.2f, 
                responses[i].GetComponent<Transform>().position.z + 1);
        }
        
        theSun = GameObject.Find("Sun");
        playerHUD = GameObject.Find("PlayerHUD");
        playerHUD.transform.SetParent(GameObject.Find("Player").transform);
    }

    public void deleteObject(GameObject obj)
    {
        Destroy(obj);
    }
}
