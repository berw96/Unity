using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lindenmeyer;

public class GameManager : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        LindenmeyerSystem ls = new LindenmeyerSystem();
        ls.Axiom = "A";
        ls.Rule_set = DoSomething;
        ls.ApplyRules(ls.Axiom);

        SierpinskiTriangle st = new SierpinskiTriangle("A+B");
        Debug.Log($"Initial state: {st.Current_state}");
        st.ApplyRules(st.Current_state);
        Debug.Log($"New state: {st.Current_state}");
    }

    // Update is called once per frame
    void Update() {
        
    }

    void DoSomething(string symbols) {
        Debug.Log("Do something");
    }
}
