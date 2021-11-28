#define TURTLE_GRAPHICS
#if (UNITY_2019_3_OR_NEWER && TURTLE_GRAPHICS)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lindenmeyer;

namespace TurtleGraphics {

    /// <summary>
    /// Defines common behaviours for Turtle Graphics.
    /// </summary>
    public interface ITurtleGraphics {
        void Grow(GameObject obj);
        void Turn(GameObject obj, float degrees);
        void AddTransform(GameObject obj);
        void RemoveTransform(GameObject obj);
    }

    /// <summary>
    /// This class instantiates game objects which represent branches produced
    /// by the provided L-System results.
    /// </summary>
    public class TutrtleGraphicsManager : MonoBehaviour, ITurtleGraphics {
        private Stack<TransformData> transform_data = new Stack<TransformData>();

        public void ApplyTurtleGraphics(LindenmeyerSystem lm, GameObject obj, int iteration) {
            Debug.Log($"Produced: {lm.Results[iteration]}");
            if (lm is SierpinskiTriangle) {
                foreach (char symbol in lm.Results[iteration]) {
                    if (symbol == 'A' || symbol == 'B') {
                        AddTransform(obj);
                        Grow(obj);
                    } else if (symbol == '+') {
                        Turn(obj, 60f);
                    } else if (symbol == '-') {
                        Turn(obj, -60f);
                    }
                }
            }
            if (lm is KochCurve || lm is DragonCurve) {
                foreach (char symbol in lm.Results[iteration]) {
                    if(symbol == 'F') {
                        AddTransform(obj);
                        Grow(obj);
                    } else if (symbol == '+') {
                        Turn(obj, 90f);
                    } else if (symbol == '-') {
                        Turn(obj, -90f);
                    }
                }
            }
            if (lm is KochSnowflake) {
                foreach (char symbol in lm.Results[iteration]) {
                    if (symbol == 'F') {
                        AddTransform(obj);
                        Grow(obj);
                    } else if (symbol == '+') {
                        Turn(obj, 60f);
                    } else if (symbol == '-') {
                        Turn(obj, -60f);
                    }
                }
            }
            if (lm is SimplePlant) {
                Debug.Log("Simple Plant Detected");
                foreach (char symbol in lm.Results[iteration]) {
                    if (symbol == 'F') {
                        Grow(obj);
                    } else if (symbol == '+') {
                        Turn(obj, 25f);
                    } else if (symbol == '-') {
                        Turn(obj, -25f);
                    } else if (symbol == '[') {
                        AddTransform(obj);
                    } else if (symbol == ']') {
                        RemoveTransform(obj);
                    }
                }
            }
        }

        public void Grow(GameObject obj) {
            Vector3 init_position = obj.transform.position;
            obj.transform.Translate(Vector3.up);
            Debug.DrawLine(init_position, obj.transform.position, Color.white, 10000f, false);
        }

        public void Turn(GameObject obj, float degrees) {
            obj.transform.Rotate(Vector3.forward * degrees);
        }

        public void AddTransform(GameObject obj) {
            TransformData current_transform = new TransformData();
            current_transform.position = obj.transform.position;
            current_transform.rotation = obj.transform.rotation.eulerAngles;
            current_transform.scale = obj.transform.localScale;
            transform_data.Push(current_transform);
        }

        public void RemoveTransform(GameObject obj) {
            TransformData current_transform = transform_data.Pop();
            obj.transform.position = current_transform.position;
            obj.transform.rotation = Quaternion.Euler(current_transform.rotation);
            obj.transform.localScale = current_transform.scale;
        }
    }

    /// <summary>
    /// An abstraction of fundamental transform components.
    /// </summary>
    public struct TransformData {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }
}
#endif
