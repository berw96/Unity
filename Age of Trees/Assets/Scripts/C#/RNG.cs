#define RNG
#if (UNITY_2019_3_OR_NEWER && RNG)

using UnityEngine;

namespace RandomNumberGeneration
{
    public sealed class RNG
    {
        private const int minRandomValue = 0;
        private const int maxRandomValue = 5;

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
    }
}
#endif
