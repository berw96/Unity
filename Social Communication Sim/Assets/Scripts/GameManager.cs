using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class <c>GameManager</c> is attatched to a singleton which
/// manages the instantiation, deletion and spawning of
/// other game objects.
/// </summary>
public class GameManager : MonoBehaviour
{
    GameManager gm;
    GameObject gameManagerObject;
    GameObject responseCanvas;
    GameObject theSun;
    private List<GameObject> responses;
    const int capacity = 4;

    private void Awake()
    {
        gm = this;
        responses = new List<GameObject>(capacity);
        instantiateObjects();
    }
    public void instantiateObjects()
    {
        for (int i = 0; i < responses.Capacity; i++)
        {
            // ordninally adds any game objects to the list which adhere to the following naming convention: Response x.
            responses.Add(GameObject.Find("Response " + i));
            responses[i].transform.SetParent(GameObject.Find(this.name).transform);
            // adjust the position of each response in the scene.
            responses[i].GetComponent<Transform>().position = new Vector3(
                responses[i].GetComponent<Transform>().position.x, 
                ((float)-i/6) + 1.2f, 
                responses[i].GetComponent<Transform>().position.z + 1);
        }
        gameManagerObject = GameObject.Find("GameManager");
        responseCanvas = GameObject.Find("ResponseCanvas");
        theSun = GameObject.Find("Sun");
        gameManagerObject.transform.SetParent(responseCanvas.transform);
        gameManagerObject.GetComponent<Transform>().position = responseCanvas.transform.position;
    }
}
