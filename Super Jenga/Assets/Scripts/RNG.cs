#define RNG
#if (UNITY_2019_3_OR_NEWER && RNG)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Effects.EffectData;

namespace RandomNumberGeneration
{
    public sealed class RNG : ScriptableObject
    {
        private const int minRandomValue = 0;
        private const int maxRandomValue = 4;

        private int randomNumber;

        private void GenerateRandomNumber()
        {
            randomNumber = Random.Range(minRandomValue, maxRandomValue);
        }

        public int ReturnRandomlyGeneratedNumber()
        {
            GenerateRandomNumber();
            Debug.LogWarning($"Random number is ... {randomNumber}");
            return randomNumber;
        }

        public EFFECT ReturnRandomEffect()
        {
            GenerateRandomNumber();
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
}
#endif
