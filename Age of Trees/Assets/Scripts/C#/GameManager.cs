#define GAME_MANAGER
#if (UNITY_2019_3_OR_NEWER && GAME_MANAGER)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lindenmeyer;
using TurtleGraphics;

/// <summary>
/// Initializes the L-Systems and invokes their Turtle Graphics.
/// Attatched game object acts as a spawner for branches.
/// </summary>
public class GameManager : MonoBehaviour {
    private List<LindenmeyerSystem> systems = new List<LindenmeyerSystem>();
    private TurtleGraphics.TutrtleGraphicsManager tgm = new TutrtleGraphicsManager();
    
    private SierpinskiTriangle st = new SierpinskiTriangle("A");
    private KochCurve kc = new KochCurve();
    private KochSnowflake ks = new KochSnowflake();
    private SimplePlant sp = new SimplePlant("A");
    private DragonCurve dc = new DragonCurve("FA");
    

    private void Start() {
        RegisterSystem(st);
        RegisterSystem(kc);
        RegisterSystem(ks);
        RegisterSystem(dc);
        RegisterSystem(sp);
        
        GenerateResults(6);
        tgm.ApplyTurtleGraphics(sp, gameObject, 5);
    }

    private void GenerateResults(int iterations) {
        // apply respective rules to each L-System
        //st.ApplyRules(iterations);
        //dc.ApplyRules(iterations);
        //kc.ApplyRules(iterations);
        //ks.ApplyRules(iterations);
        sp.ApplyRules(iterations);
    }

    private void RegisterSystem(LindenmeyerSystem lm) {
        systems.Add(lm);
    }

    public void ChangeLindenmeyerSystem(GameObject button) {
        // change the current L-System displayed based on the button pressed.
        switch (button.name) {
            case "Previous":
                Debug.Log("Selecting previous L-System");
                break;
            case "Next":
                Debug.Log("Selecting next L-System");
                break;
        }
    }

    public void ChangeResultsIteration(GameObject button) {
        // change the iteration of the current L-System based on the button pressed.
    }

    public void ChangeZoom() {
        // use GUI slider to alter the main camera's zoom level.
    }
}
#endif
