#define GAME_MANAGER
#if (UNITY_2019_3_OR_NEWER && GAME_MANAGER)
#nullable enable

/*
 * Age of Trees, a Lindenmayer simulator.
 * 
 * By Elliot Walker (2021) | SN: 3368 6408
 * Goldsmiths, University of London
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Lindenmeyer;
using TurtleGraphics;

/// <summary>
/// Initializes the L-Systems and invokes their Turtle Graphics.
/// Attatched game object acts as a spawner for branches.
/// </summary>
public class GameManager : MonoBehaviour {
    private const int max_iterations = 10;
    private const int min_iterations = 1;

    [Tooltip("It is recommended that at most 5 iterations are specified.")]
    [SerializeField] int specified_iterations;
    [SerializeField] int selected_iteration;
    [SerializeField] MODE selected_mode = MODE.DETERMINISTIC;
    private LindenmeyerSystem? selected_system = null;
    private int selected_system_index;

    private readonly List<LindenmeyerSystem> systems = new List<LindenmeyerSystem>();
    private readonly TurtleGraphicsManager tgm = new TurtleGraphicsManager();
    [SerializeField] GameObject branch_prefab;
    [SerializeField] Text l_system_UI_tag;
    [SerializeField] Text iteration_UI_tag;

    private readonly SierpinskiTriangle st = new SierpinskiTriangle("A");
    private readonly KochCurve kc = new KochCurve();
    private readonly KochSnowflake ks = new KochSnowflake();
    private readonly SimplePlantA spa = new SimplePlantA("F");
    private readonly SimplePlantB spb = new SimplePlantB("F");
    private readonly SimplePlantC spc = new SimplePlantC("F");
    private readonly SimplePlantD spd = new SimplePlantD("A");
    private readonly SimplePlantE spe = new SimplePlantE("A");
    private readonly SimplePlantF spf = new SimplePlantF("A");
    private readonly DragonCurve dc = new DragonCurve("FA");

    private void Awake() {
        RegisterSystem(st);
        RegisterSystem(kc);
        RegisterSystem(ks);
        RegisterSystem(spa);
        RegisterSystem(spb);
        RegisterSystem(spc);
        RegisterSystem(spd);
        RegisterSystem(spe);
        RegisterSystem(spf);
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
                break;
            case "Next_Iteration":
                Debug.Log("NEXT ITERATION");
                if (selected_iteration >= specified_iterations - 1)
                    break;
                selected_iteration++;
                break;
        }
        Debug.Log($"Iteration = {selected_iteration}");
        GenerateGraphics();
    }

    public void GenerateGraphics() {
        try {
            l_system_UI_tag.text = "System: " + selected_system.GetType().ToString();
            iteration_UI_tag.text = "Iteration: " + (selected_iteration + 1).ToString();
            selected_system.Mode = selected_mode;
            tgm.ApplyTurtleGraphics(selected_system, gameObject, selected_iteration);
        } catch (IndexOutOfRangeException e) {
            Debug.LogWarning($"{e}");
            selected_iteration = specified_iterations - 1;
            GenerateGraphics();
        } catch (ArgumentOutOfRangeException e) {
            Debug.LogWarning($"{e}");
            selected_iteration = specified_iterations - 1;
            GenerateGraphics();
        } catch (NullReferenceException e) {
            Debug.LogWarning($"{e}");
        } catch (StackOverflowException e) {
            Debug.LogWarning($"{e}");
        }
    }
}
#endif
