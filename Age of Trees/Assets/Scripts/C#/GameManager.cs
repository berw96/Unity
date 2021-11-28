#define GAME_MANAGER
#if (UNITY_2019_3_OR_NEWER && GAME_MANAGER)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lindenmeyer;
using TurtleGraphics;

public class GameManager : MonoBehaviour {
    private List<LindenmeyerSystem> systems = new List<LindenmeyerSystem>();
    private List<GameObject> branches = new List<GameObject>();
    private TurtleGraphics.TutrtleGraphicsManager tgm = new TutrtleGraphicsManager();

    [SerializeField] GameObject branch;

    private void Start() {

        systems.Add(new SierpinskiTriangle("A"));
        Debug.Log($"Initial state: {systems[0].Current_state}");
        systems[0].ApplyRules(2);
        Debug.Log($"End state: {systems[0].Current_state}");
        tgm.ApplyTurtleGraphics(branch, systems[0], 1);

        foreach (string result in systems[0].Results)
            Debug.Log($"Result {systems[0].Results.IndexOf(result) + 1}: {result}");
    }

    private void GenerateResults(int iterations) {
        // apply respective rules to each L-System
    }

    private void CustomLindenmeyerSystemRuleSet(int iterations) {
        Debug.Log("Did something");
    }

    public void ChangeLindenmeyerSystem(GameObject button) {
        // change the current L-System based on the button pressed.
    }

    public void ChangeResultsIteration(GameObject button) {
        // change the iteration of the current L-System based on the button pressed.
    }

    public void ChangeZoom() {
        // use GUI slider to alter the main camera's zoom level.
    }
}
#endif
