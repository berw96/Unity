using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    private static string name;
    private static string nickname;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string getName()
    {
        return name;
    }

    string getNickname()
    {
        return nickname;
    }

    void setName(string n)
    {
        name = n;
    }

    void setNickname(string nn)
    {
        nickname = nn;
    }
}
