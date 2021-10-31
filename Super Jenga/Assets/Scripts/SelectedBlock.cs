using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockManagement
{
    public class SelectedBlock : ScriptableObject
    {
        private static GameObject reference;

        public static GameObject GetSelectedBlock()
        {
            Debug.Log($"Querying {reference} as selected block.");
            return reference;
        }
        
        public static void SetSelectedBlock(GameObject block)
        {
            {
                if (!block.Equals(reference))
                {
                    DeselectCurrentBlock();
                    reference = block;
                    Debug.Log($"Block {reference} was registered.");
                }
                else
                    Debug.LogWarning("Attempted to selected a block which" +
                        "is already selected.");
            }
        }

        public static void DeselectCurrentBlock()
        {
            Debug.LogWarning("Selected block reference set to NULL.");
            reference = null;
        }
    }
}

