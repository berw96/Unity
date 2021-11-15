using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lindenmeyer;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LindenmeyerSystem ls = new LindenmeyerSystem();
        ls.Axiom = new List<char>() {'A'};
        ls.Rule_set = DoSomething;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DoSomething(List<char> symbols) {

    }
}
