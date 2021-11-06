#define OBJECT_LOADER
#if (UNITY_2019_3_OR_NEWER && OBJECT_LOADER)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoader : MonoBehaviour
{
    private void Start()
    {

    }

    public void InitializeJengaTower()
    {
        Debug.Log("Jenga tower reset.");
    }
}
#endif
