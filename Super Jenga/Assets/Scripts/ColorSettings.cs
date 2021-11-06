#define COLOR_SETTINGS
#if (UNITY_2019_3_OR_NEWER && COLOR_SETTINGS)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RandomNumberGeneration.RNG;

public sealed class ColorSettings : MonoBehaviour
{
    private readonly Color DEFAULT_BACKGROUND_COLOR = Color.black;
    private readonly Color RED_BACKGROUND_COLOR = new Color(230.0f / 255.0f, 122.0f / 225.0f, 122.0f / 255.0f);
    private readonly Color GREEN_BACKGROUND_COLOR = new Color(122.0f / 255.0f, 190.0f / 255.0f, 122.0f / 255.0f);
    private readonly Color BLUE_BACKGROUND_COLOR = new Color(0.0f, 120.0f / 255.0f, 1.0f);
    private readonly Color YELLOW_BACKGROUND_COLOR = new Color(1.0f, 224.0f / 255.0f, 120.0f / 255.0f);

    private readonly Color DEFAULT_FLOOR_COLOR = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f);
    private readonly Color RED_FLOOR_COLOR = new Color(200.0f / 255.0f, 100.0f / 225.0f, 100.0f / 255.0f);
    private readonly Color GREEN_FLOOR_COLOR = new Color(100.0f / 255.0f, 200.0f / 255.0f, 100.0f / 255.0f);
    private readonly Color BLUE_FLOOR_COLOR = new Color(0.0f, 100.0f / 255.0f, 1.0f);
    private readonly Color YELLOW_FLOOR_COLOR = new Color(1.0f, 200.0f / 255.0f, 100.0f / 255.0f);

    private RandomNumberGeneration.RNG randomNumberGenerator;

    [SerializeField] GameObject jengaMat;

    void Start()
    {
        randomNumberGenerator = ScriptableObject.CreateInstance<RandomNumberGeneration.RNG>();
        SetCameraBackgroundColor();
    }

    private void SetCameraBackgroundColor()
    {
        int rng = randomNumberGenerator.ReturnRandomlyGeneratedNumber();

        switch (rng)
        {
            case 0:
                Camera.main.backgroundColor = RED_BACKGROUND_COLOR;
                break;
            case 1:
                Camera.main.backgroundColor = GREEN_BACKGROUND_COLOR;
                break;
            case 2:
                Camera.main.backgroundColor = BLUE_BACKGROUND_COLOR;
                break;
            case 3:
                Camera.main.backgroundColor = YELLOW_BACKGROUND_COLOR;
                break;
            default:
                Camera.main.backgroundColor = DEFAULT_BACKGROUND_COLOR;
                break;
        }
        SetJengaMatMeshColor(rng);
    }

    private void SetJengaMatMeshColor(int rng)
    {
        Renderer jengaMatRenderer = jengaMat.GetComponent<Renderer>();
        switch (rng)
        {
            case 0:
                jengaMatRenderer.material.color = RED_FLOOR_COLOR;
                break;
            case 1:
                jengaMatRenderer.material.color = GREEN_FLOOR_COLOR;
                break;
            case 2:
                jengaMatRenderer.material.color = BLUE_FLOOR_COLOR;
                break;
            case 3:
                jengaMatRenderer.material.color = YELLOW_FLOOR_COLOR;
                break;
            default:
                jengaMatRenderer.material.color = DEFAULT_FLOOR_COLOR;
                break;
        }
    }
}
#endif
