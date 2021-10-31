#define EFFECT_DATA
#if (UNITY_2019_3_OR_NEWER && EFFECT_DATA)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effects
{
    public sealed class EffectData : ScriptableObject
    {
        public enum EFFECT
        {
            NO_EFFECT,
            RESTRICT_TO_BOTTOM,
            DISABLE_CAMERA_ROTATION,
            REDUCE_LIGHTING
        }
    }
}
#endif
