#define RNG
#if (UNITY_2019_3_OR_NEWER && RNG)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Effects.EffectData;

public class RNG : MonoBehaviour
{
    [SerializeField] int minRandomValue;
    [SerializeField] int maxRandomValue;

    private int randomNumber;

    // Start is called before the first frame update
    private void Start()
    {
        GenerateRandomNumber();
    }

    private void GenerateRandomNumber()
    {
        randomNumber = Random.Range(minRandomValue, maxRandomValue);
    }

    public EFFECT ReturnRandomEffect()
    {
        EFFECT effect;

        switch (randomNumber)
        {
            case 0:
                effect = EFFECT.RESTRICT_TO_BOTTOM;
                break;
            case 1:
                effect = EFFECT.DISABLE_CAMERA_ROTATION;
                break;
            case 2:
                effect = EFFECT.REDUCE_LIGHTING;
                break;
            default:
                effect = EFFECT.NO_EFFECT;
                break;
        }

        return effect;
    }
}
#endif
