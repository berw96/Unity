using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    private void Awake()
    {
        gm = this;
    }
    public void f()
    {
        Debug.Log("Game manager here!");
    }
}
