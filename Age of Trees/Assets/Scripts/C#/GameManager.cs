#define GAME_MANAGER
#if (UNITY_2019_3_OR_NEWER && GAME_MANAGER)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lindenmeyer;

public class GameManager : MonoBehaviour {
    // Start is called before the first frame update

    SierpinskiTriangle st = new SierpinskiTriangle("A");
    KochCurve kc = new KochCurve();

    void Start() {
        LindenmeyerSystem ls = new LindenmeyerSystem();
        ls.Axiom = "A";
        ls.Rule_set = DoSomething;
        ls.ApplyRules(3);

        Debug.Log($"Initial state: {st.Current_state}");
        st.ApplyRules(2);
        Debug.Log($"End state: {st.Current_state}");

        foreach (string result in st.Results)
            Debug.Log($"Result {st.Results.IndexOf(result) + 1}: {result}");
    }

    // Update is called once per frame
    void Update() {

    }

    void DoSomething(int iterations) {
        Debug.Log("Did something");
    }
}
#endif
