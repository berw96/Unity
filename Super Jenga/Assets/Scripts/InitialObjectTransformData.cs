#define INITAILIZE_OBJECT_TRANSFORM_DATA
#if (UNITY_2019_3_OR_NEWER && INITAILIZE_OBJECT_TRANSFORM_DATA)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InitialObjectTransform
{
    public sealed class InitialObjectTransformData : ScriptableObject
    {
        public List<GameObject> gameObjects;

        public List<GameObject> GetGameObjects()
        {
            return gameObjects;
        }
    }
}
#endif
