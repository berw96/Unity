#define GAME_MANAGER
#if (UNITY_2019_3_OR_NEWER && GAME_MANAGER)

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lindenmeyer;
using TurtleGraphics;

/// <summary>
/// Initializes the L-Systems and invokes their Turtle Graphics.
/// Attatched game object acts as a spawner for branches.
/// </summary>
public class GameManager : MonoBehaviour {
    private const int max_iterations = 6;
    private const int min_iterations = 1;

    [SerializeField] int specified_iterations;
    [SerializeField] int selected_iteration;
    [SerializeField] MODE selected_mode;
    private LindenmeyerSystem selected_system;
    private int selected_system_index;

    private List<LindenmeyerSystem> systems = new List<LindenmeyerSystem>();
    private TurtleGraphics.TurtleGraphicsManager tgm = new TurtleGraphicsManager();
    [SerializeField] GameObject branch_prefab;

    private SierpinskiTriangle st = new SierpinskiTriangle("A");
    private KochCurve kc = new KochCurve();
    private KochSnowflake ks = new KochSnowflake();
    private SimplePlant sp = new SimplePlant("A");
    private DragonCurve dc = new DragonCurve("FA");

    private void Start() {
        RegisterSystem(st);
        RegisterSystem(kc);
        RegisterSystem(ks);
        RegisterSystem(sp);
        RegisterSystem(dc);

        tgm.Branch = branch_prefab;

        GenerateResults(specified_iterations);
        GenerateGraphics();
    }

    private void GenerateResults(int iterations) {
        // apply respective rules to each L-System
        if (iterations > max_iterations) {
            Debug.LogWarning($"Number of iterations ({iterations}) is too high." +
                $"Adjusting to {max_iterations} iterations.");

            iterations = max_iterations;
        }
        if (iterations < min_iterations) {
            Debug.LogWarning($"Number of iterations ({iterations}) is too low." +
                $"Adjusting to {min_iterations} iterations.");

            iterations = min_iterations;
        }

        foreach (LindenmeyerSystem system in systems) {
            system.Mode = selected_mode;
            system.ApplyRules(iterations);
        }
    }

    private void RegisterSystem(LindenmeyerSystem lm) {
        systems.Add(lm);
        selected_system = lm;
        selected_system_index = systems.IndexOf(selected_system);
    }

    public void ChangeLindenmeyerSystem(Button button) {
        // change the current L-System displayed based on the button pressed.

        switch (button.name) {
            case "Previous_System":
                Debug.Log("Selecting previous L-System");
                selected_system_index--;
                if (selected_system_index < 0) {
                    selected_system_index = systems.Count - 1;
                }
                break;
            case "Next_System":
                Debug.Log("Selecting next L-System");
                selected_system_index++;
                if (selected_system_index > systems.Count - 1) {
                    selected_system_index = 0;
                }
                break;
        }
        selected_system = systems[selected_system_index];
        Debug.Log($"System = {selected_system.GetType()}");
        GenerateGraphics();
    }

    public void ChangeResultsIteration(Button button) {
        // change the iteration of the current L-System based on the button pressed.
        switch (button.name) {
            case "Previous_Iteration":
                Debug.Log("PREVIOUS ITERATION");
                if (selected_iteration <= min_iterations - 1)
                    break;
                selected_iteration--;
                GenerateGraphics();
                break;
            case "Next_Iteration":
                Debug.Log("NEXT ITERATION");
                if (selected_iteration >= max_iterations - 1)
                    break;
                selected_iteration++;
                GenerateGraphics();
                break;
        }
    }

    public void GenerateGraphics() {
        try {
            tgm.ApplyTurtleGraphics(selected_system, gameObject, selected_iteration);
        } catch (IndexOutOfRangeException) {
            Exception e = new IndexOutOfRangeException();
            Debug.LogWarning($"{e.ToString()}");
        } catch (ArgumentOutOfRangeException) {
            Exception e = new ArgumentOutOfRangeException();
            Debug.LogWarning($"{e.ToString()}");
        } catch (NullReferenceException) {
            Exception e = new NullReferenceException();
            Debug.LogWarning($"{e.ToString()}");
        }
    }
}
#endif
