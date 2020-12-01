using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameManager gm;
    private List<GameObject> responses;
    const int capacity = 4;

    private void Awake()
    {
        gm = this;
        responses = new List<GameObject>(capacity);
        instantiateResponses();
    }
    public void instantiateResponses()
    {
        for (int i = 0; i < responses.Capacity; i++)
        {
            // ordninally adds any game objects to the list which adhere to the following naming convention: Response x.
            responses.Add(GameObject.Find("Response " + i));
            responses[i].transform.SetParent(GameObject.Find(this.name).transform);
            responses[i].GetComponent<Transform>().position = new Vector3(
                responses[i].GetComponent<Transform>().position.x, 
                ((float)-i/4), 
                responses[i].GetComponent<Transform>().position.z);
        }
    }
}
