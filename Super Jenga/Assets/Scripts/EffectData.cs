using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effects
{
    public class EffectData : ScriptableObject
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

